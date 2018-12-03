using PixivDownloader2;
using PixivDownloader2.Page;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace PixivDownloaderGUI.ViewModel
{
    class MangaPreviewWindowVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Dispatcher dispatcher;
        private string illustId;
        private string alertMessage = "加载中...";

        private ObservableCollection<MangaBigPageVM> thumbnailCollection;

        public ObservableCollection<MangaBigPageVM> ThumbnailCollection
        {
            get
            {
                thumbnailCollection = thumbnailCollection ?? new ObservableCollection<MangaBigPageVM>();
                return thumbnailCollection;
            }
            set
            {
                thumbnailCollection = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ThumbnailCollection)));
            }
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string AlertMessage
        {
            get => alertMessage;
            set
            {
                alertMessage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AlertMessage)));
            }
        }

        public MangaPreviewWindowVM(Dispatcher dispatcher, string illustId)
        {
            this.illustId = illustId;
            this.dispatcher = dispatcher;
            LoadThumbnailAsync();
        }

        /// <summary>
        /// 加载缩略图
        /// 顺便初始化VM用于绑定及下载大图
        /// </summary>
        private void LoadThumbnailAsync()
        {
            Task.Run(async () =>
            {
                MangaPage page = new MangaPage();
                page.Init(illustId);

                if (page != null && page.ThumbnailUrls != null)
                {
                    for (int i = 0; i < page.ThumbnailUrls.Count; i++)
                    {
                        string url = page.ThumbnailUrls[i];
                        string filePath = await FileManager.MangaPreviewCacheAsync(url, page.PageUrl);
                        dispatcher?.Invoke(() => { ThumbnailCollection.Add(new MangaBigPageVM(dispatcher, illustId, i, filePath)); });
                    }
                }

                DispatcherShowMessage("加载完成");
            });
        }

        /// <summary>
        /// 在主线程外显示提示信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private void DispatcherShowMessage(string msg)
        {
            dispatcher?.Invoke(async () =>
            {
                await ShowMessage(msg);
            });
        }

        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private async Task ShowMessage(string msg)
        {
            AlertMessage = msg;
            await Task.Delay(5000);
            AlertMessage = null;
        }
    }
}
