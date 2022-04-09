using huhu.API.Filters;
using huhu.Commom;
using huhu.Commom.Encrypt;
using huhu.Commom.Enums;
using huhu.Commom.Token;
using huhu.IBLL;
using huhu.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;

namespace huhu.API.Controllers
{
    /// <summary>
    /// 用户个人信息
    /// </summary>
    public class UserController : ApiController
    {

        #region 配置
        /// <summary>
        /// 主文件服务器
        /// </summary>
        private static readonly string MainFileServer = ConfigurationManager.AppSettings["MainFileServer"].ToString();
        /// <summary>
        /// 用户默认头像存放文件夹
        /// </summary>
        private static readonly string UserDefaultAvatar = ConfigurationManager.AppSettings["UserDefaultAvatar"].ToString();
        #endregion

        #region 接口
        public IUserService UserBLL { get; set; }
        public IArticleService ArticleBLL { get; set; }
        public IDiggService DiggBLL { get; set; }
        public IArticleViewService ArticleViewBLL { get; set; }
        public IFollowService FollowBLL { get; set; }
        #endregion

        [HttpGet]
        [Route("user_api/v1/user/get")]
        [TokenSecurityFilter]
        public HttpResponseMessage GetInfo() {
            ResultMsg result = new ResultMsg();
            user_all user_full = new user_all {
                user_id = token.get_and_parse(ActionContext).setParams
            };
            user_all user = UserBLL.Query_ID(user_full).FirstOrDefault();
            switch (user.status) {
                case 0: break;
                case 1: return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.ACCOUNT_ABNORMAL_LIMIT, Descripion.GetDescription(ResultCode.ACCOUNT_ABNORMAL_LIMIT)));
                case 2: return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.ACCOUNT_VIOLATION_LIMIT, Descripion.GetDescription(ResultCode.ACCOUNT_VIOLATION_LIMIT)));
                case 3: return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.ACCOUNT_VIOLATION_CLOSED, Descripion.GetDescription(ResultCode.ACCOUNT_VIOLATION_CLOSED)));
                case 4: return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.ACCOUNT_IS_CANCELLED, Descripion.GetDescription(ResultCode.ACCOUNT_IS_CANCELLED)));
                default: throw new Exception($"{user.user_id}的status出现致命的错误");
            }
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), MaskingSpecialData(user)));
        }

        [HttpGet]
        [Route("user_api/v1/user/get")]
        public HttpResponseMessage GetInfo([FromUri] string user_id) {
            ResultMsg result = new ResultMsg();
            user_all user_full = new user_all { user_id = user_id };
            user_all user = UserBLL.Query_ID(user_full).FirstOrDefault();
            if (user == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.USER_NOT_EXIST, Descripion.GetDescription(ResultCode.USER_NOT_EXIST)));
            }
            switch (user.status) {
                case 0: break;
                case 1: return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.ACCOUNT_ABNORMAL_LIMIT, Descripion.GetDescription(ResultCode.ACCOUNT_ABNORMAL_LIMIT)));
                case 2: return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.ACCOUNT_VIOLATION_LIMIT, Descripion.GetDescription(ResultCode.ACCOUNT_VIOLATION_LIMIT)));
                case 3: return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.ACCOUNT_VIOLATION_CLOSED, Descripion.GetDescription(ResultCode.ACCOUNT_VIOLATION_CLOSED)));
                case 4: return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.ACCOUNT_IS_CANCELLED, Descripion.GetDescription(ResultCode.ACCOUNT_IS_CANCELLED)));
                default: throw new Exception($"{user.user_id}的status出现致命的错误");
            }
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), Synthetic_Data(user)));
        }

        private SortedDictionary<string, object> Synthetic_Data(user_all user) {
            SortedDictionary<string, object> SD = MaskingSpecialData(user);

            int got_digg_count = 0, got_view_count = 0;
            user_all _user = new user_all();
            { _user.user_id = user.user_id; }
            foreach (var item in ArticleBLL.Query_UserID(_user)) {
                article_all article = ObjectUtil.ConvertObject<article_all>(item);
                //点赞量
                user_digg _Digg = new user_digg {
                    digg_id = article.article_id,
                    type = 1
                };
                got_digg_count += DiggBLL.Digg_Count(_Digg).Count;//总点赞量
                //阅读量
                article_view view = new article_view {
                    article_id = article.article_id
                };
                got_view_count += ArticleViewBLL.Reading_Quantity(view).view_count; //总阅读量
            }

            user_follow _follow2 = new user_follow();
            { _follow2.follow_type = 1; }
            int followee_count = FollowBLL.Follow(_user, _follow2).Count;
            int fans_count = FollowBLL.Fans(_user, _follow2).Count;

            SD["got_digg_count"] = got_digg_count;//总点赞量
            SD["got_view_count"] = got_view_count;//总阅读量
            SD["followee_count"] = followee_count;//关注
            SD["fans_count"] = fans_count;//粉丝
            return SD;
        }

        [HttpPost]
        [Route("user_api/v1/user/update")]
        [SignSecurityFilter]
        [TokenSecurityFilter]
        public HttpResponseMessage Update([FromBody] JObject obj) {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(obj));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            //校验参数
            List<string> enable_changes = new List<string>();
            Dictionary<string, object> enable_dic = new Dictionary<string, object>();
            string[] ef_item = new string[] { "user_name", "avatar_large", "password", "job_title" };
            foreach (var item in obj_list.Properties()) {
                if (!((IList)ef_item).Contains(item.Name) || item.Value.ToString() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
                enable_changes.Add(item.Name);
                enable_dic.Add(item.Name, obj_list[item.Name].Value<string>());
            }
            //初始化实例
            user_all user = new user_all();
            //动态给实体赋值
            Dictionary<string, object> dic = new Dictionary<string, object>();
            Type t = user.GetType();
            foreach (PropertyInfo pi in t.GetProperties()) {
                if (((IList)enable_changes).Contains(pi.Name)) {
                    pi.SetValue(user, enable_dic[pi.Name], null);
                    object value = pi.GetValue(user, null);
                    dic.Add(pi.Name, value);
                } else {
                    if (pi.PropertyType.Name == "String")
                        dic.Add(pi.Name, "");
                    if (pi.PropertyType.Name == "Int")
                        dic.Add(pi.Name, 0);
                }
            }

            user_all next_user = JsonConvert.DeserializeObject<user_all>(JsonConvert.SerializeObject(dic));
            next_user.user_id = token.get_and_parse(ActionContext).setParams;
            var user_data = UserBLL.Query_ID(next_user).FirstOrDefault();
            if (user_data == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.USER_NOT_EXIST, Descripion.GetDescription(ResultCode.USER_NOT_EXIST)));
            }
            if (!string.IsNullOrEmpty(next_user.avatar_large)) {
                string domainNameServer = ConfigurationManager.AppSettings["MainFileServer"].ToString();
                string domainName = URLUtil.CaptureURL(next_user.avatar_large, "domainName");
                if (!URLUtil.CheckURL(next_user.avatar_large)) {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.INVALID_LINK, Descripion.GetDescription(ResultCode.INVALID_LINK)));
                } else if (domainNameServer != domainName) {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.LINK_NOT_SUPPORTED, Descripion.GetDescription(ResultCode.LINK_NOT_SUPPORTED)));
                } else {
                    next_user.avatar_large = URLUtil.Parsing_URL(next_user.avatar_large);
                }
            }
            next_user.password = next_user.password == "" ? "" : MD5Util.GetMD5_16(next_user.password);
            next_user.update_time = TimeUtil.GetCurrentTimestamp().ToString();
            UserBLL.Update_Condition(next_user, enable_changes.ToArray());
            UserBLL.SaveChanges();

            user_all two_next_user = UserBLL.Query_ID(next_user).FirstOrDefault();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), MaskingSpecialData(two_next_user)));
        }


        /// <summary>
        /// 屏蔽特殊数据
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private SortedDictionary<string, object> MaskingSpecialData(user_all user) {
            user.phone = user.phone.Length > 0 ? user.phone.Substring(0, 3) + "****" + user.phone.Substring(7) : "";
            if (user.avatar_large == "" && user.qq_figureurl != "") {
                user.avatar_large = user.qq_figureurl;
            } else
                user.avatar_large = MainFileServer + user.avatar_large;
            return EntityUtil.ConvertObject<user_all>(user, new string[] { "password", "qq_openid", "qq_figureurl", "ip_address" });
        }
    
    }
}