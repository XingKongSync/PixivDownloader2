using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixivDownloader2
{
    public class AdvancedSubString
    {
        public static SubStringResult SubString(string source, string head, string end , int startIndex, bool includeHead, bool includeEnd)
        {
            SubStringResult result = new SubStringResult();

            int s = source.IndexOf(head, startIndex);
            if (s != -1)
            {
                int e = source.IndexOf(end, s + 1);
                if (e != -1)
                {
                    result.IsSuccess = true;
                    result.EndIndex = e;
                    int subStartIndex = s;
                    if (!includeHead)
                    {
                        subStartIndex = s + head.Length;
                    }
                    int subEndIndex = e;
                    if (includeEnd)
                    {
                        subEndIndex = e + end.Length;
                    }
                    result.ResultText = source.Substring(subStartIndex, subEndIndex - subStartIndex);
                }
            }

            return result;
        }
    }

    public class SubStringResult
    {
        public bool IsSuccess = false;
        public string ResultText;
        public int EndIndex = -1;
    }
}
