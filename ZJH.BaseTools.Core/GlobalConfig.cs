using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZJH.BaseTools.IO;

namespace ZJH.BaseTools
{
    public class GlobalConfig
    {
        /// <summary>
        /// DLL所在目录
        /// </summary>
        static string _BasePath = "";
        public static string BasePath{
            get {
                if (string.IsNullOrWhiteSpace(_BasePath)) {
                    string CodeBase = Assembly.GetExecutingAssembly().CodeBase;
                    string dllPath = CodeBase.Replace("file:", "");
                    if (CodeBase.Contains(':'))
                    {
                        //包含“:”,Window系统
                        dllPath = dllPath.Replace("///", "").Replace("\\\\\\", "");
                    }
                    else {
                        //不包含“:”,Linux系统
                        dllPath = dllPath.Replace("//", "").Replace("\\\\", "");
                    }
                    _BasePath = dllPath.Replace("ZJH.BaseTools.dll", "").Replace("ZJH.BaseTools.DLL", "");
                }
                return _BasePath;
            }
        }
        /// <summary>
        /// 配置文件名，最好在程序启动时设置好。不设置则使用默认配置
        /// </summary>
        public static string CfgFileName = "ZJH.BaseTools.config";
        /// <summary>
        /// 配置文件
        /// </summary>
        static XmlReader _AppCfg = null;
        public static XmlReader AppCfg
        {
            get
            {
                if (_AppCfg == null || _AppCfg.XmlPath != BasePath + CfgFileName)
                {
                    _AppCfg = new XmlReader(BasePath + CfgFileName);
                }
                return _AppCfg;
            }
        }
    }
}
