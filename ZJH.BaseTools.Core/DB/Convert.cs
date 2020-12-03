using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ZJH.BaseTools.DB
{
    public static class DBConvert
    {
        /// <summary>
        /// 将首行转换为Dictionary对象
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Dictionary<string, object> IDataReader_to_Dict(IDataReader reader,bool Replace = false)
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
                            value = value.ToString();//.Replace("\"", "\\\"").Replace("\n", "").Replace("\t", "").Replace("\r", "");
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
        public static List<Dictionary<string, object>> IDataReader_to_DictList(IDataReader reader)
        {
            var results = new List<Dictionary<string, object>>();
            Dictionary<string, object> dict;
            while (null != (dict = IDataReader_to_Dict(reader)))
            {
                results.Add(dict);
            }
            return results;
        }
        /// <summary>
        /// 将首行转换为JSON对象
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static string IDataReader_to_JsonObject(IDataReader reader) {
            Dictionary<string, object> obj = IDataReader_to_Dict(reader);
            return obj == null ? "{}" : JsonConvert.SerializeObject(obj);
        }
        /// <summary>
        /// 将所有记录转换为JSON数组
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static string IDataReader_to_JsonArray(IDataReader reader)
        {
            List<Dictionary<string, object>> lst = IDataReader_to_DictList(reader);
            return JsonConvert.SerializeObject(lst);
        }
    }
}
