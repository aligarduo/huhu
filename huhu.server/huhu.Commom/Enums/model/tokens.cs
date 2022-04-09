using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace huhu.Commom.Enums.model
{
    public class tokens
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int err_no { get; set; }
        /// <summary>
        /// 状态信息
        /// </summary>
        public string err_msg { get; set; }
        /// <summary>
        /// 携带信息
        /// </summary>
        public object token { get; set; }
    }
}
