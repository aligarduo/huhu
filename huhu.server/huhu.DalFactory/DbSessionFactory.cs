using System.Runtime.Remoting.Messaging;

namespace huhu.DalFactory
{
    public class DbSessionFactory
    {
        public static DbSession GetDbSession()
        {
            //获取唯一的一个对象
            DbSession db = CallContext.GetData("DbSession") as DbSession;
            if (db == null)
            {
                db = new DbSession(); //new了对象之后还要写key到线程的本地存储区中
                CallContext.SetData("DbSession", db);
            }
            return db;
        }
    }
}
