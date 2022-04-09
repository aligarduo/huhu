using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;

namespace huhu.Commom.Token
{
    public class token
    {
        /// <summary>
        /// 解析token并获取携带参数
        /// </summary>
        /// <returns></returns>
        public static model get_and_parse(HttpActionContext ActionContext)
        {
            HttpRequestMessage request = ActionContext.Request;
            string passport_csrf_token = request.Headers.GetValues("passport_csrf_token").FirstOrDefault();
            string token = HttpUtility.UrlDecode(passport_csrf_token);
            return JwtUtil.GetJwtDecode<model>(token);
        }

        /// <summary>
        /// token状态
        /// </summary>
        /// <returns></returns>
        public static model is_exist_token(HttpActionContext ActionContext)
        {
            HttpRequestMessage request = ActionContext.Request;
            model token_info = new model();
            if (request.Headers.Contains("passport_csrf_token")) {
                var csrf_token = request.Headers.GetValues("passport_csrf_token").FirstOrDefault();
                if (!string.IsNullOrEmpty(csrf_token)) {
                    string token = HttpUtility.UrlDecode(csrf_token);
                    token_info = JwtUtil.GetJwtDecode<model>(token);
                    return token_info;
                }
            }
            token_info.setParams = "0";
            return token_info;
        }

    }
}
