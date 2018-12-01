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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PixivDownloaderGUI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }

        private MainWindowVM _vm;

        public MainWindow()
        {
            Instance = this;

            InitializeComponent();
            _vm = new MainWindowVM(Dispatcher);
            DataContext = _vm;
        }

        private void ListBox_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (sender != null && e != null)
            {
                bool isVEnd = IsVerticalScrollBarAtButtom(e);
                if (isVEnd)
                {
                    //到达底端，开始加载剩余图片
                    _vm.LazyLoadAsync();
                }
            }
        }

        /// <summary>
        /// 判断滚动视图是否已经到达底端
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool IsVerticalScrollBarAtButtom(ScrollChangedEventArgs e)
        {
            bool isAtButtom = false;
            double dVer = e.VerticalOffset;
            double dViewport = e.ViewportHeight;
            double dExtent = e.ExtentHeight;
            if (dVer != 0)
            {
                if (dVer + dViewport == dExtent)
                {
                    isAtButtom = true;
                }
                else
                {
                    isAtButtom = false;
                }
            }
            else
            {
                isAtButtom = false;
            }
            return isAtButtom;
        }
    }
}
