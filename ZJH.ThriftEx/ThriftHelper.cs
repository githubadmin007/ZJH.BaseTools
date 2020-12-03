using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ZJH.BaseTools;
using ZJH.ThriftEx;
using ZJH.ThriftEx.Server;
using static ZJH.BaseTools.IO.XmlReader;

namespace ZJH.ThriftEx
{
    public interface HandleClass { };
    public class ThriftHelper<T> where T : HandleClass
    {
        /// <summary>
        /// 创建服务端（T为继承了HandleClass接口的类，此类所有可供客户端调用的方法返回值都应该为BaseResult类型）
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static ThriftServer<T> CreateSocketServer(int port) {
            ThriftServer<T> server = new SocketServer<T>(port);
            return server;
        }
        /// <summary>
        /// 读取配置创建服务端
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ThriftServer<T> CreateSocketServer(string name)
        {
            List<ThriftConfig> lst = GlobalConfig.AppCfg.getListByPath<ThriftConfig>("configuration/thrift/ConnConfigs/ConnConfig");
            ThriftConfig cfg = lst.Find(l => l.name == name);
            if (cfg != null)
            {
                return CreateSocketServer(cfg.port);
            }
            return null;
        }


    }

    public class ThriftHelper {
        /// <summary>
        /// 创建客户端
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static ThriftClient CreateClient(string host, int port, int timeout)
        {
            ThriftClient client = new ThriftClient(host, port, timeout);
            return client;
        }
        /// <summary>
        /// 读取配置创建客户端
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ThriftClient CreateClient(string name)
        {
            List<ThriftConfig> lst = GlobalConfig.AppCfg.getListByPath<ThriftConfig>("configuration/thrift/ConnConfigs/ConnConfig");
            ThriftConfig cfg = lst.Find(l => l.name == name);
            if (cfg != null)
            {
                return CreateClient(cfg.host, cfg.port, cfg.timeout);
            }
            return null;
        }
    }


    /// <summary>
    /// thrift配置信息
    /// </summary>
    class ThriftConfig : IXmlNode
    {
        public string name;
        public string host;
        public int port;
        public int timeout;
        public void ParseXmlNode(XmlNode node)
        {
            name = node.Attributes["name"].Value;
            host = node.Attributes["host"].Value;
            port = int.Parse(node.Attributes["port"].Value);
            timeout = int.Parse(node.Attributes["timeout"].Value);
        }
        public XmlNode ToXmlNode()
        {
            throw new NotImplementedException();
        }
    }
}
