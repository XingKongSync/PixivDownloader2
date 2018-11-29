using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XingKongUtils;

namespace PixivDownloader2.Page
{
    public class RankPage
    {
        private string url;
        private DateTime rankDate;
        private int pictureCount;
        private bool hasPrevPage;
        private bool hasNextPage;
        private string rawHtml;

        public string Url { get => url; set => url = value; }
        public DateTime RankDate { get => rankDate; set => rankDate = value; }
        public int PictureCount { get => pictureCount; set => pictureCount = value; }
        public bool HasPrevPage { get => hasPrevPage; set => hasPrevPage = value; }
        public bool HasNextPage { get => hasNextPage; set => hasNextPage = value; }
        public string RawHtml { get => rawHtml; set => rawHtml = value; }

        public bool Init()
        {
            try
            {
                RawHtml = HttpUtils.Get(Url);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }
    }
}
