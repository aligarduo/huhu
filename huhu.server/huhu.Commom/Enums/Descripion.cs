using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace huhu.Commom.Enums
{
    public static class Descripion
    {
        /// <summary>
        /// 获取枚举的描述信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this object value)
        {
            if (value == null)
                return string.Empty;

            Type type = value.GetType();
            var fieldInfo = type.GetField(Enum.GetName(type, value));
            if (fieldInfo != null)
            {
                if (Attribute.IsDefined(fieldInfo, typeof(DescriptionAttribute)))
                {
                    var description = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (description != null)
                        return description.Description;
                }
            }
            return string.Empty;
        }
    }
}