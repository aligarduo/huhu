using huhu.API.Filters;
using huhu.Commom;
using huhu.Commom.Encrypt;
using huhu.Commom.Enums;
using huhu.Commom.SMS;
using huhu.IBLL;
using huhu.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace huhu.API.Controllers
{
    /// <summary>
    /// 发送手机登录验证码
    /// </summary>
    public class SMSCodeController : ApiController
    {
        public IAreaCodeService AreaCodeBLL { get; set; }

        [HttpGet]
        [Route("passport/web/msg_code")]
        [SignSecurityFilter]
        public HttpResponseMessage MsgCode([FromUri] string account_sdk_source, [FromUri] string mobile) {
            ResultMsg result = new ResultMsg();
            string SMSFacility = ConfigurationManager.AppSettings["SMSUpFacility"].ToString();
            bool UpFacility = ((System.Collections.IList)SMSFacility.Split(',')).Contains(account_sdk_source);
            //检验参数
            if (string.IsNullOrEmpty(account_sdk_source) || string.IsNullOrEmpty(mobile) || !UpFacility)
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            //校验手机号码格式
            string area_code = Is_Phone(mobile);
            if (!area_code.Equals("false")) {
                mobile = mobile.Replace(area_code, "");
                return JsonUtil.ToJson(Create_Code("msg_code", area_code, mobile));
            } else
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PHONE_FORMAT_IS_WRONG, Descripion.GetDescription(ResultCode.PHONE_FORMAT_IS_WRONG)));
        }

        [HttpGet]
        [Route("passport/web/reset_code")]
        [SignSecurityFilter]
        public HttpResponseMessage ResetCode([FromUri] string account_sdk_source, [FromUri] string mobile) {
            ResultMsg result = new ResultMsg();
            string SMSFacility = ConfigurationManager.AppSettings["SMSUpFacility"].ToString();
            bool UpFacility = ((System.Collections.IList)SMSFacility.Split(',')).Contains(account_sdk_source);
            //检验参数
            if (string.IsNullOrEmpty(account_sdk_source) || string.IsNullOrEmpty(mobile) || !UpFacility)
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            //校验手机号码格式
            string area_code = Is_Phone(mobile);
            if (!area_code.Equals("false")) {
                mobile = mobile.Replace(area_code, "");
                return JsonUtil.ToJson(Create_Code("reset_code", area_code, mobile));
            } else
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PHONE_FORMAT_IS_WRONG, Descripion.GetDescription(ResultCode.PHONE_FORMAT_IS_WRONG)));
        }


        /// <summary>
        /// 校验手机号码
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns>bool</returns>
        private string Is_Phone(string mobile) {
            IEnumerable<object> area_code_all_list = AreaCodeBLL.Query_All();
            if (area_code_all_list != null) {
                foreach (var item in area_code_all_list) {
                    area_code_all reg = ObjectUtil.ConvertObject<area_code_all>(item);
                    if (Regex.IsMatch(mobile, reg.regulation))
                        return reg.area_code;
                }
            }
            return "false";
        }

        /// <summary>
        /// 创建验证码并保存至Redis和下发客户端
        /// </summary>
        /// <param name="area_code">国际区号</param>
        /// <param name="mobile">接收短信的手机号</param>
        /// <returns></returns>
        private object Create_Code(string template, string area_code, string mobile) {
            ResultMsg resultMsg = new ResultMsg();
            //随机验证码
            string random = NonceUtil.GenerateRandomCode(4);
            //验证码有效时间
            string expires = ConfigurationManager.AppSettings["SMSExpires"].ToString();

            string template_id = string.Empty, save_code_type = string.Empty;
            switch (template) {
                case "msg_code": template_id = "1142298"; save_code_type = "msgcode"; break;
                case "reset_code": template_id = "1279263"; save_code_type = "resetcode"; break;
                default: break;
            }

            //调用腾讯云短信验证码平台发送手机验证码
            string area_codes = area_code.Replace("+", "");
            object result = Core.Send_Msg(template_id, area_codes, mobile, new string[] { random, expires });

            JObject json = JObject.Parse(result.ToString());
            JValue resuno = (JValue)json["result"].Value<int>();
            JValue errmsg = (JValue)json["errmsg"].Value<string>();

            if (int.Parse(resuno.ToString()) == 0) {
                //保存至Redis
                Save_Code(save_code_type,area_code, mobile, random, expires);
                object S = new {
                    expires,
                    start_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    expire_time = DateTime.Now.AddMinutes(int.Parse(expires)).ToString("yyyy-MM-dd HH:mm:ss")
                };
                return resultMsg.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), S);
            }
            return resultMsg.SetResultMsg(int.Parse(resuno.ToString()), errmsg.ToString());
        }

        /// <summary>
        /// 验证码保存至Redis
        /// </summary>
        /// <param name="mobile">接收短信的手机号</param>
        /// <param name="random">随机验证码</param>
        /// <param name="expires">验证码有效时间</param>
        /// <returns></returns>
        private bool Save_Code(string save_code_type, string area_code, string mobile, string random, string expires) {
            mobile = save_code_type + ":" + area_code + mobile;
            random = MD5Util.GetMD5_8(random);
            RedisUtil redis = new RedisUtil();
            //存在则删除
            if (redis.StringGet(mobile) != null)
                redis.KeyDelete(mobile);
            //重新写入
            redis.StringSet(mobile, random);
            redis.SetExpire(mobile, DateTime.Now.AddMinutes(double.Parse(expires)));
            //执行完成
            return true;
        }


    }
}