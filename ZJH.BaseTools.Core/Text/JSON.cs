using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJH.BaseTools.Text
{
    public class JSON
    {
        #region  Dictionary
        /// <summary>
        /// 将Dictionary转成JSON对象的字符串，每一个KeyValuePair对应一个属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dict">Dictionary</param>
        /// <returns></returns>
        public static string Dictionary_toObject<T>(Dictionary<string, T> dict)
        {
            string[] pairStrArr = dict.Select(delegate (KeyValuePair<string, T> pair)
            {
                string str = "\"" + pair.Key + "\":";
                if (pair.Value is string)
                {
                    str += "\"" + pair.Value + "\"";
                }
                else {
                    str += pair.Value;
                }
                return str;
            }).ToArray();
            string json = "{" + string.Join(",", pairStrArr) + "}";
            return json;
        }
        /// <summary>
        /// 将Dictionary转成JSON数组的字符串，每一个KeyValuePair对应一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dict">Dictionary</param>
        /// <param name="KeyName">对象中Key对应的属性名称</param>
        /// <param name="ValueName">对象中value对应的属性名称</param>
        /// <returns></returns>
        public static string Dictionary_toArray<T>(Dictionary<string, T> dict, string KeyName = "key", string ValueName = "value")
        {
            string[] pairStrArr = dict.Select(delegate (KeyValuePair<string, T> pair)
            {
                string str = "{\"" + KeyName + "\":\"" + pair.Key + "\",\"" + ValueName + "\":";
                if (pair.Value is string)
                {
                    str += "\"" + pair.Value + "\"";
                }
                else {
                    str += pair.Value;
                }
                str += "}";
                return str;
            }).ToArray();
            string json = "[" + string.Join(",", pairStrArr) + "]";
            return json;
        }
        #endregion
    }
}
