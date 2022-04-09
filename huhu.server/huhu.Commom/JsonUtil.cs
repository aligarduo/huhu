using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Web.Script.Serialization;

namespace huhu.Commom
{
    public class JsonUtil
    {
        /// <summary>
        /// json序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static HttpResponseMessage ToJson(object obj)
        {
            string str;
            if (obj is string || obj is char)
            {
                str = obj.ToString();
            }
            else
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                str = serializer.Serialize(obj);
            }
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }

        /// <summary>
        /// json反序列化
        /// </summary>
        /// <param name="jsonobj"></param>
        /// <returns></returns>
        public static HttpResponseMessage RestoreJson(JObject jsonobj)
        {
            return JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(jsonobj));
        }

    }
}
