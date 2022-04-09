using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace huhu.Commom
{
    public class RedisUtil
    {
        public RedisUtil() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="db"></param>
        /// <param name="connectStr"></param>
        public RedisUtil(int db, string connectStr)
        {
            _conn = connectStr;
            _db = db;
        }

        /// <summary>
        /// redis数据库连接字符串
        /// </summary>
        private readonly string _conn = ConfigurationManager.AppSettings["RedisConnect"].ToString();
        private readonly int _db = 0;

        /// <summary>
        /// 静态变量 保证各模块使用的是不同实例的相同链接
        /// </summary>
        private static ConnectionMultiplexer connection;

        /// <summary>
        /// 缓存数据库，数据库连接
        /// </summary>
        public ConnectionMultiplexer CacheConnection
        {
            get
            {
                try
                {
                    if (connection == null || !connection.IsConnected)
                    {
                        connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(_conn)).Value;
                    }
                }
                catch (Exception)
                {
                    return null;
                }
                return connection;
            }
        }

        /// <summary>
        /// 缓存数据库
        /// </summary>
        public IDatabase CacheRedis => CacheConnection.GetDatabase(_db);

        /// <summary>
        /// 保存一条数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <returns>bool-> true or false</returns>
        public bool StringSet(string key, string value)
        {
            return CacheRedis.StringSet(key, value);
        }

        /// <summary>
        /// 保存一条数据，可设置过期时间
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public bool StringSet(string key, string value, TimeSpan? expiry = default(TimeSpan?))
        {
            return CacheRedis.StringSet(key, value, expiry);
        }

        /// <summary>
        /// 保存多条数据
        /// </summary>
        /// <param name="arr">key</param>
        /// <returns></returns>
        public bool StringSet(KeyValuePair<RedisKey, RedisValue>[] arr)
        {
            return CacheRedis.StringSet(arr);
        }

        /// <summary>
        /// 批量存值
        /// </summary>
        /// <param name="keysStr">key</param>
        /// <param name="valuesStr">value</param>
        /// <returns>bool-> true or false</returns>
        public bool StringSetMany(string[] keysStr, string[] valuesStr)
        {
            var count = keysStr.Length;
            var keyValuePair = new KeyValuePair<RedisKey, RedisValue>[count];
            for (int i = 0; i < count; i++)
            {
                keyValuePair[i] = new KeyValuePair<RedisKey, RedisValue>(keysStr[i], valuesStr[i]);
            }

            return CacheRedis.StringSet(keyValuePair);
        }

        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool SetStringKey<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?))
        {
            string json = JsonConvert.SerializeObject(obj);
            return CacheRedis.StringSet(key, json, expiry);
        }

        /// <summary>
        /// 追加值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void StringAppend(string key, string value)
        {
            //追加值，返回追加后长度
            long appendlong = CacheRedis.StringAppend(key, value);
        }

        /// <summary>
        /// 获取单个key的值
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>当前key对应的value值</returns>
        public RedisValue GetStringKey(string key)
        {
            return CacheRedis.StringGet(key);
        }

        /// <summary>
        /// 根据Key获取值
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>String</returns>
        public string StringGet(string key)
        {
            try
            {
                return CacheRedis.StringGet(key);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取多个Key
        /// </summary>
        /// <param name="listKey">Redis Key集合</param>
        /// <returns></returns>
        public RedisValue[] GetStringKey(List<RedisKey> listKey)
        {
            return CacheRedis.StringGet(listKey.ToArray());
        }

        /// <summary>
        /// 批量获取值
        /// </summary>
        public string[] StringGetMany(string[] keyStrs)
        {
            var count = keyStrs.Length;
            var keys = new RedisKey[count];
            var addrs = new string[count];

            for (var i = 0; i < count; i++)
            {
                keys[i] = keyStrs[i];
            }
            try
            {

                var values = CacheRedis.StringGet(keys);
                for (var i = 0; i < values.Length; i++)
                {
                    addrs[i] = values[i];
                }
                return addrs;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取一个key的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetStringKey<T>(string key)
        {
            return JsonConvert.DeserializeObject<T>(CacheRedis.StringGet(key));
        }

        /// <summary>
        /// 删除一个key
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>bool-> true or false</returns>
        public bool KeyDelete(string key)
        {
            return CacheRedis.KeyDelete(key);
        }

        /// <summary>
        /// 删除多个key
        /// </summary>
        /// <param name="keys">key</param>
        /// <returns>成功删除的个数</returns>
        public long KeyDelete(RedisKey[] keys)
        {
            return CacheRedis.KeyDelete(keys);
        }

        /// <summary>
        /// 判断key是否存储
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public bool KeyExists(string key)
        {
            return CacheRedis.KeyExists(key);
        }

        /// <summary>
        /// 重新命名key
        /// </summary>
        /// <param name="key">就的redis key</param>
        /// <param name="newKey">新的redis key</param>
        /// <returns></returns>
        public bool KeyRename(string key, string newKey)
        {
            return CacheRedis.KeyRename(key, newKey);
        }

        /// <summary>
        /// 删除hasekey
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public bool HaseDelete(RedisKey key, RedisValue hashField)
        {
            return CacheRedis.HashDelete(key, hashField);
        }

        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public bool HashRemove(string key, string dataKey)
        {
            return CacheRedis.HashDelete(key, dataKey);
        }

        /// <summary>
        /// 设置缓存过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="datetime"></param>
        public void SetExpire(string key, DateTime datetime)
        {
            CacheRedis.KeyExpire(key, datetime);
        }
        
    }
}
