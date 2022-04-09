using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace huhu.Commom.Encrypt
{
    /// <summary>
    /// MD5加密解密工具类
    /// </summary>
    public class MD5Util
    {
        /// <summary>
        /// 32位MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMD5_32(string str) {
            MD5 md5 = MD5.Create();
            byte[] data = md5.ComputeHash(Encoding.Default.GetBytes(str));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++) {
                sb.Append(data[i].ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 16位MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMD5_16(string str) {
            return GetMD5_32(str).Substring(8, 16);
        }

        /// <summary>
        /// 8位MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMD5_8(string str) {
            return GetMD5_32(str).Substring(8, 8);
        }

        /// <summary>
        /// 4位MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMD5_4(string str) {
            return GetMD5_32(str).Substring(8, 4);
        }

        /// <summary>
        /// 添加MD5前缀，便于检查有无篡改
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string AddMD5Profix(string str) {
            return GetMD5_4(str) + str;
        }

        /// <summary>
        /// 移除MD5前缀
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveMD5Profix(string str) {
            return str.Substring(4);
        }

        /// <summary>
        /// 验证MD5前缀处理的字符串有无被篡改
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool ValidateValue(string str) {
            bool res = false;
            if (str.Length >= 4) {
                string tmp = str.Substring(4);
                if (str.Substring(0, 4) == GetMD5_4(tmp)) {
                    res = true;
                }
            }
            return res;
        }

        /// <summary>
        /// 获取文件MD5
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string GetMD5HashFromFile(Stream stream) {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] _byte = md5.ComputeHash(stream);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _byte.Length; i++) {
                sb.Append(_byte[i].ToString("x2"));
            }
            return sb.ToString();
        }

    }
}