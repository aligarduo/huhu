using System;

namespace huhu.Commom
{
    public class GUIDUtil
    {
        /// <summary>
        /// 16位 GUID
        /// </summary>
        /// <returns></returns>
        public static string GuidTo16()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
                i *= ((int)b + 1);
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        /// <summary>  
        /// 19位 GUID
        /// </summary>  
        /// <returns></returns>  
        public static long GuidTo19()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

        /// <summary>  
        /// 32位 GUID
        /// </summary>  
        /// <returns></returns>  
        public static string GuidTo32() {
            return Guid.NewGuid().ToString("N");
        }

    }
}
