using PixivDownloaderGUI.Global;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PixivDownloaderGUI
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (e != null && e.Args != null)
            {
                var argMap = SystemVars.StartupArgs;
                string key = null;
                for (int i = 0; i < e.Args.Length; i++)
                {
                    string arg = e.Args[i];
                    if (arg.StartsWith("-") && arg.Length > 1)
                    {
                        key = arg.Substring(1);
                    }
                    else if (!string.IsNullOrEmpty(key))
                    {
                        argMap[key] = arg;
                    }
                }
            }
        }
    }
}
