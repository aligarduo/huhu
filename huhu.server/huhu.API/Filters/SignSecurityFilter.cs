using huhu.Commom;
using huhu.Commom.Enums;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace huhu.API.Filters
{
    public class SignSecurityFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext ActionContext)
        {
            ResultMsg resultMsg = new ResultMsg();
            HttpRequestMessage request = ActionContext.Request;
            string appid = string.Empty, timestamp = string.Empty, nonce = string.Empty, sign = string.Empty;

            if (request.Headers.Contains("appid"))
                appid = HttpUtility.UrlDecode(request.Headers.GetValues("appid").FirstOrDefault());
            if (request.Headers.Contains("timestamp"))
                timestamp = HttpUtility.UrlDecode(request.Headers.GetValues("timestamp").FirstOrDefault());
            if (request.Headers.Contains("nonce"))
                nonce = HttpUtility.UrlDecode(request.Headers.GetValues("nonce").FirstOrDefault());
            if (request.Headers.Contains("sign"))
                sign = HttpUtility.UrlDecode(request.Headers.GetValues("sign").FirstOrDefault());

            // 请求头是否包含以下参数
            if (string.IsNullOrEmpty(appid) || string.IsNullOrEmpty(timestamp) || string.IsNullOrEmpty(nonce) || string.IsNullOrEmpty(sign))
            {
                ActionContext.Response = JsonUtil.ToJson(resultMsg.SetResultMsg((int)ResultCode.SIGN_LACK_FIELD, Descripion.GetDescription(ResultCode.SIGN_LACK_FIELD)));
                base.OnActionExecuting(ActionContext);
                return;
            }

            //判断时间戳有效性 允许时差（10位时单位为/秒，13位时单位为/毫秒）
            bool is_Timestamp = TimeUtil.ValidateTimestamp(double.Parse(timestamp), 600000);
            // timespan是否有效
            if (!is_Timestamp)
            {
                ActionContext.Response = JsonUtil.ToJson(resultMsg.SetResultMsg((int)ResultCode.SIGN_REQUEST_TIMEOUT, Descripion.GetDescription(ResultCode.SIGN_REQUEST_TIMEOUT)));
                base.OnActionExecuting(ActionContext);
                return;
            }

            //根据请求类型拼接参数
            NameValueCollection form = HttpContext.Current.Request.QueryString;
            string data = string.Empty;
            string method = request.Method.Method;
            switch (method)
            {
                case "POST":
                    Stream stream = HttpContext.Current.Request.InputStream;
                    string responseJson = string.Empty;
                    StreamReader streamReader = new StreamReader(stream);
                    data = streamReader.ReadToEnd();
                    break;
                case "GET":
                    //第一步：取出所有get参数
                    IDictionary<string, string> parameters = new Dictionary<string, string>();
                    for (int f = 0; f < form.Count; f++)
                    {
                        string key = form.Keys[f];
                        parameters.Add(key, form[key]);
                    }
                    //第二步：把字典按Key的字母顺序排序
                    IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters);
                    IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();
                    //第三步：把所有参数名和参数值串在一起
                    StringBuilder query = new StringBuilder();
                    while (dem.MoveNext())
                    {
                        string key = dem.Current.Key;
                        string value = dem.Current.Value;
                        if (!string.IsNullOrEmpty(key))
                        {
                            query.Append(key).Append(value);
                        }
                    }
                    data = query.ToString();
                    break;
                default:
                    ActionContext.Response = JsonUtil.ToJson(resultMsg.SetResultMsg((int)ResultCode.REQUEST_IS_NOT_SUPPORTED, Descripion.GetDescription(ResultCode.REQUEST_IS_NOT_SUPPORTED)));
                    base.OnActionExecuting(ActionContext);
                    return;
            }

            var hash = System.Security.Cryptography.MD5.Create();
            //拼接签名数据
            string signStr = appid + timestamp + nonce + data;
            //字符串转字符
            char[] singChar = signStr.ToCharArray();
            //字符转数组
            List<string> signArr = new List<string>();
            foreach (var c in singChar)
            {
                signArr.Add(c.ToString());
            }
            string[] str = signArr.ToArray();
            //将数组中字符按ASCII码升序排序 *核心*
            Array.Sort(str, string.CompareOrdinal);
            //串联数组
            var sortStr = string.Concat(str);
            var bytes = Encoding.UTF8.GetBytes(sortStr);
            //使用32位大写MD5签名
            var md5Val = hash.ComputeHash(bytes);
            var result = new StringBuilder();
            foreach (var c in md5Val)
            {
                result.Append(c.ToString("X2"));
            }
            //与前端传过来的签名参数进行比对
            if (sign == result.ToString().ToLower())
                base.OnActionExecuting(ActionContext);
            else
            {
                ActionContext.Response = JsonUtil.ToJson(resultMsg.SetResultMsg((int)ResultCode.SIGN_CHECK_FAILURE, Descripion.GetDescription(ResultCode.SIGN_CHECK_FAILURE)));
                base.OnActionExecuting(ActionContext);
                return;
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}