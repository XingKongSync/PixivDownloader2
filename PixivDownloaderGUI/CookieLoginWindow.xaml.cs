using System;
using System.Collections.Generic;
using System.IO;
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
    /// CookieLoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CookieLoginWindow : Window
    {
        public string CookieStr;
        private string appPath;
        private string cookieFilePath;

        public CookieLoginWindow()
        {
            InitializeComponent();
            appPath = AppDomain.CurrentDomain.BaseDirectory;
            cookieFilePath = System.IO.Path.Combine(appPath, "Cookies.json");
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCookieIfExist(out CookieStr);
            tbCookie.Text = CookieStr;
        }

        private void BtLogin_Click(object sender, RoutedEventArgs e)
        {
            SaveCookie();
            DialogResult = true;
        }

        private bool LoadCookieIfExist(out string cookie)
        {
            cookie = null;

            try
            {
                if (File.Exists(cookieFilePath))
                {
                    cookie = File.ReadAllText(cookieFilePath);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void SaveCookie()
        {
            CookieStr = tbCookie.Text;
            if (string.IsNullOrWhiteSpace(CookieStr))
            {
                return;
            }
            try
            {
                File.WriteAllText(cookieFilePath, CookieStr);
            }
            catch (Exception)
            {

            }
        }
    }
}
