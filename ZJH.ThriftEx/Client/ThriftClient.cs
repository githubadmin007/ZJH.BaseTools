using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thrift.Protocol;
using Thrift.Transport;
using ZJH.BaseTools.IO;
using ZJH.BaseTools.Text;

namespace ZJH.ThriftEx
{
    public class ThriftClient
    {
        string Delimiter = Guid.NewGuid().ToString();//创建guid作为分隔符，保证与参数内容不一致
        TTransport transport;
        CallFuncThrift.Client client;
        int OpenNum = 0;
        public ThriftClient(string host,int port,int timeout)
        {
            transport = new TSocket(host, port, timeout);
            TProtocol protocal = new TBinaryProtocol(transport);
            client = new CallFuncThrift.Client(protocal);
            Logger.log("创建ThriftClient", $"host:{host},port:{port},timeout:{timeout}");
        }
        /// <summary>
        /// 调用函数
        /// </summary>
        /// <param name="FuncName">函数名</param>
        /// <param name="paramArr">参数列表</param>
        /// <returns></returns>
        public Result CallFunc(string FuncName, params string[] paramArr) {
            try
            {
                if (OpenNum == 0) {
                    transport.Open();
                }
                OpenNum++;
                string Params = paramArr == null || paramArr.Length == 0 ? null : string.Join(Delimiter, paramArr);
                string result = client.CallFunc(FuncName, Params, Delimiter);
                return Result.Parse(result);
            }
            catch (Exception ex)
            {
                Logger.log("ThriftClient.CallFunc", ex);
                return new Result(ResultCode.Defeat, ex.Message);
            }
            finally {
                OpenNum--;
                if (OpenNum == 0)
                {
                    transport.Close();
                }
            }
        }
    }
}
