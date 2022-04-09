using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace huhu.Commom.GradualID
{
    public class UseridUtil
    {
        /// <summary>
        /// 生成10位纯数字id
        /// </summary>
        /// <returns></returns>
        public static string IDByGUId()
        {
            int hashCodeV = Guid.NewGuid().ToString().GetHashCode();
            if (hashCodeV < 0)
            {
                hashCodeV = -hashCodeV;
            }
            return string.Format("{0}", hashCodeV).PadRight(10, '0');
        }
    }
}
