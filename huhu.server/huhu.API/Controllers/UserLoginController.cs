using huhu.API.Filters;
using huhu.Commom;
using huhu.Commom.Encrypt;
using huhu.Commom.Enums;
using huhu.Commom.Snowflake;
using huhu.Commom.Token;
using huhu.IBLL;
using huhu.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using WebApiThrottle;

namespace huhu.API.Controllers
{
    /// <summary>
    /// 用户登录
    /// </summary>
    public class UserLoginController : ApiController
    {
        #region 配置
        /// <summary>
        /// 磁盘存储文件路径
        /// </summary>
        private static readonly string DiskStoragePath = ConfigurationManager.AppSettings["DiskStoragePath"].ToString();
        /// <summary>
        /// 用户默认头像存放文件夹
        /// </summary>
        private static readonly string UserDefaultAvatar = ConfigurationManager.AppSettings["UserDefaultAvatar"].ToString();
        #endregion

        public IAreaCodeService AreaCodeBLL { get; set; }
        public IUserService UserBLL { get; set; }
        public IUserLocateService UserLocateBLL { get; set; }


        [HttpPost]
        [SignSecurityFilter]
        [EnableThrottling(PerSecond = 2, PerMinute = 100)]
        [Route("passport/web/sms_login")]
        public HttpResponseMessage SMS_Login([FromBody] JObject obj) {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(obj));
            if (obj_list == null || obj_list.Property("mobile") == null || obj_list.Property("code") == null)
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            else
                return JsonUtil.ToJson(SMS(obj_list));
        }


