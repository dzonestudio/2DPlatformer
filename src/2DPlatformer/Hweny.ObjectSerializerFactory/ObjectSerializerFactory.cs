using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace Hweny.ObjectSerializer
{
    /// <summary>
    /// 对象序列化器工厂类
    /// </summary>
    public class ObjectSerializerFactory
    {
        /// <summary>
        /// 创建具体的对象序列化器
        /// </summary>
        /// <param name="typeName">对象序列化类型名</param>
        /// <returns></returns>
        public static ObjectSerializer Create(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
            {
                throw new ArgumentNullException("typeName is null!");
            }
            ObjectSerializer obj = null;
            try
            {
                string assemblyName = "Hweny" + "." + typeName;
                string typeFullName = "Hweny.ObjectSerializer" + "." + typeName;
                obj = (ObjectSerializer)Assembly.Load(assemblyName).CreateInstance(typeFullName);
            }
            catch (Exception e) { throw e; }
            return obj;
        }
    }
}
