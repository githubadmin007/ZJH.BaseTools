using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace ZJH.BaseTools.IO
{
    public class Logger
    {
        //todo:增加一个配置文件，控制文本、邮件、短信等功能的开关
        static bool saveText
        {
            get
            {
                return true;
            }
        }
        static bool sendEmail
        {
            get
            {
                return false;
            }
        }
        static bool sendMessage
        {
            get
            {
                return false;
            }
        }
        static bool alertAble
        {
            get
            {
                return true;
            }
        }

        const int DEBUGGER = 0x1;
        const int RELEASE = 0x2;
        /// <summary>
        /// 默认的输出模式
        /// </summary>
        public static LogMode defauldMode = LogMode.allTxt;
        public static void log(string title, Exception ex)
        {
            log(title, defauldMode, ex.Message);
        }
        public static void log(string title, LogMode mode, Exception ex)
        {
            log(title, mode, ex.Message);
        }
        public static void log(string title, params string[] contents) {
            log(title, defauldMode, contents);
        }
        public static void log(string title, LogMode mode, params string[] contents)
        {
            bool needLog = false;
#if DEBUG
            needLog = ((int)mode & (int)LogBase.Debugger) > 0;
#elif RELEASE
            needLog = ((int)mode & (int)LogBase.Release) > 0;
#endif
            if (needLog)
            {
                if (((int)mode & (int)LogBase.Txt) > 0 && saveText)
                {
                    logTxt(title, contents);
                }
                if (((int)mode & (int)LogBase.Email) > 0 && sendEmail)
                {
                    //Todo:
                }
                if (((int)mode & (int)LogBase.Message) > 0 && sendMessage)
                {
                    //Todo:
                }
                if (((int)mode & (int)LogBase.Alert) > 0 && alertAble)
                {
#if FW4_0
                    System.Windows.Forms.MessageBox.Show(string.Join("\n", contents), title);
#endif       
                }
            }
        }
        //输出txt
        static void logTxt(string title, string[] contents)
        {
            string dirName = GlobalConfig.AppCfg.getValueByPath("configuration/LogPath");
            if (string.IsNullOrEmpty(dirName)) {
                dirName = new DirectoryInfo(GlobalConfig.BasePath).Parent.FullName + "/ZjhLog";
            }
            string fileName = dirName + "/" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            StreamWriter write;
            while (true) {
                try {
                    //检测文件夹是否存在
                    if (!Directory.Exists(dirName))
                    {
                        Directory.CreateDirectory(dirName);
                    }
                    //检查文件是否存在
                    if (!File.Exists(fileName))
                    {
                        File.Create(fileName).Close();
                    }
                    write = File.AppendText(fileName);
                    break;
                }
                catch
                {
                    Thread.Sleep(200);
                }
            }
            try
            {
                write.WriteLine(string.Format("【{0}】@{1}", title, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                if (contents != null) {
                    foreach (string content in contents)
                    {
                        write.WriteLine(content);
                    }
                }
                write.WriteLine();
            }
            finally {
                write.Flush();
                write.Close();
            }
        }
    }
    enum LogBase
    {
        /// <summary>
        /// 调试时输出日志
        /// </summary>
        Debugger = 1,
        /// <summary>
        /// 发布版本输出日志
        /// </summary>
        Release = 2,
        /// <summary>
        /// 输出txt
        /// </summary>
        Txt = 4,
        /// <summary>
        /// 发送Email
        /// </summary>
        Email = 8,
        /// <summary>
        /// 发送短信
        /// </summary>
        Message = 16,
        /// <summary>
        /// 弹窗提示
        /// </summary>
        Alert = 32
    }
    public enum LogMode
    {
        /// <summary>
        /// 不输出日志
        /// </summary>
        None = 0x0,//
        /// <summary>
        /// 调试时输出文本
        /// </summary>
        dTxt = 0x5,//101
        /// <summary>
        /// 任何模式下都输出文本
        /// </summary>
        allTxt = 0x7,//111
        /// <summary>
        /// 调试时输出文本、发送邮件
        /// </summary>
        dTxtEmail = 0xD,//1101
        /// <summary>
        /// 调试时输出文本、发送邮件、发送短信
        /// </summary>
        dTxtEmailMsg = 0x1D,//11101
        /// <summary>
        /// 任何模式下都输出文本,并弹窗提示
        /// </summary>
        allTxtAlert = 0x27,//100111
        /// <summary>
        /// 占位
        /// </summary>
        a = -0x1
    }
}
