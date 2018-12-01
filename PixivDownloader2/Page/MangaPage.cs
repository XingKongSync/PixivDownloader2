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

        public override void Init(string illustId)
        {
            base.Init(illustId);

            //搜索缩略图url
            SubStringResult result = null;
            int startIndex = 0;
            do
            {
                result = AdvancedSubString.SubString(html, "data-src=\"", "\" data-index=", startIndex, false, false);
                if (result.IsSuccess)
                {
                    ThumbnailUrls.Add(result.ResultText);
                    startIndex = result.EndIndex;
                }
            } while (result != null && result.IsSuccess);
        }

        protected override string GetPageUrl(string illustId)
        {
            string pageUrl = PixivURLHelper.GetMangaPageUrl(IllustId);
            return pageUrl;
        }
    }
}
