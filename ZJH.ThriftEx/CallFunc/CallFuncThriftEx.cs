using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZJH.BaseTools.IO;
using ZJH.BaseTools.Text;

namespace ZJH.ThriftEx
{
    public class CallFuncThriftEx<T> : CallFuncThrift.Iface
    {
        public string CallFunc(string FuncName, string Params, string Delimiter)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FuncName)) {
                    throw new Exception("函数名不能为空");
                }
                string[] paramLst = Params == null ? new string[] { } : Params.Split(new string[] { Delimiter }, StringSplitOptions.None);
                // 输出日志
                Logger.log($"CallFunc:{FuncName}", paramLst);
                // 调用函数
                Type tp = typeof(T);
                MethodInfo method = tp.GetMethod(FuncName);
                object obj = Activator.CreateInstance(tp);
                Result resultObj = (Result)method.Invoke(obj, paramLst);
                return resultObj.ToJson();
            }
            catch (Exception ex) {
                Logger.log("CallFuncThriftEx.CallFunc", ex.Message);
                throw new Exception("", ex);
            }
        }
    }
}
