using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ZJH.BaseTools.BasicExtend;

namespace ZJH.BaseTools.DB.Extend
{
    public static class IDataReaderEx
    {
        /// <summary>
        /// 将所有字段名用“,”连接起来
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="map">字段映射关系,为空时使用原字段名称</param>
        /// <returns></returns>
        public static string JoinAllName(this IDataReader reader, Dictionary<string, string> map = null) {
            List<string> fieldnames = new List<string>();
            for (var i = 0; i < reader.FieldCount; i++) {
                string name = reader.GetName(i);
                if (map != null) {
                    if (map.Keys.Contains(name)) {
                        name = map[name];
                    }
                }
                fieldnames.Add(name);
            }
            return fieldnames.Join(",");
        }



        /// <summary>
        /// 遍历
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="callback"></param>
        public static void ForEach(this IDataReader reader, Action<IDataReader,int> callback) {
            int index = 0;
            while (reader.Read()) {
                callback(reader, index++);
            }
        }
        /// <summary>
        /// 遍历某个字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="fieldName">字段名</param>
        /// <param name="nullValue">当值为DBNull时返回的默认值</param>
        /// <param name="callback"></param>
        public static void ForEach<T>(this IDataReader reader, string fieldName, object nullValue, Action<T, int> callback)
        {
            int index = 0;
            while (reader.Read())
            {
                object obj = reader[fieldName];
                if (obj == DBNull.Value)
                {
                    obj = nullValue;
                }
                callback((T)obj, index++);
            }
        }


        /// <summary>
        /// 读取一行数据，并转为Dictionary对象
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ToDictionary(this IDataReader reader)
        {
            if (reader.Read())
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    string colname = reader.GetName(i);
                    object value = reader.GetValue(i);
                    string typename = reader.GetFieldType(i).Name;
                    switch (typename)
                    {
                        case "String":
                            value = value.ToString();
                            break;
                        case "Decimal":
                            if (value is DBNull)
                            {
                                value = 0;
                            }
                            break;
                    }
                    result.Add(colname, value);
                }
                return result;
            }
            return null;
        }
        /// <summary>
        /// 将所有记录转换为Dictionary数组
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> ToDictionaryList(this IDataReader reader)
        {
            var results = new List<Dictionary<string, object>>();
            Dictionary<string, object> dict;
            while (null != (dict = reader.ToDictionary()))
            {
                results.Add(dict);
            }
            return results;
        }
        /// <summary>
        /// 读取数据，并转为JSON字符串
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="FirstRowOnly">设为true只读取一行会返回一个对象，否则返回数组（无论有几条记录）</param>
        /// <returns></returns>
        public static string ToJSON(this IDataReader reader, bool FirstRowOnly = false)
        {
            return FirstRowOnly ?
                reader.ToDictionary().ToJSON("{}") :
                reader.ToDictionaryList().ToJSON("[]");
        }


        /// <summary>
        ///  读取一行数据，并解析为指定类型的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T Read<T>(this IDataReader reader) where T : IDataRow, new() {
            if (reader.Read()) {
                T obj = new T();
                obj.Parse(reader);
                return obj;
            }
            return default(T);
        }
        /// <summary>
        /// 读取所有数据，并解析为指定类型的对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<T> ReadAll<T>(this IDataReader reader) where T : IDataRow, new()
        {
            List<T> lst = new List<T>();
            T item;
            while (null != (item = reader.Read<T>()))
            {
                lst.Add(item);
            }
            return lst;
        }
    }


    /// <summary>
    /// getListByPath所用的泛型的基类
    /// </summary>
    public interface IDataRow
    {
        void Parse(IDataReader reader);
    }
}
