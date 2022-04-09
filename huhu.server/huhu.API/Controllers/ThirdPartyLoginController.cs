using huhu.Commom;
using huhu.Commom.Enums;
using huhu.Commom.Token;
using huhu.IBLL;
using huhu.Model;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace huhu.API.Controllers
{
    public class ThirdPartyLoginController : ApiController
    {
        public IUserService UserBLL { get; set; }

        [HttpGet]
        [Route("passport/thirdparty/qq_auth")]
        public HttpResponseMessage RichScan_Login([FromUri] string code) {
            //获取access_token
            string url = "https://graph.qq.com/oauth2.0/token";
            string param = "?";
            param += "grant_type=authorization_code&";
            param += "client_id=101995093&";
            param += "client_secret=ede26bd5c993a53259de55db1312e5f5&";
            param += "code=" + code + "&";
            //本地调试
            //param += "redirect_uri=http://3c875j1492.wicp.vip/passport/thirdparty/qq_auth&";
            //服务器
            param += "redirect_uri=https://api.huhu.chat/passport/thirdparty/qq_auth&";
            param += "fmt=json";
            string aa = HttpUtil.Send(url + param);
            dynamic response1 = JsonConvert.DeserializeObject(aa);
            string access_token = response1.access_token;

            //获取openid
            string url2 = "https://graph.qq.com/oauth2.0/me";
            string param2 = "?";
            param2 += "access_token=" + access_token + "&";
            param2 += "fmt=json";
            string bb = HttpUtil.Send(url2 + param2);
            dynamic response2 = JsonConvert.DeserializeObject(bb);
            string openid = response2.openid;

            //获取QQ用户信息
            string url3 = "https://graph.qq.com/user/get_user_info";
            string param3 = "?";
            param3 += "access_token=" + access_token + "&";
            param3 += "oauth_consumer_key=101995093&";
            param3 += "openid=" + openid;
            string cc = HttpUtil.Send(url3 + param3);
            dynamic response3 = JsonConvert.DeserializeObject(cc);
            string nickname = response3.nickname;
            string figureurl_qq = response3.figureurl_qq;
            string province_city = response3.province + response3.city;


            string setParams, user_id = Commom.GradualID.UseridUtil.IDByGUId();
            user_all user = DataAssignment(user_id, nickname, "", "", "", "", openid, nickname, figureurl_qq, "", "", 1, "", province_city, 0);
            var data = UserBLL.Query_QQ_Openid(user);
            if (data == null) {
                UserBLL.Add(user);
                UserBLL.SaveChanges();
                setParams = user.user_id;
            } else {
                setParams = data.user_id;
            }

            string token = Token(setParams);
            string js = "登录成功";
            js += "<script>";
            js += "window.opener.postMessage('" + token + "','*');";
            js += "window.close();";
            js += "</script>";

            HttpResponseMessage hrm = new HttpResponseMessage {
                Content = new StringContent(js, Encoding.GetEncoding("UTF-8"), "text/html")
            };
            return hrm;
        }


        #region 数据赋值
        /// <summary>
        /// 数据赋值
        /// </summary>
        /// <param name="user_id">用户id</param>
        /// <param name="user_name">用户名</param>
        /// <param name="phone">手机号</param>
        /// <param name="avatar_large">用户头像</param>
        /// <param name="password">登录密码</param>
        /// <param name="email">邮箱地址</param>
        /// <param name="qq_openid">QQ官方唯一标识</param>
        /// <param name="qq_nickname">QQ昵称</param>
        /// <param name="qq_figureurl">QQ头像</param>
        /// <param name="wechat_nickname">微信名</param>
        /// <param name="weibo_nickname">微博名</param>
        /// <param name="level">用户等级</param>
        /// <param name="job_title">用户个性签名</param>
        /// <param name="place">所在地区</param>
        /// <param name="status">账号状态</param>
        /// <returns>user_all</returns>
        private user_all DataAssignment(
            string user_id,
            string user_name,
            string phone,
            string avatar_large,
            string password,
            string email,
            string qq_openid,
            string qq_nickname,
            string qq_figureurl,
            string wechat_nickname,
            string weibo_nickname,
            int level,
            string job_title,
            string place,
            int status) {
            user_all user = new user_all();
            user.user_id = user_id;
            user.user_name = user_name;
            user.phone = phone;
            user.avatar_large = avatar_large;
            user.password = password;
            user.phone_verified = phone.Equals(string.Empty) ? 0 : 1;
            user.email = email;
            user.email_verified = email.Equals(string.Empty) ? 0 : 1;
            user.qq_openid = qq_openid;
            user.qq_nickname = qq_nickname;
            user.qq_figureurl = qq_figureurl;
            user.qq_verified = qq_figureurl.Equals(string.Empty) ? 0 : 1;
            user.wechat_nickname = wechat_nickname;
            user.wechat_verified = wechat_nickname.Equals(string.Empty) ? 0 : 1;
            user.weibo_nickname = weibo_nickname;
            user.weibo_verified = weibo_nickname.Equals(string.Empty) ? 0 : 1;
            user.level = level;
            user.job_title = job_title;
            user.ip_address = NetUtil.GET_ClientIP();
            user.place = place.Equals(string.Empty) ? NetUtil.GET_Client_Place(user.ip_address) : place;
            user.status = status;
            user.update_time = TimeUtil.GetCurrentTimestamp().ToString();
            user.register_time = TimeUtil.GetCurrentTimestamp().ToString();
            return user;
        } 
        #endregion


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





    }
}
