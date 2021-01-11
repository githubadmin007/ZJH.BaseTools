using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ZJH.BaseTools.Net
{
    public class ZWebClient
    {
        static public Encoding DefaultEncoding = Encoding.UTF8;
        static public string DownloadString(string url, Encoding encoding = null) {
            encoding = encoding ?? DefaultEncoding;
            WebClient client = new WebClient();
            byte[] b = client.DownloadData(url);
            return encoding.GetString(b);
        }
    }
}
