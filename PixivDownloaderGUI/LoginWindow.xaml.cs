using PixivDownloaderGUI.Global;
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
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        private string userName;
        private string password;

        public string UserName { get => userName; set => userName = value; }
        public string Password { get => password; set => password = value; }

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btLogin_Click(object sender, RoutedEventArgs e)
        {
            UserName = tbUserName.Text;
            password = tbPassword.Password;

            DialogResult = true;
        }

        private void TryAutoLogin()
        {
            var args = SystemVars.StartupArgs;
            if (args.ContainsKey("u") && args.ContainsKey("p"))
            {
                tbUserName.Text = args["u"];
                tbPassword.Password = args["p"];

                btLogin_Click(null, null);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TryAutoLogin();
        }
    }
}
