using PixivDownloaderGUI.ViewModel;
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

namespace PixivDownloaderGUI.View
{
    /// <summary>
    /// MangaPreviewWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MangaPreviewWindow : Window
    {
        private MangaPreviewWindowVM _vm;

        public MangaPreviewWindow()
        {
            InitializeComponent();
        }

        public MangaPreviewWindow(string illustId, int pageCount): this()
        {
            _vm = new MangaPreviewWindowVM(Dispatcher, illustId, pageCount);
            DataContext = _vm;
        }
    }
}
