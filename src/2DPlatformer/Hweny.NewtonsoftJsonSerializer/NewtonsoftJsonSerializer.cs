using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace Hweny.ObjectSerializer
{
    /// <summary>
    /// Newtonsoft json 序列化类，提供对象的序列化与反序列化
    /// </summary>
    public class NewtonsoftJsonSerializer : ObjectSerializer
    {
        public NewtonsoftJsonSerializer() { }

        public void SerializeObject(string fileName, object obj)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException("fileName is null!");
            }
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
            {
                SerializeObject(fs, obj);
                fs.Close();
            }
        }

        public void SerializeObject(System.IO.Stream sw, object obj)
        {
            if (sw == null)
            {
                throw new ArgumentNullException("output stream is null!");
            }
            if (obj == null)
            {
                throw new ArgumentNullException("object is null!");
            }
            string json = JsonConvert.SerializeObject(obj, Formatting.None);
            if (string.IsNullOrWhiteSpace(json))
            {
                throw new JsonException("serialize error!");
            }
            using (var swriter = new StreamWriter(sw))
            {
                swriter.WriteLine(json);
                swriter.Close();
            }
        }

        public object DeserializeObject<T>(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException(fileName+" is not found!");
            }
            object obj=default(T);
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                obj = DeserializeObject<T>(fs);
                fs.Close();
            }
            return obj;
        }

        public object DeserializeObject<T>(System.IO.Stream sr)
        {
            if (sr == null)
            {
                throw new ArgumentNullException("input stream is null!");
            }
            try
            {
                using (var sreader = new StreamReader(sr))
                {
                    string json = sreader.ReadToEnd();
                    object obj = default(T);
                    obj = JsonConvert.DeserializeObject<T>(json);
                    sreader.Close();
                    return obj;
                }
            }
            catch (Exception e) { throw e; }
        }
    }
}
