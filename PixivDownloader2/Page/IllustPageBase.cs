using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XingKongUtils;

namespace PixivDownloader2.Page
{
    public abstract class IllustPageBase
    {
        protected string illustId;
        protected string html;
        private string pageUrl;

        public string PageUrl { get => pageUrl; private set => pageUrl = value; }

        public string IllustId { get => illustId; set => illustId = value; }

        public virtual void Init(string illustId)
        {
            IllustId = illustId;

            PageUrl = GetPageUrl(IllustId);
            html = HttpUtils.Get(PageUrl, System.Net.SecurityProtocolType.Tls12, cookieAdder);
        }

        protected abstract string GetPageUrl(string illustId);

        private void cookieAdder(HttpWebRequest request)
        {
            request.CookieContainer = request.CookieContainer ?? new CookieContainer();
            request.CookieContainer.Add(LoginPage.Cookies);
        }
    }
}
