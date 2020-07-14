using GeneralTools.ToolsModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static GeneralTools.ToolsModel.EnumModel;

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
        public static string PostResponse<T>(string url, T obj)
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
        public static string PostXml(HttpParam param)
        {
            param.Method = "POST";
            var str = RequestString(param);
            return str;
        }
        public static string RequestString(HttpParam param)
        {
            var result = "";
            using (var reader = new StreamReader(RequestStream(param), param.Encoding))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
        /// <summary>
        /// 获取响应流
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static Stream RequestStream(HttpParam param)
        {
            #region 处理地址栏参数
            var getParamSb = new StringBuilder();
            if (param.GetParam != null)
            {
                if (param.GetParam is string)
                {
                    getParamSb.Append(param.GetParam.ToString());
                }
                else
                {
                    param.GetParam.GetType().GetProperties().ToList().ForEach(d =>
                    {
                        getParamSb.AppendFormat("{0}={1}&", d.Name, d.GetValue(param.GetParam, null));
                    });
                }
            }
            if (!string.IsNullOrWhiteSpace(getParamSb.ToString().TrimEnd('&')))
            {
                param.Url = string.Format("{0}?{1}", param.Url, getParamSb.ToString().TrimEnd('&'));
            }
            #endregion
            var r = WebRequest.Create(param.Url) as HttpWebRequest;
            if (!string.IsNullOrWhiteSpace(param.CertPath) && !string.IsNullOrWhiteSpace(param.CertPwd))
            {
                ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
                var cer = new X509Certificate2(param.CertPath, param.CertPwd, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);
                r.ClientCertificates.Add(cer);
                #region 暂时不要的
                //ServicePointManager.Expect100Continue = true;
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                //req.ProtocolVersion = HttpVersion.Version11;
                //req.UserAgent = SUserAgent;
                //req.KeepAlive = false;
                //var cookieContainer = new CookieContainer();
                //req.CookieContainer = cookieContainer;
                //req.Timeout = 1000 * 60;
                //req.Headers.Add("x-requested-with", "XMLHttpRequest");
                #endregion
            }
            r.Timeout = param.TimeOut * 1000;
            r.UserAgent = param.UserAgent;
            r.Method = param.Method ?? "POST";
            r.Referer = param.Referer;
            r.CookieContainer = param.CookieContainer;
            r.ContentType = param.ContentType;
            if (param.PostParam != null)
            {
                var postParamString = "";
                if (param.PostParam is string)
                {
                    postParamString = param.PostParam.ToString();
                }
                else if (param.ParamType == HttpParamType.Form)
                {
                    var dicParam = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(param.PostParam));
                    postParamString = dicParam.Aggregate(postParamString, (current, dic) => current + (dic.Key + "=" + dic.Value + "&")).TrimEnd('&');
                }
                else
                {
                    postParamString = JsonConvert.SerializeObject(param.PostParam);
                }
                var bs = param.Encoding.GetBytes(postParamString);
                r.ContentLength = bs.Length;
                using (var rs = r.GetRequestStream())
                {
                    rs.Write(bs, 0, bs.Length);
                }
            }
            return r.GetResponse().GetResponseStream();
        }
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
    }
}
