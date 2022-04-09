using System;
using System.Reflection;

namespace huhu.Commom.Snowflake
{
    /// <summary>
    /// 普通泛型单例模式
    /// 优点：简化单例模式构建,不需要每个单例类单独编写；
    /// 缺点：违背单例模式原则，构造函数无法设置成private，导致将T类的构造函数暴露；
    /// </summary>
    /// <typeparam name="T">class</typeparam>
    [Obsolete("Recommended use ReflectionSingleton")]
    public abstract class Singleton<T> where T : class, new()
    {
        protected static T _Instance = null;

        public static T Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new T();
                }
                return _Instance;
            }
        }

        protected Singleton()
        {
            Init();
        }

        public virtual void Init(){ }
    }

    /// <summary>
    /// 反射实现泛型单例模式【推荐使用】
    /// 优点：1.简化单例模式构建,不需要每个单例类单独编写；2.遵循单例模式构建原则，通过反射去调用私有的构造函数，实现了构造函数不对外暴露；
    /// 缺点：反射方式有一定的性能损耗(可忽略不计);
    /// </summary>
    /// <typeparam name="T">class</typeparam>
    public abstract class ReflectionSingleton<T> where T : class
    {
        private static T _Intance;
        public static T Instance
        {
            get
            {
                if (null == _Intance)
                {
                    _Intance = null;
                    Type type = typeof(T); //1.类型强制转换

                    //2.获取到T的构造函数的类型和参数信息,监测构造函数是私有或者静态，并且构造函数无参，才会进行单例的实现
                    ConstructorInfo[] constructorInfoArray = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                    foreach (ConstructorInfo constructorInfo in constructorInfoArray)
                    {
                        ParameterInfo[] parameterInfoArray = constructorInfo.GetParameters();
                        if (0 == parameterInfoArray.Length)
                        {
                            //检查构造函数无参，构建单例
                            _Intance = (T)constructorInfo.Invoke(null);
                            break;
                        }
                    }

                    if (null == _Intance)
                    {
                        //提示不支持构造函数公有且有参的单例构建
                        throw new NotSupportedException("No NonPublic constructor without 0 parameter");
                    }
                }
                return _Intance;
            }
        }

        protected ReflectionSingleton() { }

        public static void Destroy()
        {
            _Intance = null;
        }
    }
}
