using System;
using System.Collections.Generic;
using System.Reflection;
using ZJH.BaseTools.IO;

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
        public static T ToObject<T>(this IDictionary<string, object> dict) where T : new()
        {
            try {
                //var json = JsonConvert.SerializeObject(dict);
                //return JsonConvert.DeserializeObject<T>(json);
                T obj = Activator.CreateInstance<T>();
                dict.SetValueToObj(obj);
                return obj;
            }
            catch(Exception ex) {
                Logger.log("DictionaryExtend.ToObject", ex);
            }
            return default;
        }
        /// <summary>
        /// 将字典的值赋给对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dict"></param>
        /// <param name="obj"></param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns></returns>
        public static bool SetValueToObj(this IDictionary<string, object> dict, object obj, bool ignoreCase = true)
        {
            try {
                Type type = obj.GetType();
                foreach (var pair in dict) {
                    BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public;
                    if (ignoreCase) {
                        bindingAttr |= BindingFlags.IgnoreCase;
                    }
                    PropertyInfo property = type.GetProperty(pair.Key, bindingAttr);
                    if (property != null) {
                        property.SetValue(obj, pair.Value, null);
                    }
                }
                return true;
            }
            catch(Exception ex) {
                Logger.log("DictionaryExtend.SetValueToObj", ex);
            }
            return false;
        }
    }
}
