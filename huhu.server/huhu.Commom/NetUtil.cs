using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;

namespace huhu.Commom
{
    public class NetUtil
    {
        #region 获取客户端IP
        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <returns>客户端IP</returns>
        public static string GET_ClientIP()
        {
            string ip = string.Empty;
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_VIA"]))
                ip = Convert.ToString(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
            if (string.IsNullOrEmpty(ip))
                ip = Convert.ToString(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
            return ip;
        }

        #endregion

        #region 获取客户端所在地区
        /// <summary>
        /// 获取客户端所在地区
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string GET_Client_Place(string ip)
        {
            string result = string.Empty;
            string url = "https://apis.map.qq.com/ws/location/v1/ip?ip=" + ip + "&key=OSJBZ-AMR3W-MLIRO-RGJK4-XQSBJ-QSFXD";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            try {
                HttpWebResponse resp = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(resp.GetResponseStream())) {
                    result = reader.ReadToEnd();
                }

                JObject json = JObject.Parse(result.ToString());
                if (json["status"].Value<int>() == 0) {
                    object results = json["result"].Value<object>();
                    //IP定位结果
                    JObject json2 = JObject.Parse(results.ToString());
                    object ad_info = json2["ad_info"].Value<object>();
                    //定位行政区划信息
                    JObject json3 = JObject.Parse(ad_info.ToString());
                    string city = json3["city"].Value<string>();
                    string district = json3["district"].Value<string>();
                    result = city + district;
                }
                else {
                    result = json["message"].Value<string>();
                }
            }
            catch (Exception ex) {
                throw ex;
            }
            return result;
        }

        #endregion

        #region 获取客户端MAC地址
        [DllImport("Iphlpapi.dll")]
        private static extern int SendARP(Int32 DestIP, Int32 SrcIP, ref Int64 MacAddr, ref Int32 PhyAddrLen);
        [DllImport("Ws2_32.dll")]
        private static extern Int32 inet_addr(string ipaddr);
        ///<summary>
        /// 获取客户端MAC地址
        ///</summary>
        ///<param name="ip">目标机器IP地址</param>
        ///<returns>目标机器mac地址</returns>
        public static string GET_ClientMac(string ip)
        {
            StringBuilder macAddress = new StringBuilder();
            try {
                Int32 remote = inet_addr(ip);
                Int64 macInfo = new Int64();
                Int32 length = 6;
                SendARP(remote, 0, ref macInfo, ref length);
                string temp = Convert.ToString(macInfo, 16).PadLeft(12, '0').ToUpper();
                int x = 12;
                for (int i = 0; i < 6; i++) {
                    if (i == 5) {
                        macAddress.Append(temp.Substring(x - 2, 2));
                    }
                    else {
                        macAddress.Append(temp.Substring(x - 2, 2) + "-");
                    }
                    x -= 2;
                }
                return macAddress.ToString();
            }
            catch {
                return macAddress.ToString();
            }
        }

        #endregion
    }
}