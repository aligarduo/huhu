using huhu.IBLL;
using huhu.Model;
using System.Collections.Generic;

namespace huhu.BLL
{
    public class UserService : BaseService<user_all>, IUserService
    {
        /// <summary>
        /// 获取数据总条数
        /// </summary>
        /// <returns></returns>
        public int GetCount() {
            return db.UserDal.GetCount();
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public List<user_all> Query_ID(user_all user) {
            return db.UserDal.Query_ID(user);
        }

        /// <summary>
        /// 根据手机号查询
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public List<user_all> Query_Phone(user_all user) {
            return db.UserDal.Query_Phone(user);
        }

        /// <summary>
        /// 根据邮箱查询
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public List<user_all> Query_Email(user_all user) {
            return db.UserDal.Query_Email(user);
        }

        /// <summary>
        /// 根据用户手机和密码查询
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public List<user_all> Query_Phone_Password(user_all user) {
            return db.UserDal.Query_Phone_Password(user);
        }

        /// <summary>
        /// 根据用户邮箱和密码查询
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public List<user_all> Query_Email_Password(user_all user) {
            return db.UserDal.Query_Email_Password(user);
        }

        /// <summary>
        /// 查询全部
        /// </summary>
        /// <returns></returns>
        public List<user_all> Query_All() {
            return db.UserDal.Query_All();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<user_all> PagingQuery(int pageIndex, int pageSize) {
            return db.UserDal.PagingQuery(pageIndex, pageSize);
        }

        /// <summary>
        /// 条件更新
        /// </summary>
        /// <param name="n"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public bool Update_Condition(user_all n, string[] condition) {
            return db.UserDal.Update_Condition(n, condition);
        }

        public List<user_all> Query_Status(user_all user) {
            return db.UserDal.Query_Status(user);
        }

        public List<user_all> Search(string key_word, int status) {
            return db.UserDal.Search(key_word, status);
        }

        public List<user_all> Query_ALL_NOTStatus() {
            return db.UserDal.Query_ALL_NOTStatus();
        }

        public user_all Query_QQ_Openid(user_all user) {
            return db.UserDal.Query_QQ_Openid(user);
        }

    }
}