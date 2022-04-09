using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace huhu.Commom
{
    public class EntityUtil
    {
        /// <summary>
        /// 将实体对象中某些字段过滤掉
        /// </summary>
        /// <param name="Info">实体</param>
        /// <param name="shield">需要过滤的字段名</param>
        /// <returns></returns>
        public static SortedDictionary<string, object> ConvertObject<T>(object Info, string[] shield) where T : new()
        {
            SortedDictionary<string, object> dic = new SortedDictionary<string, object>();
            Type t = Info.GetType();
            foreach (PropertyInfo pi in t.GetProperties())
            {
                if (!((IList)shield).Contains(pi.Name))
                {
                    var value = pi.GetValue(Info, null);
                    if (value == null) value = "";
                    dic.Add(pi.Name, value);
                }
            }
            return dic;
        }

        /// <summary>
        /// 只获取实体对象中某些字段
        /// </summary>
        /// <param name="Info">实体</param>
        /// <param name="shield">需要过滤的字段名</param>
        /// <returns></returns>
        public static SortedDictionary<string, object> GainObject<T>(object Info, string[] shield) where T : new()
        {
            SortedDictionary<string, object> dic = new SortedDictionary<string, object>();
            Type t = Info.GetType();
            foreach (PropertyInfo pi in t.GetProperties())
            {
                if (((IList)shield).Contains(pi.Name))
                {
                    var value = pi.GetValue(Info, null);
                    if (value == null) value = "";
                    dic.Add(pi.Name, value);
                }
            }
            return dic;
        }

        /// <summary>
        /// 将实体转为字典类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Info"></param>
        /// <returns></returns>
        public static SortedDictionary<string, object> EntityByDic<T>(object Info) where T : new()
        {

            PropertyInfo[] cfgItemProperties = Info.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            SortedDictionary<string, object> dic = new SortedDictionary<string, object>();
            foreach (PropertyInfo item in cfgItemProperties)
            {
                string name = item.Name;
                object value = item.GetValue(Info, null);
                if (value != null && (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String")) && !string.IsNullOrWhiteSpace(value.ToString()))
                {
                    dic.Add(name, value.ToString());
                }
            }
            return dic;
        }

    }
}
