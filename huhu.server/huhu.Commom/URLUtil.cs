using System.Net;
using System.Text.RegularExpressions;

namespace huhu.Commom
{
    public class URLUtil
    {
        /// <summary>
        /// 检测URL是否可以访问
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>true/false</returns>
        public static bool CheckURL(string url)
        {
            bool result = false;
            try
            {
                if (url.IndexOf("http").Equals(-1))
                {
                    url = "http://" + url;
                }
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                myHttpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.63 Safari/537.36";
                myHttpWebRequest.Method = "GET";
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                if (myHttpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    result = true;
                }
                myHttpWebResponse.Close();
            }
            catch
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 正则截取URL
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="way">域名:domainName，目录：directory，页面 page，参数：parameter</param>
        /// <returns></returns>
        public static string CaptureURL(string url, string way)
        {
            string cache = string.Empty;
            try
            {
                Regex reg = new Regex(@"(?imn)(?<do>(http|https)://[^/]+/)(?<dir>([^/]+/)*([^/.]*$)?)((?<page>[^?.]+\.[^?]+)\?)?(?<par>.*$)");
                MatchCollection mc = reg.Matches(url);
                foreach (Match m in mc)
                {
                    switch (way)
                    {
                        case "domainName": cache = m.Groups["do"].Value; break;
                        case "directory": cache = m.Groups["dir"].Value; break;
                        case "page": cache = m.Groups["page"].Value; break;
                        case "parameter": cache = m.Groups["par"].Value; break;
                        default: break;
                    }
                }
                return cache;
            }
            catch
            {
                return cache;
            }
        }

        /// <summary>
        /// 解析URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Parsing_URL(string url)
        {
            string cache = string.Empty;
            if (CheckURL(url))
            {
                cache += CaptureURL(url, "directory");
                cache += CaptureURL(url, "parameter");
            }
            return cache;
        }


    }
}
