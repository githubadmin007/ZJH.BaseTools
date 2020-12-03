using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZJH.BaseTools.BasicExtend
{
    public static class DictionaryExtend
    {
        /// <summary>
        /// 将字典转为对象（使用JSON作为中介）
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="dict">字典</param>
        /// <returns></returns>
        public static T ToObject<T>(this IDictionary<string, object> dict)
        {
            var json = JsonConvert.SerializeObject(dict);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
