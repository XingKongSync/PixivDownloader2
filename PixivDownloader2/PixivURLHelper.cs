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

        public static string GetRankUrl(RankType rankType, DateTime? date = null)
        {
            string url = baseUrl + "?mode=" + rankType.ToString();
            if (date != null)
            {
                url = url + "&date=" + ((DateTime)date).ToString("yyyyMMdd");
            }
            return url;
        }
    }
}
