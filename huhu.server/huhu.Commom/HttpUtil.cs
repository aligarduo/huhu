using System;
using System.IO;
using System.Net;
using System.Text;

namespace huhu.Commom
{
    public class HttpUtil
    {
        /// <summary>
        /// POST、PUT、DELETE 请求
        /// </summary>
        /// <param name="serviceUrl">http地址</param>
        /// <param name="data">json数据</param>
        /// <param name="method">请求类型 ：Get、POST、PUT、DELETE ，如果是Get请求只传 serviceUrl 就可以</param>
        /// <returns></returns>
        public static string Send(string serviceUrl, string data = null, string method = null) {
            if (serviceUrl is null) {
                throw new ArgumentNullException(nameof(serviceUrl));
            }
            //创建Web访问对象
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(serviceUrl);
            if (data != null) {
                //把用户传过来的数据转成“UTF-8”的字节流
                byte[] buf = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(data);
                myRequest.Method = method;
                myRequest.ContentLength = buf.Length;
                myRequest.ContentType = "application/json";
                //myRequest.ContentType = "text/plain";
                myRequest.MaximumAutomaticRedirections = 1;
                myRequest.AllowAutoRedirect = true;
                //发送请求
                Stream stream = myRequest.GetRequestStream();
                stream.Write(buf, 0, buf.Length);
                stream.Close();
            }
            //获取接口返回值
            //通过Web访问对象获取响应内容
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            //通过响应内容流创建StreamReader对象，因为StreamReader更高级更快
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            //string returnXml = HttpUtility.UrlDecode(reader.ReadToEnd());//如果有编码问题就用这个方法
            string returnXml = reader.ReadToEnd();//利用StreamReader就可以从响应内容从头读到尾
            reader.Close();
            myResponse.Close();
            return returnXml;
        }

    }
}
