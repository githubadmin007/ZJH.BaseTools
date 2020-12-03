using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZJH.BaseTools.Text;

namespace ZJH.BaseTools.BasicExtend
{
    public static class ObjectExtend
    {
        /// <summary>
        /// 将字符串转为数字,非整数将返回0
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static short ToInt16(this object obj)
        {
            if (obj == null)
            {
                return 0;
            }
            return obj.ToString().ToInt16();
        }
        /// <summary>
        /// 将字符串转为数字,非整数将返回0
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ToInt32(this object obj)
        {
            if (obj == null)
            {
                return 0;
            }
            return obj.ToString().ToInt32();
        }
        /// <summary>
        /// 将字符串转为数字,非整数将返回0
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static long ToInt64(this object obj)
        {
            if (obj == null)
            {
                return 0;
            }
            return obj.ToString().ToInt64();
        }
        /// <summary>
        /// 将字符串转为数字,非数字将返回0
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double ToDouble(this object obj)
        {
            if (obj == null)
            {
                return 0;
            }
            return obj.ToString().ToDouble();
        }
        /// <summary>
        /// 将对象转为字符串，对象为null将返回defaultValue
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="defaultValue">对象为null时的默认值</param>
        /// <returns></returns>
        public static string ToString(this object obj, string nullValue = "")
        {
            if (obj == null)
            {
                return nullValue;
            }
            return obj.ToString();
        }
        /// <summary>
        /// 将对象转为JSON字符串，对象为null时将返回nullValue
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="nullValue">对象为null时的默认值</param>
        /// <returns></returns>
        public static string ToJSON(this object obj, string nullValue = "{}") {
            if (obj == null) {
                return nullValue;
            }
            return JsonConvert.SerializeObject(obj);
        }
    }
}
