using PixivDownloader2;
using PixivDownloader2.Entity;
using PixivDownloader2.Page;
using PixivDownloaderGUI.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace PixivDownloaderGUI.ViewModel
{
    public class IllustVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Dispatcher dispatcher;

        private string title;
        private string userName;
        private int rank;
        private string picFilePath;
        private int width;
        private int height;
        private int multiPageCount;

        private ContentsItem rawContentItem;

        private string btDownloadText = "下载";
        private bool btDownloadEnabled = true;

        private string ReferUrl { get; set; }

        public string Title
        {
            get => title;
            set
            {
                title = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
            }
        }

        public string UserName
        {
            get => userName;
            set
            {
                userName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserName)));
            }
        }

        public int Rank
        {
            get => rank;
            set
            {
                rank = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Rank)));
            }
        }

        public string PicFilePath
        {
            get => picFilePath;
            set
            {
                picFilePath = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PicFilePath)));
            }
        }

        public int Width
        {
            get => width;
            set
            {
                width = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Width)));
            }
        }
        public int Height
        {
            get => height;
            set
            {
                height = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Height)));
            }
        }

        public bool HasMultiPage { get => MultiPageCount > 1; }

        public int MultiPageCount
        {
            get => multiPageCount;
            set
            {
                multiPageCount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MultiPageCount)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasMultiPage)));
            }
        }

        #region Download
        /// <summary>
        /// 下载按钮文本
        /// </summary>
        public string BtDownloadText
        {
            get => btDownloadText;
            set
            {
                btDownloadText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BtDownloadText)));
            }
        }

        /// <summary>
        /// 下载按钮是否启用
        /// </summary>
        public bool BtDownloadEnabled { get => btDownloadEnabled; set => btDownloadEnabled = value; }
        #endregion

        public ContentsItem RawContentItem { get => rawContentItem; set => rawContentItem = value; }

        public ICommand DownloadCommand { get; set; }

        public ICommand OpenDetailCommand { get; set; }

        public IllustVM(Dispatcher dispatcher)
        {
            DownloadCommand = new DelegateCommand<object>(DownloadCommandHandler);
            OpenDetailCommand = new DelegateCommand<object>(OpenDetailCommandHandler);
            this.dispatcher = dispatcher;
        }

        public IllustVM(Dispatcher dispatcher, ContentsItem content, string referUrl) : this(dispatcher)
        {
            ReferUrl = referUrl;
            if (content != null)
            {
                RawContentItem = content;
                Title = content.title;
                UserName = content.user_name;
                Rank = content.rank;
                Width = content.width;
                Height = content.height;
                MultiPageCount = int.Parse(content.illust_page_count);

                LoadThumbnailAsync();
            }
        }

        /// <summary>
        /// 异步加载缩略图
        /// </summary>
        private async void LoadThumbnailAsync()
        {
            string filePath = await FileManager.CacheAsync(RawContentItem.url, ReferUrl);
            dispatcher?.Invoke(()=> { PicFilePath = filePath; });
        }

        /// <summary>
        /// 用点击了下载按钮
        /// </summary>
        /// <param name="obj"></param>
        private void DownloadCommandHandler(object obj)
        {
            if (BtDownloadEnabled)
            {
                BtDownloadText = "下载中...";
                Task.Run(async () =>
                {
                    await DownloadWorker();
                });
            }
        }

        /// <summary>
        /// 实际下载的线程
        /// </summary>
        /// <returns></returns>
        private async Task DownloadWorker()
        {
            BtDownloadEnabled = false;
            MediumPage illust = new MediumPage();
            illust.Init(RawContentItem.illust_id.ToString());
            if (string.IsNullOrEmpty(illust.LargePicUrl))
            {
                dispatcher?.Invoke(() => { BtDownloadText = "下载失败"; });
            }
            else
            {
                string filePath = await FileManager.DownloadAsync(illust.LargePicUrl, ReferUrl);
                dispatcher?.Invoke(() => { BtDownloadText = "下载完成"; });
            }
            BtDownloadEnabled = true;
        }

        /// <summary>
        /// 用户点击了一个多页作品
        /// </summary>
        /// <param name="obj"></param>
        private void OpenDetailCommandHandler(object obj)
        {
            //弹出详细窗口
            MangaPreviewWindow window = new MangaPreviewWindow(RawContentItem.illust_id.ToString());
            window.Show();
        }
    }
}
