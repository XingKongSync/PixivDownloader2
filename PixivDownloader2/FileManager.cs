using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixivDownloader2
{
    public class FileManager
    {
        private static readonly string CACHE_PATH = "Caches";
        private static readonly string MANGA_PREVIEW_CACHE_PATH = "MangaPreviewCaches";
        private static readonly string DOWNLOAD_PATH = "Downloads";

        public static async Task<string> CacheAsync(string url, string referUrl)
        {
            //创建本地缓存文件夹
            if (!Directory.Exists(CACHE_PATH))
            {
                Directory.CreateDirectory(CACHE_PATH);
            }
            return await DownloadIfNotExistAsync(url, referUrl, CACHE_PATH);
        }

        public static async Task<string> MangaPreviewCacheAsync(string url, string referUrl)
        {
            //创建本地多图作品预览图缓存文件夹
            if (!Directory.Exists(MANGA_PREVIEW_CACHE_PATH))
            {
                Directory.CreateDirectory(MANGA_PREVIEW_CACHE_PATH);
            }
            return await DownloadIfNotExistAsync(url, referUrl, MANGA_PREVIEW_CACHE_PATH);
        }

        public static async Task<string> DownloadAsync(string url, string referUrl)
        {
            //创建本地下载文件夹
            if (!Directory.Exists(DOWNLOAD_PATH))
            {
                Directory.CreateDirectory(DOWNLOAD_PATH);
            }
            return await DownloadIfNotExistAsync(url, referUrl, DOWNLOAD_PATH);
        }

        private static async Task<string> DownloadIfNotExistAsync(string url, string referUrl, string directory)
        {
            Uri uri = new Uri(url);
            string fileName = Path.GetFileName(uri.LocalPath);
            DirectoryInfo d = new DirectoryInfo(directory);
            string fileFullPathAndName = Path.Combine(d.FullName, fileName);
            if (!File.Exists(fileFullPathAndName))
            {
                //说明此文件不存在，则创建
                PictureDownloader downloader = new PictureDownloader(referUrl);
                await downloader.DownloadAsync(url, Path.Combine(directory, fileName));
            }
            return fileFullPathAndName;
        }
    }
}
