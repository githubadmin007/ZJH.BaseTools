using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ZJH.BaseTools.Text
{
    public class Encrypt
    {
        public static string MD5Encrypt(string text)
        {
            return MD5Encrypt(text, Encoding.UTF8);
        }
        public static string MD5Encrypt(string text, Encoding encode)
        {
            byte[] result = encode.GetBytes(text);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            return BitConverter.ToString(output).Replace("-", "");
        }
    }
}
