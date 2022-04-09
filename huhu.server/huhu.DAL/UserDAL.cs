using huhu.IDAL;
using huhu.Model;
using System.Collections.Generic;
using System.Linq;

namespace huhu.DAL
{
    public class UserDAL : BaseDAL<user_all>, IUserDAL
    {
        /// <summary>
        /// 获取数据总条数
        /// </summary>
        /// <returns></returns>
        public int GetCount() {
            return Count(x => x.user_id.Contains(""));
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public List<user_all> Query_ID(user_all n) {
            return GetEntities(x => x.user_id == n.user_id).ToList();
        }

        /// <summary>
        /// 根据手机号查询
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public List<user_all> Query_Phone(user_all n) {
            return GetEntities(x => x.phone == n.phone).ToList();
        }

        /// <summary>
        /// 根据邮箱查询
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public List<user_all> Query_Email(user_all n) {
            return GetEntities(x => x.email == n.email).ToList();
        }

        /// <summary>
        /// 根据用户手机和密码查询
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public List<user_all> Query_Phone_Password(user_all n) {
            return GetEntities(x => x.phone == n.phone && x.password == n.password).ToList();
        }

        /// <summary>
        /// 根据用户邮箱和密码查询
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public List<user_all> Query_Email_Password(user_all n) {
            return GetEntities(x => x.email == n.email && x.password == n.password).ToList();
        }

        /// <summary>
        /// 查询全部
        /// </summary>
        /// <returns></returns>
        public List<user_all> Query_All() {
            List<user_all> list = GetEntities(x => x.user_id != null).ToList();
            return list;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<user_all> PagingQuery(int PageIndex, int PageSize) {
            return GetConditionPagingQuery(x => x.user_id.Contains(""), k => k.user_id.Contains(""), PageIndex, PageSize).ToList();
        }

        /// <summary>
        /// 条件更新
        /// </summary>
        /// <param name="n"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public bool Update_Condition(user_all n, string[] condition) {
            return UpdateCondition(n, condition);
        }


        public List<user_all> Query_Status(user_all n) {
            return GetEntities(x => x.status == n.status).ToList();
        }

        public List<user_all> Search(string key_word, int status) {
            return GetEntities(x => x.user_id == key_word | x.place.Contains(key_word) & x.status == status).ToList();
        }

        public List<user_all> Query_ALL_NOTStatus() {
            return GetEntities(x => x.status != 0).ToList();
        }

        public user_all Query_QQ_Openid(user_all user) {
            return GetEntities(x => x.qq_openid == user.qq_openid).FirstOrDefault();
        }

    }
}
