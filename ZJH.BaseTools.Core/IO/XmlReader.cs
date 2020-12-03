using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ZJH.BaseTools.IO
{
    public class XmlReader
    {
        XmlDocument _doc;
        public string XmlPath { get; }
        /// <summary>
        /// 构造函数，初始化XmlDocument
        /// </summary>
        /// <param name="xml"></param>
        public XmlReader(string xml)
        {
            try
            {
                XmlPath = xml;
                _doc = new XmlDocument();
                _doc.Load(XmlPath);
            }
            catch (Exception ex) {
                // 使用Logger时会调用XmlReader读取配置，会形成死循环
                // Logger.log("XmlReader", "初始化时出错，Xml路径为：" + xml, "Exception：" + ex.Message);
            }
        }
        /// <summary>
        /// 按路径获取节点的InnerText
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string getValueByPath(string path)
        {
            string result = "";
            if (path != null && path != "")
            {
                XmlNode node = _doc.SelectSingleNode(path);
                if (node != null)
                {
                    result = node.InnerText;
                }
            }
            return result;
        }
        /// <summary>
        /// 按路径设置节点的InnerText
        /// </summary>
        /// <param name="path"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool setValueByPath(string path, string value)
        {
            if (value == null) return false;
            string[] pathArr = path.Split('/');
            XmlNode curNode = null, subNode = null;
            for (int i = 0; i < pathArr.Length; i++)
            {
                if (i == 0)
                {
                    curNode = _doc.SelectSingleNode(pathArr[i]);
                    if (curNode == null) return false;
                }
                else {
                    subNode = curNode.SelectSingleNode(pathArr[i]);
                    if (subNode == null)
                    {
                        subNode = _doc.CreateNode("element", pathArr[i], "");
                        curNode.AppendChild(subNode);
                    }
                    curNode = subNode;
                    subNode = null;
                }
            }
            curNode.InnerText = value;
            _doc.Save(XmlPath);
            return true;
        }

        /// <summary>
        /// getListByPath所用的泛型的基类
        /// </summary>
        public interface IXmlNode
        {
            void ParseXmlNode(XmlNode node);
            XmlNode ToXmlNode();
        }
        /// <summary>
        /// 按路径获取节点列表，并以List<T>的形式返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public List<T> getListByPath<T>(string path) where T : IXmlNode, new()
        {
            List<T> lst = new List<T>();
            if (path != null && path != "")
            {
                XmlNodeList nodeLst = _doc.SelectNodes(path);
                if (nodeLst != null)
                {
                    foreach (XmlNode node in nodeLst)
                    {
                        T item = new T();
                        item.ParseXmlNode(node);
                        lst.Add(item);
                    }
                }
            }
            return lst;
        }
    }
}
