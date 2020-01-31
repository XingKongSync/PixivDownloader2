using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixivDownloader2.Page
{
    public class MangaBigPage
    {
        private string illustId;
        private string largePicUrl;
        private string pageUrl;

        public string LargePicUrl { get => largePicUrl; set => largePicUrl = value; }

        public string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(pageUrl))
                {
                    pageUrl = GetPageUrl();
                }
                return pageUrl;
            }
        }

        public MangaBigPage(string illustId, string picUrl)
        {
            this.illustId = illustId;
            LargePicUrl = picUrl;
        }


        private string GetPageUrl()
        {
            return PixivURLHelper.GetIllustUrl(illustId);
        }
    }
}
