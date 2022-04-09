using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace huhu.Commom.SMS
{
    public static class Core
    {
        /// <summary>
        /// 获取时间戳格式
        /// </summary>
        /// <param name="flg">多少位的时间戳</param>
        /// <returns>时间戳</returns>
        public static long GetTimeStamp(int flg)
        {
            long time = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            switch (flg) {
                case 10:
                    time = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
                    break;
                case 13:
                    DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
                    time = (DateTime.Now.Ticks - startTime.Ticks) / 10000;
                    break;
            }
            return time;
        }

        /// <summary>
        /// Sha256加密算法
        /// </summary>
        /// <param name="data">加密的内容</param>
        /// <returns>加密后的数据</returns>
        public static string Sha256(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < SHA256.Create().ComputeHash(bytes).Length; i++) {
                builder.Append(SHA256.Create().ComputeHash(bytes)[i].ToString("X2"));
            }
            return builder.ToString();
        }

        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="postdata">参数</param>
        /// <returns>返回内容</returns>
        public static string HttpPost(string url, string postdata)
        {
            string result = string.Empty;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.Referer = null;
            req.AllowAutoRedirect = true;
            req.Accept = "*/*";

            byte[] data = Encoding.UTF8.GetBytes(postdata);
            using (Stream reqStream = req.GetRequestStream()) {
                reqStream.Write(data, 0, data.Length);
            }
            try {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                using (StreamReader reader = new StreamReader(resp.GetResponseStream())) {
                    result = reader.ReadToEnd();
                }
            }
            catch (Exception ex) {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <param name="template_id">短信模版id</param>
        /// <param name="area_code">国家标识</param>
        /// <param name="mobiles">验证接收短信的手机号</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public static object Send_Msg(string template_id, string area_code, string mobiles, string[] param)
        {
            string mobile = mobiles;
            string appkey = "4768f010b131c71d068e6614b7b4a9bc";//AppKey
            string random = NonceUtil.GenerateRandomCode(4);
            string time = GetTimeStamp(10).ToString();
            string sig = Sha256($"appkey={appkey}&random={random}&time={time}&mobile={mobile}");
            Entity postData = new Entity {
                Ext = "",
                Extend = "",
                Params = param,
                Sig = sig,
                Sign = "乎乎huhu",//短信签名
                Tel = new Phone {
                    Mobile = mobiles,
                    Nationcode = area_code
                },
                Time = time,
                Tpl_id = template_id
            };
            string url = $"https://yun.tim.qq.com/v5/tlssmssvr/sendsms?sdkappid=1400579497&random={random}";//sdkappid = SDKAppID
            string postDataStr = JsonConvert.SerializeObject(postData).ToLower();
            return HttpPost(url, postDataStr);
        }

    }
}
