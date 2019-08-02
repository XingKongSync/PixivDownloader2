using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PixivDownloaderGUI
{
    /// <summary>
    /// WebLoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WebLoginWindow : Window
    {
        public string CookieStr;

        public WebLoginWindow()
        {
            InitializeComponent();
            wb.Navigated += Wb_Navigated;
        }

        private void Wb_Navigated(object sender, System.Windows.Forms.WebBrowserNavigatedEventArgs e)
        {
            if (wb.Document?.Body?.InnerHtml?.Contains("click-profile") == true)
            {
                CookieStr = wb.Document?.Cookie;

                if (!string.IsNullOrEmpty(CookieStr))
                {
                    wb.Navigated -= Wb_Navigated;
                    DialogResult = true;
                }
            }
        }
    }
}
