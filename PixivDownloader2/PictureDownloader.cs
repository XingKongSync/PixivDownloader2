using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PixivDownloader2
{
    public class PictureDownloader
    {
        private HttpClient downloader;

        public PictureDownloader(string referUrl)
        {
            downloader = new HttpClient();
            downloader.DefaultRequestHeaders.Add("Referer", referUrl);
        }

        public async Task DownloadAsync(string picUrl, string localFileUrl)
        {
            byte[] bytes = await downloader.GetByteArrayAsync(picUrl);
            FileStream fs = new System.IO.FileStream(localFileUrl, System.IO.FileMode.CreateNew);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }
    }
}
