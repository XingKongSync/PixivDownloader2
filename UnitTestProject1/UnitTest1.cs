using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PixivDownloader2;
using PixivDownloader2.Enum;
using PixivDownloader2.Page;
using PixivDownloader2.Entity;
using AI;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        //[TestMethod]
        //public void TestUrlGenerate()
        //{
        //    string url = PixivURLHelper.GetRankUrl(PixivDownloader2.Enum.RankType.male);
        //    Console.WriteLine(url);
        //    url = PixivURLHelper.GetRankUrl(PixivDownloader2.Enum.RankType.male);
        //    Console.WriteLine(url);
        //}

        //[TestMethod]
        //public void TestPageGetHtml()
        //{
        //    string url = PixivURLHelper.GetRankUrl(RankType.male);
        //    RankPage rankPage = new RankPage();
        //    rankPage.Url = url;
        //    if (rankPage.Init())
        //    {
        //        Console.WriteLine(rankPage.RawHtml);
        //    }
        //}

        //[TestMethod]
        //public void TestPageGetPicPageUrls()
        //{
        //    string url = PixivURLHelper.GetRankUrl(RankType.male);
        //    RankPage rankPage = new RankPage();
        //    rankPage.Url = url;
        //    if (rankPage.Init())
        //    {
        //        foreach (var pageUrl in rankPage.PicPageUrls)
        //        {
        //            Console.WriteLine(pageUrl);
        //        }
        //    }
        //}

        [TestMethod]
        public void TestGetRankPage()
        {
            RankPage rankPage = new RankPage();
            rankPage.Init(RankType.male);
        }

        [TestMethod]
        public void TestGetPostKey()
        {
            LoginPage.Init("abc", "123");
        }

        [TestMethod]
        public void TestAI()
        {
            Judger judger = new Judger();
            judger.Start();

            string[] images = new string[]
            {
                @"D:\My Media\My Pictures\Favourite\Other-纵向8\0001.JPG",
                @"D:\My Media\My Pictures\Favourite\Other-纵向8\0020.JPG",
                @"D:\My Media\My Pictures\Favourite\Other-纵向8\0100.JPG",
                @"D:\My Media\My Pictures\2019-10-03 国庆回家\IMG_2340.jpg"
            };

            foreach (var img in images)
            {
                float score = judger.GetImageRank(img);

                Console.WriteLine(img);
                Console.Write($"score: {score}, ");
                if (score >= 0.5)
                    Console.WriteLine("是涩图");
                else
                    Console.WriteLine("不是涩图");
                Console.WriteLine();
            }

            judger.Stop();
        }
    }
}
