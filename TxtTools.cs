using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GeneralTools
{
    public class TxtTools
    {
        /// <summary>
        /// 写入txt
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="text"></param>
        public  void UpdateTxtFile(string filePath, string text)
        {
            if (!File.Exists(filePath))
            {
                FileStream fs = File.Create(filePath);
                fs.Close();
            }
            StreamWriter sr = new StreamWriter(filePath, false, System.Text.Encoding.Default);
            sr.Write(text);
            sr.Close();
        }
        /// <summary>
        /// 读取txt
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public  string GetTxtFile(string filePath)
        {
            string innerText = string.Empty;
            if (File.Exists(filePath))
            {
                StreamReader sr = new StreamReader(filePath, System.Text.Encoding.Default);
                innerText = sr.ReadToEnd();
                sr.Close();
                innerText = innerText.Replace("\r\n", "").Replace("\n", "");

            }
            return innerText;
        }
    }
}
