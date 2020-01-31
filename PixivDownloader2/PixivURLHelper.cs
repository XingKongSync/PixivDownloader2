using PixivDownloader2.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixivDownloader2
{
    public class PixivURLHelper
    {
        private static readonly string baseUrl = "https://www.pixiv.net/ranking.php";
        private static readonly string illustUrlFormat = "https://www.pixiv.net/artworks/{0}";
        private static readonly string loginPageUrl = "https://accounts.pixiv.net/login?lang=zh&source=pc&view_type=page&ref=wwwtop_accounts_index";
        private static readonly string loginPostUrl = "https://accounts.pixiv.net/api/login?lang=zh";

        public static string GetRankUrl(RankType rankType, bool json = true, int page = 1, string date = null)
        {
            StringBuilder url = new StringBuilder();

            url.Append(baseUrl);
            url.Append("?mode=");
            url.Append(rankType.ToString());
            if (json)
            {
                url.Append("&format=json");
            }
            if (page != 1)
            {
                url.Append("&p=");
                url.Append(page);
            }
            if (!string.IsNullOrEmpty(date))
            {
                url.Append("&date=");
                url.Append(date);
            }
            return url.ToString();
        }

        public static string GetRankUrl(RankType rankType, bool json = true, int page = 1, DateTime? date = null)
        {
            string sDate = null;
            if (date != null)
            {
                sDate = ((DateTime)date).ToString("yyyyMMdd");
            }
            return GetRankUrl(rankType, json, page, sDate);
        }

        public static string GetIllustUrl(string illustId)
        {
            return string.Format(illustUrlFormat, illustId);
        }

        public static string GetLoginPageUrl()
        {
            return loginPageUrl;
        }

        public static string GetLoginPostUrl()
        {
            return loginPostUrl;
        }

        //public static string GetMangaPageUrl(string illustId)
        //{
        //    return string.Format(mangaPageUrlFormat, illustId);
        //}

        //public static string GetMangaBigPageUrl(string illustId, int page)
        //{
        //    return string.Format(mangaBigPageUrlFormat, illustId, page);
        //}
    }
}
