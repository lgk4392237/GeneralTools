using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralTools.ToolsModel
{
    public class PayParam
    {
        /// <summary>
        /// 微信OpenId
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNumber { get; set; }
        /// <summary>
        /// 金额（分）
        /// </summary>
        public int? TotalFee { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 支付成功回调地址
        /// </summary>
        public string NotifyUrl { get; set; }

        /// <summary>
        /// 退款订单号
        /// </summary>
        public string RefundNumber { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        public int? RefundFee { get; set; }
        /// <summary>
        /// 自定义数据
        /// </summary>
        public string Attach { get; set; }

        /// <summary>
        /// 企业付款描述信息
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 支持IPV4和IPV6两种格式的IP地址。用户的客户端IP
        /// </summary>
        public string SpbillCreateIp { get; set; }

    }
}
