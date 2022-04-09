using huhu.Commom;
using huhu.Commom.Enums;
using huhu.Commom.Snowflake;
using huhu.Commom.Token;
using huhu.IBLL;
using huhu.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace huhu.API.Controllers
{
    /// <summary>
    /// 管理员登录
    /// </summary>
    public class AdminLoginController : ApiController
    {
        public IAdminService AdminBLL { get; set; }
        public IAdminManagerService AdminManagerBLL { get; set; }

        [HttpPost]
        [Route("passport/admin/cipher_login")]
        public HttpResponseMessage Cipher_Login([FromBody] JObject obj)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(obj));
            if (obj_list == null || obj_list.Property("account") == null || obj_list.Property("password") == null)
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            else
                return JsonUtil.ToJson(Cipher(obj_list));
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="obj_list">前端数据集</param>
        /// <returns>执行状态信息</returns>
        private object Cipher(JObject obj_list)
        {
            ResultMsg result = new ResultMsg();
            string account = obj_list["account"].Value<string>();
            string password = obj_list["password"].Value<string>();
            if (RegularUtil.IsPhone(account)) {
                admin_all a = new admin_all();
                a.phone = account;
                a.password = Commom.Encrypt.MD5Util.GetMD5_8(password);
                admin_all Q = AdminBLL.Query_Phone_Password(a).FirstOrDefault();
                if (Q == null) {
                    return result.SetResultMsg((int)ResultCode.ACCOUNT_OR_PASS_INCORRECT, Descripion.GetDescription(ResultCode.ACCOUNT_OR_PASS_INCORRECT));
                }
                switch (Q.audit_status) {
                    case 0: break;
                    case 1: return result.SetResultMsg((int)ResultCode.ADMINISTRATOR_SHUTS_IT_DOWN, Descripion.GetDescription(ResultCode.ADMINISTRATOR_SHUTS_IT_DOWN));
                    default: throw new Exception(string.Format("{0}的audit_status出现致命错误", Q.admin_id));
                }
                Snowflake.Instance.SnowflakesInit(0, 0);
                admin_manager manager = new admin_manager {
                    id = Snowflake.Instance.NextId().ToString(),
                    admin_id = Q.admin_id,
                    ip = NetUtil.GET_ClientIP(),
                    time = TimeUtil.GetCurrentTimestamp().ToString()
                };
                manager.location = NetUtil.GET_Client_Place(manager.ip);
                AdminManagerBLL.Add(manager);
                AdminManagerBLL.SaveChanges();
                return result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), Token(Q.admin_id));
            }
            return result.SetResultMsg((int)ResultCode.PHONE_FORMAT_IS_WRONG, Descripion.GetDescription(ResultCode.PHONE_FORMAT_IS_WRONG));
        }

        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        private string Token(string user_id)
        {
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


    }
}