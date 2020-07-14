using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralTools.ToolsModel
{
    /// <summary>
    /// 微信支付完的返回值
    /// </summary>
    public class PayResult
    {
        /// <summary>
        /// 返回状态码
        /// </summary>
        public string ReturnCode { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string ReturnMsg { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        private bool isSuccess;

        /// <summary>
        /// 微信订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSuccess
        {
            get { return ReturnCode.ToUpper() == "SUCCESS" ? true : false; }
        }
    }
}
