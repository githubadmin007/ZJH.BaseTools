using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJH.BaseTools.BasicExtend
{
    public static class ListExtend
    {
        #region 通用
        /// <summary>
        /// 获取唯一值列表（排除列表中的重复值）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="IEnume"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetUniqueValue<T>(this IEnumerable<T> IEnume) {
            List<T> list = IEnume.ToList();
            List<T> tmp = new List<T>();
            for (int i = 0; i < list.Count(); i++)
            {
                if (!tmp.Contains(list[i]) ){
                    tmp.Add(list[i]);
                }
            }
            return tmp;
        }
        #endregion

        #region string
        /// <summary>
        /// 使用指定分隔符连接字符数组
        /// </summary>
        /// <param name="IEnume"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string Join(this IEnumerable<string> IEnume,string separator)
        {
            return string.Join(separator, IEnume.ToArray());
        }
        #endregion
    }
}
