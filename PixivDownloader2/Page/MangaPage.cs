using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixivDownloader2.Page
{
    public class MangaPage : IllustPageBase
    {
        private List<string> thumbnailUrls;

        public List<string> ThumbnailUrls
        {
            get
            {
                thumbnailUrls = thumbnailUrls ?? new List<string>();
                return thumbnailUrls;
            }
            set => thumbnailUrls = value;
        }

        public void Init(string illustId, int pageCount)
        {
            base.Init(illustId);

            //搜索缩略图url
            SubStringResult result = null;
            int startIndex = 0;
            result = AdvancedSubString.SubString(html, "\"original\":\"", "\"},\"", startIndex, false, false);
            if (result.IsSuccess)
            {
                string p0url = result.ResultText;
                ThumbnailUrls.Add(p0url);
                for (int i = 1; i < pageCount; i++)
                {
                    ThumbnailUrls.Add(p0url.Replace("p0", $"p{i}"));
                }
            }
        }

        protected override string GetPageUrl(string illustId)
        {
            string pageUrl = PixivURLHelper.GetIllustUrl(IllustId);
            return pageUrl;
        }
    }
}
