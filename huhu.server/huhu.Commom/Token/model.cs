using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace huhu.Commom.Token
{
    public class model
    {
        /// <summary>
        /// TokenID
        /// </summary>
        public string setId { get; set; }
        /// <summary>
        /// 携带用户参数
        /// </summary>
        public string setParams { get; set; }
        /// <summary>
        /// 签发者
        /// </summary>
        public string setIssuer { get; set; }
        /// <summary>
        /// 签发时间
        /// </summary>
        public string setIssuedAt { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public string setExpiration { get; set; }
    }
}
