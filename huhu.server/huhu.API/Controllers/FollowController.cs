using huhu.API.Filters;
using huhu.Commom;
using huhu.Commom.Enums;
using huhu.Commom.Snowflake;
using huhu.Commom.Token;
using huhu.IBLL;
using huhu.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace huhu.API.Controllers
{
    /// <summary>
    /// 关注作者
    /// </summary>
    public class FollowController : ApiController
    {
        public IUserService UserBLL { get; set; }
        public IFollowService FollowBLL { get; set; }


        [HttpPost]
        [Route("interact_api/v1/follow/do")]
        [SignSecurityFilter]
        [TokenSecurityFilter]
        public HttpResponseMessage Do([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            string[] ef_item = new string[] { "id", "type" };
            foreach (var item in ef_item) {
                if (obj_list.Property(item) == null || obj_list[item].Value<string>() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }
            Snowflake.Instance.SnowflakesInit(0, 0);
            user_follow follow = new user_follow {
                id = Snowflake.Instance.NextId().ToString(),
                user_id = token.get_and_parse(ActionContext).setParams,
                follow_id = obj_list["id"].Value<string>(),
                follow_type = obj_list["type"].Value<int>(),
                ctime = TimeUtil.GetCurrentTimestamp().ToString()
            };

            switch (follow.follow_type) {
                case 1: {
                        user_all user = new user_all {
                            user_id = follow.follow_id
                        };
                        if (follow.user_id == follow.follow_id) {
                            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.CANT_FOCUS_ON_YOURSELF, Descripion.GetDescription(ResultCode.CANT_FOCUS_ON_YOURSELF)));
                        }
                        if (UserBLL.Query_ID(user).FirstOrDefault() == null) {
                            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.USER_NOT_EXIST, Descripion.GetDescription(ResultCode.USER_NOT_EXIST)));
                        }
                    }; break;
                default: return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.OPERATION_TYPE_ERROR, Descripion.GetDescription(ResultCode.OPERATION_TYPE_ERROR)));
            }

            if (FollowBLL.Is_Follow(follow).FirstOrDefault() != null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.REPETITIVE_OPERATION, Descripion.GetDescription(ResultCode.REPETITIVE_OPERATION)));
            }

            FollowBLL.Add(follow);
            FollowBLL.SaveChanges();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS)));
        }


        [HttpPost]
        [Route("interact_api/v1/follow/undo")]
        [SignSecurityFilter]
        [TokenSecurityFilter]
        public HttpResponseMessage Undo([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            string[] ef_item = new string[] { "id", "type" };
            foreach (var item in ef_item) {
                if (obj_list.Property(item) == null || obj_list[item].Value<string>() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }
            user_follow follow = new user_follow {
                user_id = token.get_and_parse(ActionContext).setParams,
                follow_id = obj_list["id"].Value<string>(),
                follow_type = obj_list["type"].Value<int>()
            };

            switch (follow.follow_type) {
                case 1: {
                        user_all user = new user_all();
                        user.user_id = follow.follow_id;
                        if (UserBLL.Query_ID(user).FirstOrDefault() == null) {
                            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.USER_NOT_EXIST, Descripion.GetDescription(ResultCode.USER_NOT_EXIST)));
                        }
                    }; break;
                default: return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.OPERATION_TYPE_ERROR, Descripion.GetDescription(ResultCode.OPERATION_TYPE_ERROR)));
            }

            user_follow Fol = FollowBLL.Is_Follow(follow).FirstOrDefault();
            if (Fol == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.OPERATION_FAILURE, Descripion.GetDescription(ResultCode.OPERATION_FAILURE)));
            }
            follow.id = Fol.id;
            FollowBLL.Delete(follow);
            FollowBLL.SaveChanges();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS)));
        }

    }
}