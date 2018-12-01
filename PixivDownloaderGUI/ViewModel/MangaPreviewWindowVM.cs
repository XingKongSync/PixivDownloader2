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

            });
        }

  
    }
}
