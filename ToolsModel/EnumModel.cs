using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralTools.ToolsModel
{
    public class EnumModel
    {
        public enum HttpParamType
        {
            /// <summary>
            /// json数据。默认值。
            /// </summary>
            Json,
            /// <summary>
            /// 形如：key=value＆key=value＆key=value
            /// </summary>
            Form
        }
    }
}
