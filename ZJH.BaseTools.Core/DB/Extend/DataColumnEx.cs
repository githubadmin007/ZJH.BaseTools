using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using ZJH.BaseTools.BasicExtend;

namespace ZJH.BaseTools.DB.Extend
{
    public static class DataColumnEx
    {
        /// <summary>
        /// 将JToken转为适合该字段的格式
        /// </summary>
        /// <param name="column"></param>
        /// <param name="valueObj"></param>
        /// <returns></returns>
        public static object Convert(this DataColumn column, JToken valueObj) {
            if (valueObj == null || valueObj.ToString() == "") return null;
            switch (column.DataType.FullName)
            {
                case "System.DateTime":
                    // 如果是数字认为是时间戳，否则认为是时间字符串
                    long milliseconds = valueObj.ToInt64();
                    if (milliseconds > 0)
                    {
                        DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)); // 当地时区
                        return startTime.AddMilliseconds(milliseconds);
                    }
                    else
                    {
                        return DateTime.Parse(valueObj.ToString());
                    }
                case "System.Decimal":
                case "System.double":
                    return valueObj.ToDouble();
                case "System.int":
                case "System.Int32":
                    return valueObj.ToInt32();
                case "System.Int16":
                    return valueObj.ToInt16();
                case "System.Int64":
                    return valueObj.ToInt64();
                default:
                    return valueObj.ToString();
            }
        }
    }
}
