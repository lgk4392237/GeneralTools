﻿using LitJson;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralTools
{
    /// <summary>
    /// 微信企业号工具类
    /// </summary>
    public class WeiXinQiYeTools
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appId">唯一凭证</param>
        /// <param name="appSecret">唯一凭证密钥</param>
        /// <param name="IsGetNew">是否重新获取</param>
        /// <returns></returns>
        public string GetAccessToken(string appId, string appSecret, bool IsGetNew = false)
        {
            string keyName = "token_" + appSecret;
            string str = "";
            if (IsGetNew == false)
            {

                string Access_token = "";
                if (!string.IsNullOrWhiteSpace(Access_token))
                {
                    str = Access_token.ToString();

                }

            }

            if (string.IsNullOrWhiteSpace(str))
            {

                string url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}", appId, appSecret);
                string res = AjaxTools.GetResponse(url);
                JsonData jd = JsonMapper.ToObject(res);
                try
                {
                    if (jd != null && jd["access_token"] != null)
                    {
                        str = jd["access_token"].ToString();
                    }

                }
                catch
                {
                    str = "";

                }
            }
            return str;
        }
        /// <summary>
        /// 获取jsapi_ticket，有效期7200秒，调用次数有限，不可频繁获取
        /// </summary>
        /// <param name="token">AccessToken</param>
        /// <returns></returns>
        public string GetJsApiTicket(string token)
        {
            string str = string.Empty; 
            string url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/get_jsapi_ticket?access_token={0}", token);
            string res = AjaxTools.GetResponse(url);
            JsonData jd = JsonMapper.ToObject(res);
            if (jd!=null&&jd["ticket"]!=null)
            {
                str = jd["ticket"].ToString();
            }
            return str;
        }
        /// <summary>
        /// 创建jssdk config
        /// </summary>
        /// <param name="appId">唯一凭证</param>
        /// <param name="token">AccessToken</param>
        /// <param name="url">当前页面urL</param>
        /// <param name="debug">是否调试模式</param>
        /// <returns></returns>
        public string CreateJsApiConfig(string appId,string token, string url, bool debug = false)
        {
            string configStr = string.Empty;
            string jsapi_ticket = GetJsApiTicket(token);
            if (string.IsNullOrEmpty(jsapi_ticket))
            {
                return "";
            }
            string noncestr = EncryptionTools.GetNoncestr();
            string timestamp = EncryptionTools.GetTimestamp();
            var packageReq = new RequestHandler();
            packageReq.SetParameter("noncestr", noncestr);
            packageReq.SetParameter("jsapi_ticket", jsapi_ticket);
            packageReq.SetParameter("timestamp", timestamp);
            packageReq.SetParameter("url", url);
            var signature = packageReq.CreateSHA1Sign();
            configStr = "wx.config({ beta: true,debug: " + (debug ? "true" : "false")
                + ", appId: '" + appId
                + "', timestamp: " + timestamp
                + ", nonceStr: '" + noncestr
                + "',signature: '" + signature
                + "',jsApiList: ['checkJsApi','scanQRCode','chooseImage','previewImage','uploadImage','downloadImage']});";

            return configStr;
        }
        /// <summary>
        /// 发送微信模板消息
        /// </summary>
        /// <param name="token">AccessToken</param>
        /// <param name="templateID">模板ID</param>
        /// <param name="toOpenID">接受用户的OpenID</param>
        /// <param name="first">标题</param>
        /// <param name="remark">备注</param>
        /// <param name="keywords">多个消息内容体</param>
        /// <param name="linkUrl">跳转的链接</param>
        /// <returns></returns>
        public string SendTemplateMess(string token, string templateID, string toOpenID, string first, string remark, string[] keywords, string linkUrl = null)
        {
            string postData = "{ \"touser\":\"" + toOpenID + "\","
                               + "\"template_id\":\"" + templateID + "\",";
            if (!string.IsNullOrEmpty(linkUrl)) postData += "\"url\":\"" + linkUrl + "\",";
            postData += "\"topcolor\":\"#FF0000\",\"data\":{\"first\":{\"value\":\"" + first + "\"},";
            for (int i = 0; i < keywords.Length; i++) { postData += "\"keyword" + (i + 1) + "\":{\"value\":\"" + keywords[i] + "\"},"; }
            postData += "\"remark\":{\"value\":\"" + remark + "\"}}}";
            string result = AjaxTools.PostResponse("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + token, postData);
            return result;
        }
    }
}