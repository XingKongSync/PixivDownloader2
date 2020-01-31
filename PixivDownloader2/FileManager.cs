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

        /// <summary>
        /// 把漫画缓存文件夹中的文件拷贝到下载文件夹中
        /// </summary>
        /// <param name="url"></param>
        /// <param name="referUrl"></param>
        /// <returns></returns>
        public static async Task<string> CopyMangaPreviewToDownloads(string url, string referUrl)
        {
            string mangaPreviewFile = await MangaPreviewCacheAsync(url, referUrl);
            if (!File.Exists(mangaPreviewFile))
            {
                throw new Exception("下载失败");
            }
            Uri uri = new Uri(url);
            string fileName = Path.GetFileName(uri.LocalPath);
            DirectoryInfo d = new DirectoryInfo(DOWNLOAD_PATH);
            string fileFullPathAndName = Path.Combine(d.FullName, fileName);
            File.Copy(mangaPreviewFile, fileFullPathAndName);
            return fileFullPathAndName;
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

        /// <summary>
        /// 清理缓存目录
        /// </summary>
        public static void CleanCache()
        {
            DeleteOutdateFiles(CACHE_PATH, DateTime.Now.AddDays(-3));
        }

        /// <summary>
        /// 清理多页作品缓存目录
        /// </summary>
        public static void CleanMangaPreviewCache()
        {
            DeleteOutdateFiles(MANGA_PREVIEW_CACHE_PATH, DateTime.Now.AddDays(-3));
        }

        /// <summary>
        /// 删除指定目录下的过期文件
        /// </summary>
        /// <param name="directory">指定目录</param>
        /// <param name="daysbefore">过期时间</param>
        private static void DeleteOutdateFiles(string directory, DateTime daysbefore)
        {
            if (Directory.Exists(directory))
            {
                DirectoryInfo d = new DirectoryInfo(directory);
                string basePath = d.FullName;
                foreach (var file in d.GetFiles())
                {
                    //比较文件创建时间
                    if (file.CreationTime < daysbefore)
                    {
                        //删除过期文件
                        try
                        {
                            file.Delete();
                        }
                        catch (Exception) { }
                    }
                }
            }
        }
    }
}
