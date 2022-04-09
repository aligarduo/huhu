using System;
using System.Threading;

namespace huhu.Commom.Snowflake
{
    /// <summary>
    /// 【C#实现Snowflake算法】
    /// 动态生产有规律的ID，Snowflake算法是Twitter的工程师为实现递增而不重复的ID需求实现的分布式算法可排序ID
    /// Twitter的分布式雪花算法 SnowFlake 每秒自增生成26个万个可排序的ID
    /// 1、twitter的SnowFlake生成ID能够按照时间有序生成
    /// 2、SnowFlake算法生成id的结果是一个64bit大小的整数
    /// 3、分布式系统内不会产生重复id（用有datacenterId和machineId来做区分）
    /// =>datacenterId（分布式）（服务ID 1，2，3.....） 每个服务中写死
    /// =>machineId（用于集群） 机器ID 读取机器的环境变量MACHINEID 部署时每台服务器ID不一样
    /// 参考：https://www.cnblogs.com/shiningrise/p/5727895.html
    /// </summary>
    public class Snowflake : ReflectionSingleton<Snowflake>
    {
        /// <summary>
        /// 构造函数私有化
        /// </summary>
        private Snowflake() { }

        #region 初始化字段
        private static long machineId;//机器ID
        private static long datacenterId = 0L;//数据ID
        private static long sequence = 0L;//序列号,计数从零开始

        private static readonly long twepoch = 687888001020L; //起始的时间戳，唯一时间变量，这是一个避免重复的随机量，自行设定不要大于当前时间戳

        private static readonly long machineIdBits = 5L; //机器码字节数
        private static readonly long datacenterIdBits = 5L; //数据字节数
        public static readonly long maxMachineId = -1L ^ -1L << (int)machineIdBits; //最大机器ID
        public static readonly long maxDatacenterId = -1L ^ (-1L << (int)datacenterIdBits);//最大数据ID

        private static readonly long sequenceBits = 12L; //计数器字节数，12个字节用来保存计数码        
        private static readonly long machineIdShift = sequenceBits; //机器码数据左移位数，就是后面计数器占用的位数
        private static readonly long datacenterIdShift = sequenceBits + machineIdBits; //数据中心码数据左移位数
        private static readonly long timestampLeftShift = sequenceBits + machineIdBits + datacenterIdBits; //时间戳左移动位数就是机器码+计数器总字节数+数据字节数
        public static readonly long sequenceMask = -1L ^ -1L << (int)sequenceBits; //一微秒内可以产生计数，如果达到该值则等到下一微妙在进行生成
        private static long lastTimestamp = -1L;//最后时间戳

        private static readonly object syncRoot = new object(); //加锁对象 
        #endregion

        #region Snowflake
        /// <summary>
        /// 数据初始化
        /// </summary>
        /// <param name="machineId">机器Id</param>
        /// <param name="datacenterId">数据中心Id</param>
        public void SnowflakesInit(short machineId, short datacenterId)
        {
            if (machineId < 0 || machineId > Snowflake.maxMachineId)
            {
                throw new ArgumentOutOfRangeException($"The machineId is illegal! => Range interval [0,{Snowflake.maxMachineId}]");
            }
            else
            {
                Snowflake.machineId = machineId;
            }

            if (datacenterId < 0 || datacenterId > Snowflake.maxDatacenterId)
            {
                throw new ArgumentOutOfRangeException($"The datacenterId is illegal! => Range interval [0,{Snowflake.maxDatacenterId}]");
            }
            else
            {
                Snowflake.datacenterId = datacenterId;
            }
        }

        /// <summary>
        /// 生成当前时间戳
        /// </summary>
        /// <returns>时间戳:毫秒</returns>
        private static long GetTimestamp()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

        /// <summary>
        /// 获取下一微秒时间戳
        /// </summary>
        /// <param name="lastTimestamp"></param>
        /// <returns>时间戳:毫秒</returns>
        private static long GetNextTimestamp(long lastTimestamp)
        {
            long timestamp = GetTimestamp();
            int count = 0;
            while (timestamp <= lastTimestamp)//这里获取新的时间,可能会有错,这算法与comb一样对机器时间的要求很严格
            {
                count++;
                if (count > 10) throw new Exception("The machine may not get the right time.");
                Thread.Sleep(1);
                timestamp = GetTimestamp();
            }
            return timestamp;
        }

        /// <summary>
        /// 获取长整形的ID
        /// </summary>
        /// <returns>分布式Id</returns>
        public long NextId()
        {
            lock (syncRoot)
            {
                long timestamp = GetTimestamp();
                if (Snowflake.lastTimestamp == timestamp)
                {
                    //同一微妙中生成ID
                    Snowflake.sequence = (Snowflake.sequence + 1) & Snowflake.sequenceMask; //用&运算计算该微秒内产生的计数是否已经到达上限
                    if (Snowflake.sequence == 0)
                    {
                        //一微妙内产生的ID计数已达上限，等待下一微妙
                        timestamp = GetNextTimestamp(Snowflake.lastTimestamp);
                    }
                }
                else
                {
                    //不同微秒生成ID
                    Snowflake.sequence = 0L; //计数清0
                }
                if (timestamp < Snowflake.lastTimestamp)
                {
                    //如果当前时间戳比上一次生成ID时时间戳还小，抛出异常，因为不能保证现在生成的ID之前没有生成过
                    throw new Exception($"Clock moved backwards.  Refusing to generate id for {Snowflake.lastTimestamp - timestamp} milliseconds!");
                }
                Snowflake.lastTimestamp = timestamp; //把当前时间戳保存为最后生成ID的时间戳
                long id = ((timestamp - Snowflake.twepoch) << (int)Snowflake.timestampLeftShift)
                    | (datacenterId << (int)Snowflake.datacenterIdShift)
                    | (machineId << (int)Snowflake.machineIdShift)
                    | Snowflake.sequence;
                return id;
            }
        }
        #endregion
    }
}
