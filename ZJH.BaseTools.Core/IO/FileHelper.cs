using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace ZJH.BaseTools.IO
{
    public class FileHelper
    {
        /// <summary>
        /// 读取ftp上的文件
        /// </summary>
        /// <param name="ftpFile">文件路径(注意：一定要用“/”而不是“\”)
        /// 格式：ftp://{user}:{password}@{ip}:{port}/path/filename
        /// 示例：ftp://ftpuser:PDFxz88256234@172.18.18.143:21/2019/07/22/联合测绘成果数据.pdf</param>
        /// <returns></returns>
        public static byte[] getByteFormFtpFile(string ftpFile)
        {
            byte[] filedata = new WebClient().DownloadData(ftpFile);
            return filedata;
        }
        /// <summary>
        /// 获取指定目录下所有文件路径
        /// </summary>
        /// <param name="dir">文件夹</param>
        /// <param name="ext">扩展名(大写,例如：.JPG)</param>
        /// <returns></returns>
        public static List<string> GetFileList(string directory, string[] exts)
        {
            if (Directory.Exists(directory))
            {
                DirectoryInfo dir = new DirectoryInfo(directory);
                return GetFileList(dir, exts);
            }
            return null;
        }
        /// <summary>
        /// 获取指定目录下所有文件路径
        /// </summary>
        /// <param name="dir">文件夹</param>
        /// <param name="ext">扩展名(大写,例如：.JPG)</param>
        /// <returns></returns>
        public static List<string> GetFileList(DirectoryInfo dir,string[] exts)
        {
            List<string> list = new List<string>();
            FileInfo[] files = dir.GetFiles();
            list.AddRange(files.Where(a => exts.Contains(a.Extension.ToUpper())).Select(a => a.Name));
            DirectoryInfo[] dirs = dir.GetDirectories();
            foreach (DirectoryInfo _dir in dirs)
            {
                list.AddRange(GetFileList(_dir, exts).Select(a => _dir.Name + "/" + a));
            }
            return list;
        }
        /// <summary>
        /// 将文件转为Base64字符串
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static string FileToBase64String(string filepath)
        {
            if (File.Exists(filepath))
            {
                using (FileStream filestream = new FileStream(filepath, FileMode.Open))
                {
                    return StreamToBase64String(filestream);
                }
            }
            throw new Exception("文件不存在");
        }
        /// <summary>
        /// 将文件流转为Base64字符串
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string StreamToBase64String(Stream stream)
        {
            if (stream != null)
            {
                byte[] bt = new byte[stream.Length];
                stream.Read(bt, 0, bt.Length);
                return Convert.ToBase64String(bt);
            }
            throw new Exception("文件流为null");
        }
        /// <summary>
        /// 保存Base64字符串为文件
        /// </summary>
        /// <param name="base64">base64</param>
        /// <param name="path">文件路径</param>
        /// <param name="overwrite">是否覆盖</param>
        /// <returns></returns>
        public static bool SaveFileFromBase64(string base64, string path, bool overwrite = false) {
            try
            {
                //检查文件是否已存在
                if (File.Exists(path)) {
                    if (overwrite)
                    {
                        File.Delete(path);
                    }
                    else {
                        throw new Exception("文件已存在");
                    }
                }
                //检查文件夹并创建
                DirectoryInfo dirInfo = new FileInfo(path).Directory;
                if (!dirInfo.Exists) {
                    dirInfo.Create();
                }
                //保存文件
                using (FileStream filestream = new FileStream(path, FileMode.CreateNew, FileAccess.Write)) {
                    byte[] bt = Convert.FromBase64String(base64);
                    filestream.Write(bt, 0, bt.Length);
                    filestream.Flush();
                    return true;
                }
            }
            catch (Exception ex) {
                Logger.log("SaveFileFromBase64", ex);
            }
            return false;
        }
        /// <summary>
        /// 保存Base64字符串为Stream
        /// </summary>
        /// <param name="base64">base64</param>
        /// <param name="path">文件路径</param>
        /// <param name="overwrite">是否覆盖</param>
        /// <returns></returns>
        public static Stream GetStreamFromBase64(string base64) {
            byte[] bt = Convert.FromBase64String(base64);
            Stream stream = new MemoryStream(bt);
            return stream;
        }
    }
}
