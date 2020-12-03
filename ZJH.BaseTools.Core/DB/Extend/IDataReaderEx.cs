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
    }
}
