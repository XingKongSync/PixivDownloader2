using Newtonsoft.Json;
using PixivDownloader2.Convert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixivDownloader2.Entity
{
    /// <summary>
    /// 作品要素
    /// </summary>
    public class Illust_content_type
    {
        /// <summary>
        /// 色情
        /// </summary>
        public int sexual { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string grotesque { get; set; }
        /// <summary>
        /// 暴力
        /// </summary>
        public string violent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string homosexual { get; set; }
        /// <summary>
        /// 毒品
        /// </summary>
        public string drug { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string thoughts { get; set; }
        /// <summary>
        /// 反社会
        /// </summary>
        public string antisocial { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string religion { get; set; }
        /// <summary>
        /// 原创
        /// </summary>
        public string original { get; set; }
        /// <summary>
        /// 兽人
        /// </summary>
        public string furry { get; set; }
        /// <summary>
        /// 搞比利
        /// </summary>
        public string bl { get; set; }
        /// <summary>
        /// 搞百合
        /// </summary>
        public string yuri { get; set; }
    }

    /// <summary>
    /// 作品详情
    /// </summary>
    public class ContentsItem
    {
        /// <summary>
        /// 作品标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 上传时间
        /// </summary>
        public string date { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public List<string> tags { get; set; }
        /// <summary>
        /// 缩略图地址
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string illust_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string illust_book_style { get; set; }
        /// <summary>
        /// 作品页数
        /// </summary>
        public string illust_page_count { get; set; }
        /// <summary>
        /// 作者名称
        /// </summary>
        public string user_name { get; set; }
        /// <summary>
        /// 作者头像
        /// </summary>
        public string profile_img { get; set; }
        /// <summary>
        /// 作品要素
        /// </summary>
        public Illust_content_type illust_content_type { get; set; }

        //public string illust_series { get; set; }

        /// <summary>
        /// 作品ID
        /// </summary>
        public int illust_id { get; set; }
        /// <summary>
        /// 宽
        /// </summary>
        public int width { get; set; }
        /// <summary>
        /// 高
        /// </summary>
        public int height { get; set; }
        /// <summary>
        /// 作者ID
        /// </summary>
        public int user_id { get; set; }
        /// <summary>
        /// 排名
        /// </summary>
        public int rank { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int yes_rank { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int rating_count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int view_count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int illust_upload_timestamp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string attr { get; set; }
    }

    public class RankPageJson
    {
        /// <summary>
        /// 排行榜分页内容
        /// </summary>
        public List<ContentsItem> contents { get; set; }
        /// <summary>
        /// male：男性内容，female：女性内容
        /// </summary>
        public string mode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 前一页页码
        /// </summary>
        [JsonConverter(typeof(BoolToIntConverter<int>))]
        public int prev { get; set; }
        /// <summary>
        /// 后一页页码
        /// </summary>
        [JsonConverter(typeof(BoolToIntConverter<int>))]
        public int next { get; set; }
        /// <summary>
        /// 当前页日期
        /// </summary>
        public string date { get; set; }
        /// <summary>
        /// 前一页日期
        /// </summary>
        [JsonConverter(typeof(BoolToStringConverter<string>))]
        public string prev_date { get; set; }
        /// <summary>
        /// 后一页日期
        /// </summary>
        [JsonConverter(typeof(BoolToStringConverter<string>))]
        public string next_date { get; set; }
        /// <summary>
        /// 排行榜总作品个数
        /// </summary>
        public int rank_total { get; set; }
    }
}
