using PixivDownloader2;
using PixivDownloader2.Page;
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
    public class MangaBigPageVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Dispatcher dispatcher;

        private string illustId;
        private int page;
        private string thumbnailFilePath;

        private string btDownloadText = "下载";
        private bool btDownloadEnabled = true;

        public string IllustId
        {
            get => illustId;
            set
            {
                illustId = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IllustId)));
            }
        }
        public int Page
        {
            get => page;
            set
            {
                page = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Page)));
            }
        }
        public string ThumbnailFilePath
        {
            get => thumbnailFilePath;
            set
            {
                thumbnailFilePath = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ThumbnailFilePath)));
            }
        }

        #region Download

        public ICommand DownloadCommand { get; set; }

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

        public MangaBigPageVM(Dispatcher dispatcher, string illustId, int page, string thumbnailFilePath)
        {
            this.dispatcher = dispatcher;
            DownloadCommand = new DelegateCommand<object>(DownlaodCommandHandler);

            IllustId = illustId;
            Page = page;
            ThumbnailFilePath = thumbnailFilePath;
        }

        private void DownlaodCommandHandler(object obj)
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
            MangaBigPage bigPage = new MangaBigPage(Page);
            bigPage.Init(IllustId);
            if (string.IsNullOrEmpty(bigPage.LargePicUrl))
            {
                dispatcher?.Invoke(() => { BtDownloadText = "下载失败"; });
            }
            else
            {
                try
                {
                    string filePath = await FileManager.DownloadAsync(bigPage.LargePicUrl, bigPage.PageUrl);
                    dispatcher?.Invoke(() => { BtDownloadText = "下载完成"; });
                }
                catch (Exception)
                {
                    //下载时遇到错误
                    dispatcher?.Invoke(() => { BtDownloadText = "下载失败"; });
                }
              
            }
            BtDownloadEnabled = true;
        }
    }
}
