using System;

namespace huhu.Commom
{
    public static class TimeUtil
    {
        #region 获取当前时间戳
        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <param name="millisecond">精度（毫秒）设置 true，则生成13位的时间戳;精度（秒）设置为 false,则生成10位的时间戳;默认为 true</param>
        /// <returns></returns>
        public static string GetCurrentTimestamp(bool millisecond = false)
        {
            return DateTime.Now.ToTimestamp(millisecond);
        }
        #endregion

        #region 转换指定时间得到对应的时间戳
        /// <summary>
        /// 转换指定时间得到对应的时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="millisecond">精度（毫秒）设置 true,则生成13位的时间戳;精度（秒）设置为 false,则生成10位的时间戳;默认为 true</param>
        /// <returns>返回对应的时间戳</returns>
        public static string ToTimestamp(this DateTime dateTime, bool millisecond = false)
        {
            return dateTime.ToTimestampLong(millisecond).ToString();
        }
        #endregion

        #region 转换指定时间得到对应的时间戳
        /// <summary>
        /// 转换指定时间得到对应的时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="millisecond">精度（毫秒）设置 true,则生成13位的时间戳;精度（秒）设置为 false,则生成10位的时间戳;默认为 true</param>
        /// <returns>返回对应的时间戳</returns>
        public static long ToTimestampLong(this DateTime dateTime, bool millisecond = false)
        {
            var ts = dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return millisecond ? Convert.ToInt64(ts.TotalMilliseconds) : Convert.ToInt64(ts.TotalSeconds);
        }
        #endregion

        #region 转换指定时间戳到对应的时间
        /// <summary>
        /// 转换指定时间戳到对应的时间
        /// </summary>
        /// <param name="timestamp">（10位或13位）时间戳</param>
        /// <returns>返回对应的时间</returns>
        public static DateTime ToDateTime(this string timestamp)
        {
            var tz = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            return timestamp.Length == 13
                ? tz.AddMilliseconds(Convert.ToInt64(timestamp))
                : tz.AddSeconds(Convert.ToInt64(timestamp));
        }
        #endregion

        #region 验证时间戳（10位、13位）
        /// <summary>
        /// 验证时间戳（10位、13位皆可）
        /// </summary>
        /// <param name="timestamp">时间戳</param>
        /// <param name="timeDiff">允许时差（10位时单位为 秒，13位时单位为 毫秒）</param>
        /// <param name="timeKind">时间类型（只能为 Local、Utc，默认 Local）</param>
        /// <returns></returns>
        public static bool ValidateTimestamp(double timestamp, int timeDiff, DateTimeKind timeKind = DateTimeKind.Local)
        {
            TimeSpan timeSpan = new TimeSpan();

            switch (timeKind)
            {
                case DateTimeKind.Utc: timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, timeKind); break;
                case DateTimeKind.Local: timeSpan = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, timeKind); break;
                default: throw new Exception("时间类型只能为 Local、Utc");
            }

            double nowTimestamp = 0;  //现在的时间戳
            int format = timestamp.ToString("f0").Length;

            switch (format)
            {
                case 10: nowTimestamp = double.Parse(TimeUtil.GetCurrentTimestamp(false)); break;
                case 13: nowTimestamp = double.Parse(TimeUtil.GetCurrentTimestamp(true)); break;
                default: throw new Exception("时间戳格式错误");
            }

            double nowTimeDiff = nowTimestamp - timestamp;  //现在的时差

            if (-timeDiff <= nowTimeDiff && nowTimeDiff <= timeDiff)
                return true;
            else
                return false;
        }
        #endregion

    }
}
