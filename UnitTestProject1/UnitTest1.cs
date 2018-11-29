using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PixivDownloader2;
using PixivDownloader2.Enum;
using PixivDownloader2.Page;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestUrlGenerate()
        {
            string url = PixivURLHelper.GetRankUrl(PixivDownloader2.Enum.RankType.male, DateTime.Now);
            Console.WriteLine(url);
            url = PixivURLHelper.GetRankUrl(PixivDownloader2.Enum.RankType.male);
            Console.WriteLine(url);
        }

        [TestMethod]
        public void TestPageGetHtml()
        {
            string url = PixivURLHelper.GetRankUrl(RankType.male);
            RankPage rankPage = new RankPage();
            rankPage.Url = url;
            if (rankPage.Init())
            {
                Console.WriteLine(rankPage.RawHtml);
            }
        }
    }
}