        [HttpPost]
        [SignSecurityFilter]
        [EnableThrottling(PerSecond = 2, PerMinute = 100)]
        [Route("passport/web/cipher_login")]
        public HttpResponseMessage Cipher_Login([FromBody] JObject obj) {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(obj));
            if (obj_list == null || obj_list.Property("account") == null || obj_list.Property("password") == null)
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            else
                return JsonUtil.ToJson(Cipher(obj_list));
        }


        [HttpPost]
        [SignSecurityFilter]
        [EnableThrottling(PerSecond = 2, PerMinute = 100)]
        [Route("passport/web/reset_password")]
        public HttpResponseMessage Reset_Password([FromBody] JObject obj) {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(obj));
            //校验参数
            string[] ef_item = new string[] { "mobile", "code", "password" };
            foreach (var item in ef_item) {
                if (obj_list.Property(item) == null || obj_list[item].Value<string>() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }
            return JsonUtil.ToJson(Reset(obj_list));
        }


        [HttpGet]
        [SignSecurityFilter]
        [TokenSecurityFilter]
        [Route("passport/web/logout")]
        public HttpResponseMessage Logout() {
            string _token = token.get_and_parse(ActionContext).setId;
            return JsonUtil.ToJson(Logout(_token));
        }


        /// <summary>
        /// 手机登录
        /// </summary>
        /// <param name="obj_list">前端数据集</param>
        /// <returns>执行状态信息</returns>
        private object SMS(JObject obj_list) {
            ResultMsg result = new ResultMsg();
            string mobile = obj_list["mobile"].Value<string>();
            string code = obj_list["code"].Value<string>();
            string setParams = string.Empty;
            //校验手机号
            string area_code = Is_Phone(mobile);
            if (area_code == "false")
                return result.SetResultMsg((int)ResultCode.PHONE_FORMAT_IS_WRONG, Descripion.GetDescription(ResultCode.PHONE_FORMAT_IS_WRONG));
            //校验验证码
            string mobiles = "msgcode:" + mobile;
            RedisUtil redis = new RedisUtil();
            if (redis.KeyExists(mobiles) && redis.StringGet(mobiles) == code) {
                //随机头像
                string[] defaultAvatar = Directory.GetFiles(DiskStoragePath + "\\" + UserDefaultAvatar, "*.png");
                string avatar_large = UserDefaultAvatar + "/" + Path.GetFileName(defaultAvatar[new Random().Next(0, defaultAvatar.Length)]);

                lock (this) {
                    user_all u = new user_all();
                    mobile = mobile.Replace(area_code, "");
                    u.phone = mobile;
                    user_all user_info = UserBLL.Query_Phone(u).FirstOrDefault();
                    //校验手机号是否已被注册
                    if (user_info == null) {
                        u.user_id = Commom.GradualID.UseridUtil.IDByGUId();
                        u.user_name = mobile.Remove(0, 4);
                        u.avatar_large = avatar_large;
                        u.password = "";
                        u.phone_verified = 1;
                        u.email = "";
                        u.email_verified = 0;
                        u.qq_openid = "";
                        u.qq_nickname = "";
                        u.qq_figureurl = "";
                        u.qq_verified = 0;
                        u.wechat_nickname = "";
                        u.wechat_verified = 0;
                        u.weibo_nickname = "";
                        u.weibo_verified = 0;
                        u.level = 1;
                        u.job_title = "";
                        u.ip_address = NetUtil.GET_ClientIP();
                        u.place = NetUtil.GET_Client_Place(u.ip_address);
                        u.status = 0;
                        u.update_time = TimeUtil.GetCurrentTimestamp().ToString();
                        u.register_time = TimeUtil.GetCurrentTimestamp().ToString();
                        UserBLL.Add(u);
                        UserBLL.SaveChanges();
                        Is_UserStatus(u);
                        setParams = u.user_id;
                    } else {
                        Is_UserStatus(user_info);
                        setParams = user_info.user_id;
                    }
                }
                if (redis.StringGet("msgcode:" + obj_list["mobile"].Value<string>()) != null)
                    redis.KeyDelete("msgcode:" + obj_list["mobile"].Value<string>());
                //登录成功
                return result.SetResultToken((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), Token(setParams));
            }
            //验证码输入错误
            else
                return result.SetResultMsg((int)ResultCode.VCODE_WRONG, Descripion.GetDescription(ResultCode.VCODE_WRONG));
        }

        /// <summary>
        /// 密码登录
        /// </summary>
        /// <param name="obj_list">前端数据集</param>
        /// <returns>执行状态信息</returns>
        private object Cipher(JObject obj_list) {
            ResultMsg result = new ResultMsg();
            user_all u = new user_all();
            string account = obj_list["account"].Value<string>();
            string password = obj_list["password"].Value<string>();
            if (Is_Phone(account) != "false") {
                u.phone = account;
                u.password = MD5Util.GetMD5_16(password);
                if (UserBLL.Query_Phone_Password(u).Count > 0) {
                    u.phone = account;
                    user_all user = UserBLL.Query_Phone(u).FirstOrDefault();
                    return Is_UserStatus(user);
                } else
                    return result.SetResultMsg((int)ResultCode.ACCOUNT_OR_PASS_INCORRECT, Descripion.GetDescription(ResultCode.ACCOUNT_OR_PASS_INCORRECT));
            } else if (RegularUtil.IsEmail(account)) {
                u.email = account;
                u.password = MD5Util.GetMD5_16(password);
                if (UserBLL.Query_Email_Password(u).Count > 0) {
                    u.email = account;
                    user_all user = UserBLL.Query_Email(u).FirstOrDefault();
                    return Is_UserStatus(user);
                } else
                    return result.SetResultMsg((int)ResultCode.ACCOUNT_OR_PASS_INCORRECT, Descripion.GetDescription(ResultCode.ACCOUNT_OR_PASS_INCORRECT));
            }
            //操作类型出错
            return result.SetResultMsg((int)ResultCode.ACCOUNT_OR_PASS_INCORRECT, Descripion.GetDescription(ResultCode.ACCOUNT_OR_PASS_INCORRECT));
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="obj_list"></param>
        /// <returns></returns>
        private object Reset(JObject obj_list) {
            ResultMsg result = new ResultMsg();
            string mobile = obj_list["mobile"].Value<string>();
            string code = obj_list["code"].Value<string>();
            string password = obj_list["password"].Value<string>();
            //校验手机号
            string area_code = Is_Phone(mobile);
            if (area_code == "false")
                return result.SetResultMsg((int)ResultCode.PHONE_FORMAT_IS_WRONG, Descripion.GetDescription(ResultCode.PHONE_FORMAT_IS_WRONG));
            //校验验证码
            string mobiles = "resetcode:" + mobile;
            RedisUtil redis = new RedisUtil();
            if (redis.KeyExists(mobiles) && redis.StringGet(mobiles) == code) {
                user_all _user = new user_all {
                    phone = mobile.Replace(area_code, ""),
                };
                user_all user_info = UserBLL.Query_Phone(_user).FirstOrDefault();
                if (user_info == null) {
                    return result.SetResultMsg((int)ResultCode.USER_NOT_EXIST, Descripion.GetDescription(ResultCode.USER_NOT_EXIST));
                }
                user_all user = new user_all {
                    user_id = user_info.user_id,
                    phone = mobile.Replace(area_code, ""),
                    password = MD5Util.GetMD5_16(password),
                    user_name = "",
                    avatar_large = "",
                    phone_verified = 1,
                    email = "",
                    email_verified = 0,
                    qq_openid = "",
                    qq_nickname = "",
                    qq_verified = 0,
                    wechat_nickname = "",
                    wechat_verified = 0,
                    weibo_nickname = "",
                    weibo_verified = 0,
                    level = 1,
                    job_title = "",
                    ip_address = "",
                    place = "",
                    status = 0,
                    update_time = "",
                    register_time = "",
                };
                UserBLL.Update_Condition(user, new string[] { "password" });
                UserBLL.SaveChanges();
                if (redis.StringGet("resetcode:" + mobile) != null)
                    redis.KeyDelete("resetcode:" + mobile);
                //重置密码成功
                return result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS));
            }
            //验证码错误
            else
                return result.SetResultMsg((int)ResultCode.VCODE_WRONG, Descripion.GetDescription(ResultCode.VCODE_WRONG));
        }

        /// <summary>
        /// 校验手机号码
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns>校验成功则返回区号，否则返回"false"</returns>
        private string Is_Phone(string mobile) {
            IEnumerable<object> area_code_all_list = AreaCodeBLL.Query_All();
            if (area_code_all_list != null) {
                foreach (var item in area_code_all_list) {
                    var obj = ObjectUtil.ConvertObject<area_code_all>(item);
                    if (Regex.IsMatch(mobile, obj.regulation)) {
                        return obj.area_code;
                    }
                }
            }
            return "false";
        }

        /// <summary>
        /// 校验账号状态
        /// </summary>
        /// <param name="user_info"></param>
        /// <returns></returns>
        private object Is_UserStatus(user_all user_info) {
            ResultMsg result = new ResultMsg();
            switch (user_info.status) {
                case 0: {
                        RecordingUnit(user_info.user_id);
                        return result.SetResultToken((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), Token(user_info.user_id));
                    };
                case 1: return result.SetResultMsg((int)ResultCode.ACCOUNT_ABNORMAL_LIMIT, Descripion.GetDescription(ResultCode.ACCOUNT_ABNORMAL_LIMIT));
                case 2: return result.SetResultMsg((int)ResultCode.ACCOUNT_VIOLATION_LIMIT, Descripion.GetDescription(ResultCode.ACCOUNT_VIOLATION_LIMIT));
                case 3: return result.SetResultMsg((int)ResultCode.ACCOUNT_VIOLATION_CLOSED, Descripion.GetDescription(ResultCode.ACCOUNT_VIOLATION_CLOSED));
                case 4: return result.SetResultMsg((int)ResultCode.ACCOUNT_IS_CANCELLED, Descripion.GetDescription(ResultCode.ACCOUNT_IS_CANCELLED));
                default: throw new Exception($"{user_info.user_id}的audit_status出现致命错误");
            }
        }

        /// <summary>
        /// 记录登录设备
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        private bool RecordingUnit(string user_id) {
            Snowflake.Instance.SnowflakesInit(0, 0);
            user_locate locate = new user_locate();
            locate.id = Snowflake.Instance.NextId().ToString();
            locate.user_id = user_id;
            locate.ip = NetUtil.GET_ClientIP();
            locate.place = NetUtil.GET_Client_Place(locate.ip);
            locate.mac = NetUtil.GET_ClientMac(locate.ip);
            locate.time = TimeUtil.GetCurrentTimestamp().ToString();
            UserLocateBLL.Add(locate);
            UserLocateBLL.SaveChanges();
            return true;
        }

        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        private string Token(string user_id) {
            string TokenExpirationTime = ConfigurationManager.AppSettings["TokenExpirationTime"].ToString();
            string Guid = GUIDUtil.GuidTo16();
            Guid = "token:" + Guid;
            model token = new model {
                setId = Guid,
                setParams = user_id,
                setIssuer = "huhu",
                setIssuedAt = TimeUtil.GetCurrentTimestamp(false),
                setExpiration = TimeUtil.ToTimestamp(DateTime.Now.AddDays(double.Parse(TokenExpirationTime)), false)
            };
            string Token = JwtUtil.SetJwtEncode(token);
            RedisUtil redis = new RedisUtil();
            if (redis.StringGet(Guid) != null) {
                redis.KeyDelete(Guid);
            }
            redis.StringSet(Guid, Token);
            redis.SetExpire(Guid, DateTime.Now.AddDays(double.Parse(TokenExpirationTime)));
            return Token;
        }

        /// <summary>
        /// 账号注销登录
        /// </summary>
        /// <param name="token_setId"></param>
        /// <returns></returns>
        private object Logout(string token_setId) {
            ResultMsg result = new ResultMsg();
            RedisUtil redis = new RedisUtil();
            if (redis.StringGet(token_setId) != null) {
                redis.KeyDelete(token_setId);
                return result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS));
            }
            return result.SetResultMsg((int)ResultCode.PLEASE_LOGIN_FIRST, Descripion.GetDescription(ResultCode.PLEASE_LOGIN_FIRST));
        }

    }
}