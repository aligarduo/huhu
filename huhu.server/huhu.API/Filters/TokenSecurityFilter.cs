using huhu.Commom;
using huhu.Commom.Enums;
using huhu.Commom.Token;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace huhu.API.Filters
{
    public class TokenSecurityFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext ActionContext)
        {
            ResultMsg result = new ResultMsg();
            HttpRequestMessage request = ActionContext.Request;
            if (request.Headers.Contains("passport_csrf_token")) {
                string token = HttpUtility.UrlDecode(request.Headers.GetValues("passport_csrf_token").FirstOrDefault());
                model token_info = JwtUtil.GetJwtDecode<model>(token);

                //Token是否包含以下参数
                if (string.IsNullOrEmpty(token_info.setId) ||
                    string.IsNullOrEmpty(token_info.setParams) ||
                    string.IsNullOrEmpty(token_info.setIssuer) ||
                    string.IsNullOrEmpty(token_info.setIssuedAt) ||
                    string.IsNullOrEmpty(token_info.setExpiration)) {
                    ActionContext.Response = JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.CSRF_TOKEN_CHECK_FAILURE, Descripion.GetDescription(ResultCode.CSRF_TOKEN_CHECK_FAILURE)));
                    base.OnActionExecuting(ActionContext);
                    return;
                }
                //存在token
                if (new RedisUtil().StringGet(token_info.setId) == null) {
                    ActionContext.Response = JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.CSRF_TOKEN_CHECK_FAILURE, Descripion.GetDescription(ResultCode.CSRF_TOKEN_CHECK_FAILURE)));
                    base.OnActionExecuting(ActionContext);
                    return;
                }
                //携带参数为空
                if (token_info.setParams == null) {
                    ActionContext.Response = JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.CSRF_TOKEN_CHECK_FAILURE, Descripion.GetDescription(ResultCode.CSRF_TOKEN_CHECK_FAILURE)));
                    base.OnActionExecuting(ActionContext);
                    return;
                }
                //签发者
                if (token_info.setIssuer != "huhu") {
                    ActionContext.Response = JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.CSRF_TOKEN_CHECK_FAILURE, Descripion.GetDescription(ResultCode.CSRF_TOKEN_CHECK_FAILURE)));
                    base.OnActionExecuting(ActionContext);
                    return;
                }
                //过期Token
                string TokenExpirationTime = ConfigurationManager.AppSettings["TokenExpirationTime"].ToString();
                TokenExpirationTime = TimeUtil.ToTimestamp(System.DateTime.Now.AddDays(double.Parse(TokenExpirationTime)), false);
                if (!TimeUtil.ValidateTimestamp(double.Parse(token_info.setExpiration), int.Parse(TokenExpirationTime))) {
                    ActionContext.Response = JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.CSRF_TOKEN_CHECK_FAILURE, Descripion.GetDescription(ResultCode.CSRF_TOKEN_CHECK_FAILURE)));
                    base.OnActionExecuting(ActionContext);
                    return;
                }

                base.OnActionExecuting(ActionContext);
            }
            else {
                ActionContext.Response = JsonUtil.ToJson(new ResultMsg().SetResultMsg((int)ResultCode.CSRF_REQUEST_NOT_AUTHORIZED, Descripion.GetDescription(ResultCode.CSRF_REQUEST_NOT_AUTHORIZED)));
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