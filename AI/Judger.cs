using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI
{
    public class Judger
    {
        private Process _tensorFlowProc;

        private List<string> _tempReadLineBuffer = new List<string>();

        private static readonly char[] spliterChars = new char[] { ':' };

        public void Start()
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = GetFullPath("python.exe");
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.RedirectStandardInput = true;
            psi.WorkingDirectory = GetWorkingDirectory();
            psi.Arguments = $"label_image_input.py --input_mean 0 --input_std 255 --model_file new_mobile_model.tflite --label_file class_labels.txt";

            _tensorFlowProc = Process.Start(psi);
            _tensorFlowProc.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            if (_tensorFlowProc != null)
            {
                _tensorFlowProc.Refresh();
                if (!_tensorFlowProc.HasExited)
                {
                    _tensorFlowProc.StandardInput.WriteLine("quit");
                    if (!_tensorFlowProc.WaitForExit(2000))
                    {
                        _tensorFlowProc.Kill();
                    }
                }
                _tensorFlowProc = null;
            }
        }

        private static string GetFullPath(string fileName)
        {
            if (File.Exists(fileName))
                return Path.GetFullPath(fileName);

            var values = Environment.GetEnvironmentVariable("PATH");
            foreach (var path in values.Split(Path.PathSeparator))
            {
                var fullPath = Path.Combine(path, fileName);
                if (File.Exists(fullPath))
                    return fullPath;
            }
            return null;
        }

        private string GetWorkingDirectory()
        {
            string baseDir = Path.GetDirectoryName(GetType().Assembly.Location);
            return Path.Combine(baseDir, "TensorFlow");
        }

        public float GetImageRank(string imgPath)
        {
            if (_tensorFlowProc == null || _tensorFlowProc.HasExited)
                throw new InvalidOperationException("TensorFlow 进程已退出");
            if (string.IsNullOrEmpty(imgPath) || !File.Exists(imgPath))
                throw new ArgumentException("图片路径不正确");

            IEnumerable<string> response = GetResponse(imgPath);
            if (response == null || response.Count() != 2)
                throw new ArgumentException("TensorFlow 返回的结果不对");

            foreach (var line in response)
            {
                if (line.Contains("good"))
                {
                    string[] parts = line.Split(spliterChars, StringSplitOptions.RemoveEmptyEntries);
                    return float.Parse(parts[0]);
                }
            }
            throw new ArgumentException("TensorFlow 返回的结果不对");
        }

        private IEnumerable<string> GetResponse(string input)
        {
            _tempReadLineBuffer.Clear();
            WriteLine(input);

            GetLine("[Begin]");
            _tempReadLineBuffer.Add(ReadLine());
            _tempReadLineBuffer.Add(ReadLine());
            GetLine("[End]");

            return _tempReadLineBuffer;
        }

        private void WriteLine(string input)
        {
            _tensorFlowProc.StandardInput.WriteLine(input);
        }

        private string ReadLine()
        {
            return _tensorFlowProc.StandardOutput.ReadLine();
        }

        private string GetLine(string keyword)
        {
            while (true)
            {
                string line = ReadLine();
                if (line == null)
                    throw new InvalidOperationException("TensorFlow 的响应不正确");
                if (line.ToLower().Contains("error"))
                    throw new InvalidOperationException("TensorFlow 返回了错误");

                if (line.Contains(keyword))
                {
                    return line;
                }
            }
        }
    }
}
