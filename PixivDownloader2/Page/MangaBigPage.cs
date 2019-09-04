using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixivDownloader2.Page
{
    public class MangaBigPage : IllustPageBase
    {
        private string largePicUrl;
        private int page;

        public string LargePicUrl { get => largePicUrl; set => largePicUrl = value; }

        public MangaBigPage(int page)
        {
            this.page = page;
        }

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
            return PixivURLHelper.GetMangaBigPageUrl(illustId, page);
        }
    }
}
