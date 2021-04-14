using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PixivDownloader2
{
    public class PictureDownloader
    {
        public static PictureDownloader Instance { get => _instance.Value; }

        private static Lazy<PictureDownloader> _instance = new Lazy<PictureDownloader>(() => new PictureDownloader());

        private BlockingCollection<HttpClient> FreeHttpClients = new BlockingCollection<HttpClient>();

        private PictureDownloader() 
        {
            for (int i = 0; i < 10; i++)
            {
                FreeHttpClients.Add(new HttpClient());
            }
        }

        //private HttpClient downloader;

        //public PictureDownloader(string referUrl)
        //{
        //    downloader = new HttpClient();
        //    downloader.DefaultRequestHeaders.Add("Referer", referUrl);
        //}

        public async Task DownloadAsync(string referUrl, string picUrl, string localFileUrl)
        {
            await Task.Run(() => Download(referUrl, picUrl, localFileUrl));
        }

        private void Download(string referUrl, string picUrl, string localFileUrl)
        {
            HttpClient downloader = TakeHttpClient();

            int retryCount = 0;
            Exception exception = null;
            while (true)
            {
                retryCount++;
                try
                {
                    downloader.DefaultRequestHeaders.Remove("Referer");
                    downloader.DefaultRequestHeaders.Add("Referer", referUrl);
                    byte[] bytes = downloader.GetByteArrayAsync(picUrl).Result;
                    FileStream fs = new System.IO.FileStream(localFileUrl, System.IO.FileMode.CreateNew);
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Close();

                    break;
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                if (retryCount > 4)
                {
                    break;
                }
            }

            FreeHttpClients.Add(downloader);
            if (exception != null)
            {
                throw exception;
            }
        }

        private HttpClient TakeHttpClient()
        {
            HttpClient client = null;
            while (null == (client = FreeHttpClients.Take()))
            {

            }
            return client;
        }
    }
}
