using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixivDownloader2.Page
{
    public class MediumPage : IllustPageBase
    {
        private string largePicUrl;
        public string LargePicUrl { get => largePicUrl; set => largePicUrl = value; }

        public override void Init(string illustId)
        {
            base.Init(illustId);
            var result = AdvancedSubString.SubString(html, "\"original\":\"", "\"},\"tags", 0, false, false);
            if (result != null && result.IsSuccess)
            {
                LargePicUrl = result.ResultText.Replace("\\", "");
            }
        }

        protected override string GetPageUrl(string illustId)
        {
            string pageUrl = PixivURLHelper.GetIllustUrl(IllustId);
            return pageUrl;
        }
    }
}
