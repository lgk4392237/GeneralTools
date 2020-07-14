using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralTools.ToolsModel
{
    public class PayConfig
    {
        /// <summary>
        /// 服务号appid
        /// </summary>
        public string AppID { get; set; }
        /// <summary>
        /// 服务号appsecret
        /// </summary>
        public string AppSecret { get; set; }
        /// <summary>
        /// 商户mchid
        /// </summary>
        public string MchID { get; set; }
        /// <summary>
        /// 商户key
        /// </summary>
        public string MchKey { get; set; }
    }
}
