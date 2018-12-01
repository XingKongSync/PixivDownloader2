using Newtonsoft.Json;
using PixivDownloader2.Entity;
using PixivDownloader2.Enum;
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
        private string referUrl;

        private RankType rankType;
        private string rankDate;
        private string prevRankDate;
        private string nextRankDate;
        private int rankCount;
        private List<ContentsItem> illustCollection;

        public string ReferUrl { get => referUrl; set => referUrl = value; }
        public RankType RankType { get => rankType; set => rankType = value; }
        public string RankDate { get => rankDate; set => rankDate = value; }
        public string PrevRankDate { get => prevRankDate; set => prevRankDate = value; }
        public string NextRankDate { get => nextRankDate; set => nextRankDate = value; }
        public int RankCount { get => rankCount; set => rankCount = value; }

        public bool HasPrevRank { get { return !string.IsNullOrEmpty(PrevRankDate); } }
        public bool HasNextRank { get { return !string.IsNullOrEmpty(NextRankDate); } }

        public List<ContentsItem> IllustCollection { get { illustCollection = illustCollection ?? new List<ContentsItem>(); return illustCollection; } set => illustCollection = value; }


        #region LazyLoad
        private string url;
        private string json;
        private RankPageJson response;
        #endregion

        public void Init(RankType rankType, int page = 1, string date = null, bool autoLoadAll = true)
        {
            if (string.IsNullOrEmpty(date))
            {
                date = null;
            }
            ReferUrl = PixivURLHelper.GetRankUrl(rankType, false, page, date);
            url = PixivURLHelper.GetRankUrl(rankType, true, page, date);
            json = HttpUtils.Get(url);
            response = JsonConvert.DeserializeObject<RankPageJson>(json);

            this.RankType = rankType;
            RankDate = response.date;
            PrevRankDate = response.prev_date;
            NextRankDate = response.next_date;
            rankCount = response.rank_total;

            //添加第一批加载的作品
            IllustCollection.AddRange(response.contents);

            //是否自动加载剩余作品
            if (autoLoadAll)
            {
                //添加其他批次加载的作品
                while (LazyLoad() != null)
                {
                    Task.Delay(1000).Wait();
                }
            }
        }

        public void Init(RankType rankType, DateTime date, int page = 1, bool autoLoadAll = true)
        {
            Init(rankType, page, date.ToString("yyyyMMdd"), autoLoadAll);
        }

        public bool CanLazyLoad()
        {
            if (response != null && response.next != -1)
            {
                return true;
            }
            return false;
        }

        public List<ContentsItem> LazyLoad()
        {
            if (CanLazyLoad())
            {
                url = PixivURLHelper.GetRankUrl(RankType, true, response.next, RankDate);
                json = HttpUtils.Get(url);
                response = JsonConvert.DeserializeObject<RankPageJson>(json);
                illustCollection.AddRange(response.contents);
                return response.contents; ;
            }
            return null;
        }
    }
}
