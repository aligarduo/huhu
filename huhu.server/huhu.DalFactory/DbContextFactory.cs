using huhu.Model;
using System.Data.Entity;
using System.Runtime.Remoting.Messaging;

namespace huhu.DalFactory
{
    public class DbContextFactory
    {
        public static DbContext GetCurrentDbContext()
        {
            DbContext db = CallContext.GetData("DbContext") as DbContext;
            if (db == null)
            {
                db = new huhuEntities();
                CallContext.SetData("DbContext", db);
            }
            return db;
        }
    }
}
