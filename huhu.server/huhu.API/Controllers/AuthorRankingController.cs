using huhu.Commom;
using huhu.Commom.Enums;
using huhu.IBLL;
using huhu.Model;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace huhu.API.Controllers
{
    /// <summary>
    /// 作者排行榜
    /// </summary>
    public class RankingController : ApiController
    {
        public IUserService UserBLL { get; set; }
        public IDiggService DiggBLL { get; set; }
        public IArticleService ArticleBLL { get; set; }
        public IArticleViewService ArticleViewBLL { get; set; }

        [HttpGet]
        [Route("user_api/v1/author/ranking")]
        public HttpResponseMessage authorRanking([FromUri] string capacity) {

            ResultMsg result = new ResultMsg();
            int tmp;
            if (!int.TryParse(capacity, out tmp)) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }

            Dictionary<string, int> dic = new Dictionary<string, int> { };

            foreach (var user in UserBLL.Query_All()) {
                int got_digg_count = 0;
                user_all _user = ObjectUtil.ConvertObject<user_all>(user);
                foreach (var item in ArticleBLL.Query_UserID(_user)) {
                    article_all article = ObjectUtil.ConvertObject<article_all>(item);
                    //点赞量
                    user_digg _Digg = new user_digg {
                        digg_id = article.article_id,
                        type = 1
                    };
                    got_digg_count += DiggBLL.Digg_Count(_Digg).Count;//总点赞量
                }
                dic.Add(_user.user_id, got_digg_count);
            }


            Dictionary<string, int> dic1desc = dic.OrderByDescending(p => p.Value).ToDictionary(p => p.Key, p => p.Value);
            //大到小排序
            List<object> result_dic = new List<object>();
            user_all user_info = new user_all();

            int counter = 0;//计数器
            foreach (KeyValuePair<string, int> k in dic1desc) {
                if (counter < int.Parse(capacity)) {
                    counter++;
                    user_info.user_id = k.Key;
                    user_all child_user_info = ObjectUtil.ConvertObject<user_all>(UserBLL.Query_ID(user_info).FirstOrDefault());
                    child_user_info.phone = child_user_info.phone.Substring(0, 3) + "****" + child_user_info.phone.Substring(7);
                    if (child_user_info.avatar_large == "") {
                        child_user_info.avatar_large = ConfigurationManager.AppSettings["MainFileServer"].ToString();
                        child_user_info.avatar_large += ConfigurationManager.AppSettings["DefaultUserProfilePicture"].ToString();
                    } else {
                        child_user_info.avatar_large = ConfigurationManager.AppSettings["MainFileServer"].ToString() + child_user_info.avatar_large;
                    }
                    result_dic.Add(EntityUtil.GainObject<user_all>(child_user_info, new string[] { "avatar_large", "job_title", "level", "user_id", "user_name" }));
                }
            }

            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), result_dic));
        }


        [HttpGet]
        [Route("content_api/v1/article/hot")]
        public HttpResponseMessage articleHot([FromUri] string capacity) {

            ResultMsg result = new ResultMsg();
            if (!int.TryParse(capacity, out int tmp)) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }

            Dictionary<string, int> dic = new Dictionary<string, int> { };
            foreach (var articleView in ArticleViewBLL.Query_All()) {
                article_view _view = ObjectUtil.ConvertObject<article_view>(articleView);
                dic.Add(_view.article_id, _view.view_count);
            }

            //大到小排序
            Dictionary<string, int> dic1desc = dic.OrderByDescending(p => p.Value).ToDictionary(p => p.Key, p => p.Value);


            List<object> result_dic = new List<object>();
            article_all _article = new article_all();
            int counter = 0;//计数器
            foreach (KeyValuePair<string, int> k in dic1desc) {
                if (counter < int.Parse(capacity)) {
                    counter++;
                    _article.article_id = k.Key;

                    object data = ArticleBLL.Query_ID(_article).FirstOrDefault();
                    article_all article_info = ObjectUtil.ConvertObject<article_all>(data);
                    if (article_info.cover_image != "" && article_info.cover_image != null) {
                        article_info.cover_image = ConfigurationManager.AppSettings["MainFileServer"].ToString() + article_info.cover_image;
                    }
                    Dictionary<string, object> _dic = new Dictionary<string, object> {
                        { "article_info", article_info },
                        { "heat_out", k.Value }
                    };
                    result_dic.Add(_dic);
                }
            }

            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), result_dic));
        }



    }
}
