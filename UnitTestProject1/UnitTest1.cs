using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PixivDownloader2;
using PixivDownloader2.Enum;
using PixivDownloader2.Page;
using PixivDownloader2.Entity;

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
    }
}
