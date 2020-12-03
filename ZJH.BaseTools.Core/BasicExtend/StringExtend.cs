using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ZJH.BaseTools.BasicExtend
{
    public static class StringExtend
    {
        /// <summary>
        /// 获取字符串的MD5码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToMD5(this string str)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(str);
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
        /// <summary>
        /// 将字符串转为数字,非整数将返回0
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static short ToInt16(this string str)
        {
            short v;
            if (short.TryParse(str, out v))
            {
                return v;
            }
            return 0;
        }
        /// <summary>
        /// 将字符串转为数字,非整数将返回0
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt32(this string str) {
            int v;
            if (int.TryParse(str, out v))
            {
                return v;
            }
            return 0;
        }
        /// <summary>
        /// 将字符串转为数字,非整数将返回0
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long ToInt64(this string str)
        {
            long v;
            if (long.TryParse(str, out v))
            {
                return v;
            }
            return 0;
        }
        /// <summary>
        /// 将字符串转为数字,非数字将返回0
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double ToDouble(this string str)
        {
            double v;
            if (double.TryParse(str, out v))
            {
                return v;
            }
            return 0.0;
        }
        /// <summary>
        /// 将字符串转为枚举值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string str, Enum defaultVaule = null) where T: Enum // C＃7.3以上才支持Enum类作为约束
        {
            try
            {
                return (T)Enum.Parse(typeof(T), str);
            }
            catch
            {
                return (T)defaultVaule;
            }
        }
        /// <summary>
        /// 指示指定的字符串是 null、空还是仅由空白字符组成。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
        /// <summary>
        /// 指示指定的字符串是 null、空还是仅由空白字符组成。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string str) {
            return string.IsNullOrWhiteSpace(str);
        }
    }
}
