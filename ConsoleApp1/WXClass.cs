using System;
using System.Configuration;
using BlueThink.Comm;
using DotNet.Utilities;

namespace ConsoleApp1
{
    public class WXClass
    {

        public string GetCacheAccessToken()
        {
            string token = String.Empty;
            if (CacheHelper.GetCache("wxAccessToken") == string.Empty || CacheHelper.GetCache("wxAccessToken") == null)
            {
                token = AccessToken();
                TimeSpan ts = new TimeSpan(1, 30, 0);
                CacheHelper.SetCache("wxAccessToken", token, ts);
            }
            else
            {
                token = CacheHelper.GetCache("wxAccessToken").ToString();
            }

            return token;
        }


        public string AccessToken()
        {
            string AccessToken = string.Empty;
            string CorpID = ConfigurationManager.AppSettings["appid"];
            string Secret = ConfigurationManager.AppSettings["appsecret"];

            AccessToken = GetAccessToken(CorpID, Secret);

            return AccessToken;
        }


        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="corpid"></param>
        /// <param name="corpsecret"></param>
        /// <returns></returns>
        private string GetAccessToken(string corpid, string corpsecret)
        {
            string Gurl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}", corpid, corpsecret);
            string html = HttpHelper.HttpGet(Gurl, "");
            string regex = "\"access_token\":\"(?<token>.*?)\"";

            string token = CRegex.GetText(html, regex, "token");
            return token;
        }

    }
}
