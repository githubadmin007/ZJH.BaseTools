using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ZJH.BaseTools.IO
{
    public class PathHelper
    {
        /// <summary>
        /// 连接两个路径，支持解析相对路径
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <returns></returns>
        public static string Combine(string path1, string path2) {
            path1 = Path.GetDirectoryName(path1);
            while (path2.StartsWith(".")) {
                if (path2.StartsWith("../") || path2.StartsWith("..\\"))
                {
                    path1 = Directory.GetParent(path1).ToString();
                    if (path2.StartsWith("../"))
                    {
                        path2 = path2.TrimStart("../".ToCharArray());
                    }
                    if (path2.StartsWith("..\\"))
                    {
                        path2 = path2.TrimStart("..\\".ToCharArray());
                    }
                }
                else if (path2.StartsWith("./"))
                {
                    path2 = path2.TrimStart("./".ToCharArray());
                }
                else if (path2.StartsWith(".\\")) {
                    path2 = path2.TrimStart(".\\".ToCharArray());
                }
            }
            return Path.Combine(path1, path2);
        }
    }
}
