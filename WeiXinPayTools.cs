using GeneralTools.ToolsModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;

namespace GeneralTools
{
    public class WeiXinPayTools
    {
        /// <summary>
        /// 创建支付参数环境
        /// </summary>
        /// <param name="config"></param>
        /// <param name="param">传入 TotalFee，ProductName，OrderNumber（分），OpenId，TimeExpire（20141010121314），商品名称。</param>
        /// <returns></returns>
        public PayResult CreatePayJsApiParameter(PayConfig config, PayParam param)
        {
            var result = new PayResult { ReturnCode = "FAIL", ReturnMsg = "支付失败！" };
            if (param.TotalFee == null) { result.ReturnMsg = "TotalFee 为必填！"; return result; }
            if (param.ProductName == null) { result.ReturnMsg = "ProductName 为必填！"; return result; }
            if (param.OpenId == null) { result.ReturnMsg = "OpenId 为必填！"; return result; }
            if (param.NotifyUrl == null) { result.ReturnMsg = "NotifyUrl 为必填！"; return result; }
            var resultStr = GetUnifiedOrder(config, param);
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(resultStr);
            XmlElement xmlElement = xml.DocumentElement;
            result.ReturnCode = xmlElement.SelectSingleNode("return_code").InnerText;
            result.ReturnMsg = xmlElement.SelectSingleNode("return_msg").InnerText;
            var err_code = xmlElement.SelectSingleNode("err_code") == null ? null : xmlElement.SelectSingleNode("err_code").InnerText;
            var err_code_des = xmlElement.SelectSingleNode("err_code_des") == null ? null : xmlElement.SelectSingleNode("err_code_des").InnerText;
            if (!string.IsNullOrWhiteSpace(err_code_des))
            {
                result.ReturnCode = err_code;
                result.ReturnMsg = err_code_des;
            }
            if (result.IsSuccess)
            {
                try
                {
                    var prepayId = xmlElement.SelectSingleNode("prepay_id").InnerText;
                    var payReq = new RequestHandler();
                    payReq.SetKey(config.MchKey);
                    payReq.SetParameter("appId", config.AppID);
                    payReq.SetParameter("timeStamp", GetTimestamp());
                    payReq.SetParameter("nonceStr", EncryptionTools.GetNoncestr());
                    payReq.SetParameter("package", "prepay_id=" + prepayId);
                    payReq.SetParameter("signType", "MD5");
                    payReq.SetParameter("paySign", payReq.CreateMd5Sign());
                    var payReqXml = payReq.ParseXml();
                    var payReqJson = payReq.ParseJson();
                    result.ReturnMsg = payReqJson;
                    return result;
                }
                catch (Exception ex) 
                {
                    result.ReturnCode = "";
                    result.ReturnMsg = "自定义数据："+param.Attach+",创建支付参数环境";
                    return result;
                }
            }
            return result;
        }
        /// <summary>
        /// 创建微信统一订单
        /// </summary>
        /// <param name="config"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private string GetUnifiedOrder(PayConfig config, PayParam param)
        {
            var req = new RequestHandler();
            param.OrderNumber = param.OrderNumber ?? GenerateOutTradeNo(config.MchID);
            req.SetKey(config.MchKey);
            req.SetParameter("appid", config.AppID);
            req.SetParameter("mch_id", config.MchID);
            req.SetParameter("nonce_str", EncryptionTools.GetNoncestr());
            req.SetParameter("body", param.ProductName);
            req.SetParameter("attach", param.Attach);
            req.SetParameter("out_trade_no", param.OrderNumber);
            req.SetParameter("total_fee", param.TotalFee.ToString());
            req.SetParameter("spbill_create_ip", param.SpbillCreateIp);
            req.SetParameter("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
            req.SetParameter("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
            req.SetParameter("notify_url", param.NotifyUrl);
            req.SetParameter("trade_type", "JSAPI");
            req.SetParameter("openid", param.OpenId);
            req.SetParameter("sign", req.CreateMd5Sign());

            var reqXml = req.ParseXml();
            var result = AjaxTools.PostXml(new HttpParam()
            {
                Url = "https://api.mch.weixin.qq.com/pay/unifiedorder",
                PostParam = reqXml,
                Encoding = Encoding.Default
            }) ;
            return result;
        }
        /// <summary>
        /// 根据当前系统时间加随机序列来生成订单号
        /// </summary>
        /// <param name="mchid">商户ID</param>
        /// <returns></returns>
        private string GenerateOutTradeNo(string mchid)
        {
            var ran = new Random();
            return string.Format("{0}{1}{2}", mchid, DateTime.Now.ToString("yyyyMMddHHmmss"), ran.Next(999));
        }
        /// <summary>
        /// 生成时间戳，自1970年以来的秒数     
        /// </summary>
        /// <returns></returns>
        public  string GetTimestamp()
        {
            var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString(CultureInfo.InvariantCulture);
        }
    }
}
