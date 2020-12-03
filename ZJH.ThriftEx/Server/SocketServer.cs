using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thrift.Server;
using Thrift.Transport;
using ZJH.BaseTools.IO;

namespace ZJH.ThriftEx.Server
{
    class SocketServer<T> : ThriftServer<T>
    {
        public SocketServer(int port) : base(port)
        {
            CallFuncThriftEx<T> handler = new CallFuncThriftEx<T>();
            CallFuncThrift.Processor processor = new CallFuncThrift.Processor(handler);
            TServerTransport serverTransport = new TServerSocket(port);
            server = new TThreadPoolServer(processor, serverTransport);
            Logger.log("创建ThriftServer", $"端口号:{port}");
        }
    }
}
