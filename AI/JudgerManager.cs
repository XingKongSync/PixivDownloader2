using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI
{
    public class JudgerManager
    {
        public static Lazy<JudgerManager> Instance = new Lazy<JudgerManager>(() => new JudgerManager());

        private Judger _judger;

        private JudgerManager()
        {
            _judger = new Judger();
            _judger.Start();
        }

        private float Judge(string imgPath)
        {
            lock (this)
            {
                try
                {
                    return _judger.GetImageRank(imgPath);
                }
                catch (InvalidOperationException)
                {
                    _judger?.Stop();
                    _judger = new Judger();
                    _judger.Start();
                }

                return 0f;
            }
        }

        /// <summary>
        /// 排队异步对图片进行打分
        /// </summary>
        /// <param name="imgPath"></param>
        /// <returns></returns>
        public async Task<float> JudgeAsync(string imgPath)
        {
            return await Task.Run(() => Judge(imgPath));
        }
    }
}
