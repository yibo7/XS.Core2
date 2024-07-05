using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XS.Core2
{
    #region 第1种单例实现，如果你的类没有继承其他类，这种方法更简单
    /// <summary>
    /// 通用实现类的但例模式，要使用的类继承此类即可
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : class, new()
    {
        private static T _instance;
        private static readonly object syslock = new object();

        public static T GetInstance()
        {
            if (_instance == null)
            {
                lock (syslock)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                    }
                }
            }
            return _instance;
        }
    }
    #endregion

    #region 第2种方式单例实现，如果你有类继承了其他类，无法使用第1种，可以使用这种简化方式
    public interface ISingleton { }
    /// <summary>
    /// 调用方式:
    /// 1.public class ChatHistoryBll : LiteDataBase<ChatHistory>, ISingleton
    /// 2.var chatHistoryBll = SingletonProvider.GetInstance<ChatHistoryBll>();
    /// </summary>
    public static class SingletonProvider
    {
        private static readonly ConcurrentDictionary<Type, object> _instances = new ConcurrentDictionary<Type, object>();

        public static T GetInstance<T>() where T : class, ISingleton
        {
            return _instances.GetOrAdd(typeof(T), _ => CreateInstance<T>()) as T;
        }

        private static T CreateInstance<T>() where T : class, ISingleton
        {
            var constructor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, Type.EmptyTypes, null);
            if (constructor == null)
            {
                throw new InvalidOperationException($"No suitable constructor found for {typeof(T)}");
            }
            return constructor.Invoke(null) as T;
        }
    }
    #endregion


}
