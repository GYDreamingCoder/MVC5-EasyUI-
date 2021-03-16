using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace JTS.Utils
{
    /// <summary>
    /// Http工具类
    /// </summary>
    public partial class HttpUtil
    {
        /// <summary>
        ///发送Post请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">参数</param>
        /// <returns>返回string</returns>
        public static string Post(string url, string param, int time = 60000)
        {
            Uri address = new Uri(url);
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json;charset=utf-8"; //"application/x-www-form-urlencoded";
            request.Timeout = time;
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(param == null ? "" : param);
            request.ContentLength = byteData.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
            string result = "";
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                result = reader.ReadToEnd();
            }
            return (result);
        }

        /// <summary>
        /// 发送Get请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns>返回</returns>
        public static string Get(string url)
        {
            Uri address = new Uri(url);
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
            request.Method = "GET";
            request.AllowAutoRedirect = true;
            request.KeepAlive = true;
            request.Headers.Add(HttpRequestHeader.AcceptCharset, "GBK,utf-8;q=0.7,*;q=0.3");
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-cn,zh;q=0.8,en-us;q=0.5,en;q=0.3");
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.ContentType = "application/x-www-form-urlencoded";//"application/json;charset=utf-8"; 
            request.Timeout = 60000;
            string result = "";
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                result = reader.ReadToEnd();
            }
            return (result);
        }
 
        /// <summary>
        /// 发送POST请求
        /// </summary>
        /// <param name="url">Url地址</param>
        /// <param name="data">数据</param>
        public static string SendPost(string url, string data)
        {
            return SendPost(url, "application/x-www-form-urlencoded", data);
        }

        /// <summary>
        /// 发送POST请求
        /// </summary>
        /// <param name="url">Url地址</param>
        /// <param name="contentType">所发送的请求数据的内容类型</param>
        /// <param name="requestData">数据</param>
        public static string SendPost(string url, string contentType, string requestData)
        {
            WebRequest request = (WebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            byte[] postBytes = null;
            request.ContentType = contentType;
            postBytes = Encoding.UTF8.GetBytes(requestData);
            request.ContentLength = postBytes.Length;
            using (Stream outstream = request.GetRequestStream())
            {
                outstream.Write(postBytes, 0, postBytes.Length);
            }
            string result = string.Empty;
            using (WebResponse response = request.GetResponse())
            {
                if (response != null)
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            result = reader.ReadToEnd();
                        }
                    }

                }
            }
            return result;
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="url">Url地址</param>
        /// <param name="contentType">所发送的请求数据的内容类型</param>
        public static string SendGet(string url, string contentType = "application/x-www-form-urlencoded")
        {
            WebRequest request = (WebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = contentType;
            string result = string.Empty;
            using (WebResponse response = request.GetResponse())
            {
                if (response != null)
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            result = reader.ReadToEnd();
                        }
                    }
                }
            }
            return result;
        }
 

    }
}
