using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ZJH.BaseTools.DB.Extend;

namespace ZJH.BaseTools.DB
{
    public static class DBConvert
    {
        /// <summary>
        /// 将首行转换为Dictionary对象
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        [Obsolete("改为使用IDataReader的扩展函数ToDictionary")]
        public static Dictionary<string, object> IDataReader_to_Dict(IDataReader reader,bool Replace = false)
        {
            return reader.ToDictionary();
        }
        /// <summary>
        /// 将所有记录转换为Dictionary数组
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        [Obsolete("改为使用IDataReader的扩展函数ToDictionaryList")]
        public static List<Dictionary<string, object>> IDataReader_to_DictList(IDataReader reader)
        {
            return reader.ToDictionaryList();
        }
        /// <summary>
        /// 将首行转换为JSON对象
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        [Obsolete("改为使用IDataReader的扩展函数ToJSON")]
        public static string IDataReader_to_JsonObject(IDataReader reader) {
            return reader.ToJSON(true);
        }
        /// <summary>
        /// 将所有记录转换为JSON数组
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        [Obsolete("改为使用IDataReader的扩展函数ToJSON")]
        public static string IDataReader_to_JsonArray(IDataReader reader)
        {
            return reader.ToJSON();
        }
    }
}
