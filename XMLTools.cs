using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GeneralTools
{
    public class XMLTools
    {
        /// <summary>
        /// 读取XML文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static DataTable LoadXML(string path)
        {
            try
            {
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                //读取XML到DataTable
                ds.ReadXml(path);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
                return dt;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
