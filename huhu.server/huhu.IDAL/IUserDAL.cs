using huhu.Model;
using System.Collections.Generic;

namespace huhu.IDAL
{
    public interface IUserDAL : IBaseDAL<user_all>
    {
        /// <summary>
        /// 获取数据总条数
        /// </summary>
        /// <returns></returns>
        int GetCount();

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        List<user_all> Query_ID(user_all n);

        /// <summary>
        /// 根据手机号查询
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        List<user_all> Query_Phone(user_all n);

        /// <summary>
        /// 根据邮箱查询
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        List<user_all> Query_Email(user_all n);

        /// <summary>
        /// 根据用户手机和密码查询
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        List<user_all> Query_Phone_Password(user_all n);

        /// <summary>
        /// 根据用户邮箱和密码查询
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        List<user_all> Query_Email_Password(user_all n);

        /// <summary>
        /// 查询全部
        /// </summary>
        /// <returns></returns>
        List<user_all> Query_All();

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        List<user_all> PagingQuery(int PageIndex, int PageSize);

        /// <summary>
        /// 条件更新
        /// </summary>
        /// <param name="n"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        bool Update_Condition(user_all n, string[] condition);

        List<user_all> Query_Status(user_all n);
        List<user_all> Search(string key_word, int status);
        List<user_all> Query_ALL_NOTStatus();
        user_all Query_QQ_Openid(user_all user);
    }
}
