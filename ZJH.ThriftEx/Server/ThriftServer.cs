using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Thrift.Server;
using Thrift.Transport;
using ZJH.BaseTools.IO;
using ZJH.ThriftEx;

namespace ZJH.ThriftEx.Server
{
    public class ThriftServer<T>
    {
        /// <summary>
        /// 服务所使用的端口号
        /// </summary>
        public int Port { get; }
        /// <summary>
        /// 服务是否已启动
        /// </summary>
        public bool IsOpen { get; protected set; }
        // Thread thread = null;
        protected TServer server = null;
        public ThriftServer(int port) {
            Port = port;
        }


        /// <summary>
        /// 启动Thrift服务
        /// </summary>
        public void StartServe() {
            server.Serve();
            IsOpen = true;
            Logger.log("启动Thrift服务");
        }
        /// <summary>
        /// 停止Thrift服务
        /// </summary>
        public void StopServe() {
            server.Stop();
            IsOpen = false;
            Logger.log("停止Thrift服务");
        }
    }
}