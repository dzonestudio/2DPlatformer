using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Hweny.ObjectSerializer
{
    /// <summary>
    /// 对象序列化接口
    /// </summary>
    public interface ObjectSerializer
    {
        /// <summary>
        /// 序列化指定对象
        /// </summary>
        /// <param name="fileName">序列化文件保存路径</param>
        /// <param name="obj">需要序列化的对象</param>
        void SerializeObject(string fileName, object obj);
        /// <summary>
        /// 序列化指定对象
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="obj"></param>
        void SerializeObject(Stream sw, object obj);
        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName">序列化文件路径</param>
        /// <returns></returns>
        object DeserializeObject<T>(string fileName);
        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sr"></param>
        /// <returns></returns>
        object DeserializeObject<T>(Stream sr);
    }
}
