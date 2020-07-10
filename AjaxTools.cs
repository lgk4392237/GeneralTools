using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace GeneralTools
{
    public class AjaxTools
    {
        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetResponse(string url)
        {
            try
            {
                StringBuilder content = new StringBuilder();
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader sReader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                Char[] sReaderBuffer = new Char[2560];
                int count = sReader.Read(sReaderBuffer, 0, 2560);
                while (count > 0)
                {
                    String tempStr = new String(sReaderBuffer, 0, count);
                    content.Append(tempStr);
                    count = sReader.Read(sReaderBuffer, 0, 2560);
                }
                sReader.Close();
                return content.ToString();
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string PostResponse<T>(string url, T obj)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(obj);
                byte[] b = System.Text.Encoding.UTF8.GetBytes(postData);
                StringBuilder content = new StringBuilder();
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "Post";
                request.ContentLength = b.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(b, 0, b.Length);
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader sReader = new StreamReader(responseStream);
                Char[] sReaderBuffer = new Char[2560];
                int count = sReader.Read(sReaderBuffer, 0, 2560);
                while (count > 0)
                {
                    String tempStr = new String(sReaderBuffer, 0, count);
                    content.Append(tempStr);
                    count = sReader.Read(sReaderBuffer, 0, 2560);
                }
                sReader.Close();
                return content.ToString();
            }
            catch
            {
                return "";
            }
        }
    }
}
