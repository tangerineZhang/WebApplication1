using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Configuration;
using System.Collections;
using System.ServiceModel.Activities;

namespace WXSend
{
    public class QYWeixinHelper
    {
        static string corpid = System.Configuration.ConfigurationManager.AppSettings["appid"].ToString();
        static string corpsecret = System.Configuration.ConfigurationManager.AppSettings["appsecret"].ToString();
        static string messageSendURI = System.Configuration.ConfigurationManager.AppSettings["messageSendURI"].ToString();
        static string getAgentContentURL = System.Configuration.ConfigurationManager.AppSettings["getAgentContent"].ToString();

        /// <summary>
        /// 获取企业微信的accessToken
        /// </summary>
        /// <param name="corpid">企业微信ID</param>
        /// <param name="corpsecret">管理组密钥</param>
        /// <returns></returns>
        static string GetQYAccessToken(string corpid, string corpsecret)
        {
            string getAccessTokenUrl = System.Configuration.ConfigurationManager.AppSettings["getAccessTokenUrl"].ToString();
            string accessToken = "";

            string respText = "";

            
            //获取josn数据
            string url = string.Format(getAccessTokenUrl, corpid, corpsecret);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream resStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(resStream, Encoding.Default);
                respText = reader.ReadToEnd();
                resStream.Close();
            }

            try
            {
                JavaScriptSerializer Jss = new JavaScriptSerializer();
                Dictionary<string, object> respDic = (Dictionary<string, object>)Jss.DeserializeObject(respText);
                //通过键access_token获取值
                accessToken = respDic["access_token"].ToString();
            }
            catch (Exception) { }

            return accessToken;
        }

        /// <summary>
        /// Post数据接口
        /// </summary>
        /// <param name="postUrl">接口地址</param>
        /// <param name="paramData">提交json数据</param>
        /// <param name="dataEncode">编码方式</param>
        /// <returns></returns>
        static string PostWebRequest(string postUrl, string paramData, Encoding dataEncode)
        {
            string ret = string.Empty;
            try
            {
                byte[] byteArray = dataEncode.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";

                webReq.ContentLength = byteArray.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 推送信息
        /// </summary>
        /// <param name="corpid">企业微信ID</param>
        /// <param name="corpsecret">管理组密钥</param>
        /// <param name="paramData">提交的数据json</param>
        /// <param name="dataEncode">编码方式</param>
        /// <returns></returns>
        public static void SendText(string empCode, string message)
        {
            string accessToken = "";
            string postUrl = "";
            string param = "";
            string postResult = "";

            accessToken = GetQYAccessToken(corpid, corpsecret);
            postUrl = string.Format(messageSendURI, accessToken);
            CorpSendText paramData = new CorpSendText(message);
            foreach (string item in empCode.Split('|'))
            {
                //paramData.touser = GetOAUserId(item);//在实际应用中需要判断接收消息的成员是否在系统账号中存在。
                paramData.touser = item;
                param = JsonConvert.SerializeObject(paramData);
                if (paramData.touser != null)
                {
                    postResult = PostWebRequest(postUrl, param, Encoding.UTF8);
                }
                else
                {
                    postResult = "账号" + paramData.touser + "在OA中不存在!";
                }
                CreateLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss") + ":\t" + item + "\t" + param + "\t" + postResult);
            }
        }

        private static void CreateLog(string strlog)
        {
            string str1 = "QYWeixin_log" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            //BS CS应用日志自适应
            //string path = System.Web.HttpContext.Current == null ? Path.GetFullPath("..") + "\\temp\\" : System.Web.HttpContext.Current.Server.MapPath("temp");
            string path = Environment.CurrentDirectory + "\\log\\";
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = Path.Combine(path, str1);
                StreamWriter sw = File.AppendText(path);
                sw.WriteLine(strlog);
                sw.Flush();
                sw.Close();

            }
            catch
            {
            }
        }

        /// <summary>
        /// 获取应用详情
        /// </summary>
        /// <returns></returns>
        public static string GetAgentContent()
        {
            string accessToken = String.Empty;
            string postUrl = String.Empty;
            string param = String.Empty;
            string postResult = String.Empty;
            string respText = String.Empty;
            string userinfos = String.Empty;

            accessToken = GetQYAccessToken(corpid, corpsecret);
            //postUrl = string.Format(getAgentContentURL, accessToken,ConfigurationManager.AppSettings["CorpSendBaseAgentID"]);
            //获取josn数据
            string url = string.Format(getAgentContentURL, accessToken, ConfigurationManager.AppSettings["CorpSendBaseAgentID"]);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream resStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(resStream, Encoding.UTF8);
                respText = reader.ReadToEnd();
                resStream.Close();
            }

            try
            {

                JavaScriptSerializer Jss = new JavaScriptSerializer();
                Dictionary<string, object> respDic = (Dictionary<string, object>)Jss.DeserializeObject(respText);
                if (respDic != null && respDic.Count > 0)
                {
                    foreach (KeyValuePair<string, object> item in respDic)
                    {
                        if (item.Key == "allow_userinfos")
                        {
                            if (item.Value.GetType() == typeof(Dictionary<string, object>))
                            {
                                foreach (KeyValuePair<string, object> itemUser in (Dictionary<string, object>)item.Value)
                                {
                                    //Dictionary<string, object> myDic = Dictionary<string, object>(itemUser.Value);
                                    object[] o = (object[])itemUser.Value;
                                    for (int i = 0; i < o.Count(); i++)
                                    {
                                        foreach (KeyValuePair<string, object> user in (Dictionary<string, object>)o[i])
                                        {
                                            userinfos += user.Value+"|";
                                        }
                                    }

                                }
                            }
                        }
                    }
                  
                }
                return userinfos = userinfos.Substring(0, userinfos.Length - 1);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                return "";
            }
        }

    }
}
