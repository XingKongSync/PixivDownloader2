using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixivDownloaderGUI.Global
{
    class SystemVars
    {
        private static Dictionary<string, string> startupArgs = new Dictionary<string, string>();

        public static Dictionary<string, string> StartupArgs { get => startupArgs; }
    }
}
