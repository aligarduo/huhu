using huhu.API.Filters;
using huhu.Commom;
using huhu.Commom.Enums;
using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace huhu.API.Controllers
{
    /// <summary>
    /// 发送邮箱验证码
    /// </summary>
    public class EmailCodeController : ApiController
    {
        [HttpGet]
        [Route("passport/web/send_email_code")]
        [SignSecurityFilter]
        public HttpResponseMessage Send_Email_Code([FromUri] string account_sdk_source, [FromUri] string email)
        {
            ResultMsg result = new ResultMsg();
            //检验参数
            if (string.IsNullOrEmpty(account_sdk_source) || account_sdk_source != "web")
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            if (string.IsNullOrEmpty(email))
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            //校验格式
            if (RegularUtil.IsEmail(email))
                return JsonUtil.ToJson(Create_Code(email));
            else
                return JsonUtil.ToJson(new ResultMsg().SetResultMsg((int)ResultCode.EMAIL_FORMAT_IS_WRONG, Descripion.GetDescription(ResultCode.EMAIL_FORMAT_IS_WRONG)));
        }

        /// <summary>
        /// 创建验证码
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private object Create_Code(string email)
        {
            ResultMsg result = new ResultMsg();
            //验证码有效时间
            string expires = ConfigurationManager.AppSettings["EmailExpires"].ToString();
            string random = NonceUtil.GenerateRandomCode(4);
            //发送邮件
            if (Send_Email(email, random, int.Parse(expires))) {
                //保存至Redis
                Save_Code(email.Substring(0, email.IndexOf('@')), random);
                object S = new {
                    expires,
                    start_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    expire_time = DateTime.Now.AddMinutes(int.Parse(expires)).ToString("yyyy-MM-dd HH:mm:ss")
                };
                return result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), S);
            }
            return result.SetResultMsg((int)ResultCode.ISSUED_ERROR, Descripion.GetDescription(ResultCode.ISSUED_ERROR));
        }

        /// <summary>
        /// 验证码保存至Redis
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private bool Save_Code(string email, string code)
        {
            RedisUtil redis = new RedisUtil();
            if (redis.StringGet(email) != null)
                redis.KeyDelete(email);

            redis.StringSet(email, code);
            redis.SetExpire(email, DateTime.Now.AddSeconds(60));

            return true;
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="email"></param>
        /// <param name="random"></param>
        /// <returns></returns>
        private bool Send_Email(string email, string random, int expires)
        {
            string body = SendEmail.Template("email/code.html", "设备绑定", random, expires);
            Task.Factory.StartNew(() => { SendEmail.SendMails(email, "乎乎-验证邮件", body); });
            return true;
        }

    }
}