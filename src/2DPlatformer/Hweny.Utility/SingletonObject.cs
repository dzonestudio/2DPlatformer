using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Hweny.Utility
{
    /// <summary>
    /// 单例对象基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SingletonObject<T> where T : class
    {
        protected SingletonObject()
        {
            //
        }

        public static T GetInstance()
        {
            return SingletonObjectCreator.Instance;
        }

        class SingletonObjectCreator
        {
            public static readonly T Instance = CreateInstance();
            static SingletonObjectCreator() { }
            static T CreateInstance()
            {
                try
                {
                    ConstructorInfo cor = typeof(T).GetConstructor(
                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static,
                        Type.DefaultBinder, Type.EmptyTypes, null);
                    if (cor != null)
                    {
                        return cor.Invoke(null) as T;
                    }
                    return null;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}
