using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

namespace huhu.Commom.SensitiveCheck
{
    public class SensitiveMain
    {
        #region 配置
        /// <summary>
        /// 敏感词库路径
        /// </summary>
        private static readonly string SensitivePath = ConfigurationManager.AppSettings["SensitivePath"].ToString();
        #endregion

        /// <summary>
        /// 敏感词校验
        /// </summary>
        /// <param name="text">需校验的字符串</param>
        /// <returns>敏感词</returns>
        public static object SensitiveCheck(string text) {
            //只读取txt文件,得到的是一个包含所有txt文件路径的字符数组
            string[] strPath = Directory.GetFiles(SensitivePath, "*.txt");
            //敏感词组
            List<string> sensitive_list = new List<string>();
            foreach (var i in strPath) {
                StreamReader sr = new StreamReader(i, Encoding.UTF8);
                string line;
                //按行读取 line为每行的数据
                while ((line = sr.ReadLine()) != null) {
                    sensitive_list.Add(line);
                }
            }
            //敏感词库类可被继承，如果想实现自定义 敏感词导入方法可以对LoadWords方法进行重写
            WordsLibrary library = new WordsLibrary(sensitive_list.ToArray()); //实例化 敏感词库

            ContentCheck check = new ContentCheck(library, text);  //实例化 内容检测类
            List<string> list = check.FindSensitiveWords();    //调用 查找敏感词方法 返回敏感词列表
            //string str = check.SensitiveWordsReplace();  //调用 敏感词替换方法 返回处理过的字符串

            if (list == null) {
                return null;
            } else {
                return list;
            }
        }


    }
}