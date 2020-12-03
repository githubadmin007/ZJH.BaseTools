using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Thrift.Protocol;
using Thrift.Transport;

namespace ZJH.ThriftEx.Server
{
    // 未完成
    class HTTPServer<T> : ThriftServer<T>
    {
        public HTTPServer(int port) : base(port)
        {
            CallFuncThriftEx<T> handler = new CallFuncThriftEx<T>();
            CallFuncThrift.Processor processor = new CallFuncThrift.Processor(handler);


            string serviceUrl = "http://localhost:99/";

            TProtocolFactory protocolFactory = new TJSONProtocol.Factory();
            THttpHandler httpServer = new THttpHandler(processor, protocolFactory);

            HttpListener httpListener = new HttpListener();
            httpListener.Prefixes.Add(serviceUrl);
            httpListener.Start();

            //Logger.log("创建ThriftServer", $"端口号:{port}");
        }
    }
}
