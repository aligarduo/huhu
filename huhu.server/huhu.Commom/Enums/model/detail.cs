using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace huhu.Commom.Enums.model
{
    public class detail
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
        /// 数据
        /// </summary>
        public object data { get; set; }
    }
}
