using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hweny.Utility
{
    public static class Utility
    {
        /// <summary>
        /// 如果字符串为null或由空字符构成则返回null，否则返回字符串去掉前后空格后的大写形式。
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns></returns>
        public static string Upper(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            return value.Trim().ToUpper();
        }
    }
}
