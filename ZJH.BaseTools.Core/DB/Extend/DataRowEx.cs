using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;
using ZJH.BaseTools.IO;

namespace ZJH.BaseTools.DB.Extend
{
    public static class DataRowEx
    {
        /// <summary>
        /// 将json转为对象并赋值
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="DataJSON"></param>
        /// <returns></returns>
        public static bool SetData(this DataRow Row, string DataJSON) {
            try
            {
                JObject data = JObject.Parse(DataJSON);
                foreach (DataColumn column in Row.Table.Columns)
                {
                    if (data.ContainsKey(column.ColumnName))
                    {
                        JToken token = data.GetValue(column.ColumnName);
                        Row[column.ColumnName] = column.Convert(token);
                    }
                }
                return true;
            }
            catch(Exception ex) {
                Logger.log("DataRowEx.SetData", ex);
            }
            return false;
        }
    }
}
