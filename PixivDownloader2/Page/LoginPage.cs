using Newtonsoft.Json;
using PixivDownloader2.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XingKongUtils;

namespace PixivDownloader2.Page
{
    public class LoginPage
    {
        public static CookieCollection Cookies { get; private set; }

        public static bool LoginSuccess
        {
            get
            {
                if (Cookies != null && Cookies.Count > 0)
                {
                    return true;
                }
                return false;
            }
        }

        //public static void Init(string cookieStr)
        //{
        //    CookieCollection myCookieContainer = new CookieCollection();

        //    //String 的Cookie　要转成　Cookie型的　并放入CookieContainer中
        //    string[] cookstr = cookieStr.Split(';');

        //    foreach (string str in cookstr)
        //    {
        //        string[] cookieNameValue = str.Split('=');
        //        Cookie ck = new Cookie(cookieNameValue[0].Trim().ToString(), cookieNameValue[1].Trim().ToString());
        //        ck.Domain = ".pixiv.net";
        //        myCookieContainer.Add(ck);
        //    }

        //    Cookies = myCookieContainer;
        //}

        public static void Init(string cookieJson)
        {
            CookieEntity[] cookieEntities = JsonConvert.DeserializeObject<CookieEntity[]>(cookieJson);
            Cookies = CookieEntity.getCookieCollection(cookieEntities);
        }

        public static void Init(string userName, string password)
        {
            string url = PixivURLHelper.GetLoginPageUrl();
            string html = HttpUtils.Get(url, out CookieCollection _cookies);
            var postKeyResult = AdvancedSubString.SubString(html, "name=\"post_key\" value=\"", "\"><input type=", 0, false, false);
            if (postKeyResult != null && postKeyResult.IsSuccess)
            {
                string postKey = postKeyResult.ResultText;

                string postUrl = PixivURLHelper.GetLoginPostUrl();
                var args = HttpUtils.ConstructArgs();
                args["pixiv_id"] = userName;
                //args["captcha"] = string.Empty;
                //args["g_recaptcha_response"] = string.Empty;
                args["password"] = password;
                args["post_key"] = postKey;
                args["source"] = "pc";
                args["ref"] = "wwwtop_accounts_index";
                args["return_to"] = "https://www.pixiv.net/";

                string response = HttpUtils.Post(postUrl, args, HttpUtils.RequestType.Form, out CookieCollection _outputCookies, HttpUtils.DefaultUserAgent, _cookies);

                //防止Cookies中的domain出现空字符串
                string domain = null;
                foreach (Cookie cookie in _outputCookies)
                {
                    if (!string.IsNullOrWhiteSpace(cookie.Domain))
                    {
                        domain = cookie.Domain;
                        break;
                    }
                }
                if (domain != null)
                {
                    foreach (Cookie cookie in _outputCookies)
                    {
                        if (string.IsNullOrWhiteSpace(cookie.Domain))
                        {
                            cookie.Domain = domain;
                        }
                    }
                }

                Cookies = _outputCookies;
            }
        }
    }
}
