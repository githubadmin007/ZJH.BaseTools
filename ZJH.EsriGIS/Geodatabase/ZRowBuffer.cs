using ESRI.ArcGIS.Geodatabase;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ZJH.EsriGIS.Geodatabase
{
    public class ZRowBuffer: IDisposable
    {
        IRowBuffer buffer = null;
        public ZRowBuffer(IRowBuffer buffer)
        {
            if (buffer == null)
            {
                throw new Exception("传入空的IRowBuffer");
            }
            this.buffer = buffer;
        }
        [Obsolete("似乎不释放也可以")]
        public void Dispose()
        {
            if (buffer != null)
            {
                Marshal.ReleaseComObject(buffer);
            }
        }
        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void SetValue(int index, object value)
        {
            if (index > -1 && index < buffer.Fields.FieldCount)
            {
                buffer.Value[index] = value;
            }
            else {
                throw new Exception("ZRowBuffer.SetValue索引越界");
            }
        }
        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetValue(string name, object value)
        {
            int index = buffer.Fields.FindField(name);
            if (index > -1) {
                SetValue(index, value);
            }
        }
        /// <summary>
        /// 传入Json设置属性值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetValues(string json)
        {
            if (!string.IsNullOrWhiteSpace(json))
            {
                JObject attrs = JObject.Parse(json);
                foreach (var pair in attrs)
                {
                    SetValue(pair.Key, pair.Value);
                }
            }
        }
        /// <summary>
        /// 传入另一个对象设置属性值
        /// </summary>
        /// <param name="zRowBuffer"></param>
        public void SetValues(ZRowBuffer zRowBuffer)
        {
            for (int i = 0; i < buffer.Fields.FieldCount; i++)
            {
                if (buffer.Fields.Field[i].Editable) {
                    string name = buffer.Fields.Field[i].Name;
                    object obj = zRowBuffer.GetValue(name);
                    SetValue(name, obj);
                }
            }
        }
        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object GetValue(int index)
        {
            if (index > -1 && index < buffer.Fields.FieldCount)
            {
                return buffer.Value[index];
            }
            else {
                throw new Exception("ZFeatureBuffer.SetValue索引越界");
            }
        }
        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        public object GetValue(string name)
        {
            int index = buffer.Fields.FindField(name);
            if (index > -1) {
                return GetValue(index);
            }
            return null;
        }
        /// <summary>
        /// 获取多个属性值
        /// </summary>
        /// <param name="nameArr">字段名数组</param>
        /// <returns></returns>
        public object[] GetValues(string[] nameArr)
        {
            return nameArr.Select(n => GetValue(n)).ToArray();
        }
    }
}
