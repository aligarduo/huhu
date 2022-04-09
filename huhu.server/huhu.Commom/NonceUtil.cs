using System;
using System.Text;

namespace huhu.Commom
{
    /// <summary>
    /// 产生随机数工具类
    /// </summary>
    public class NonceUtil
    {
        /// <summary>
        /// 返回指定个数的随机数
        /// </summary>
        /// <param name="length">个数</param>
        /// <returns>随机数</returns>
        public static string GenerateRandomCode(int length)
        {
            var result = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                var r = new Random(Guid.NewGuid().GetHashCode());
                result.Append(r.Next(1, 10));
            }
            return result.ToString();
        }
    }
}
