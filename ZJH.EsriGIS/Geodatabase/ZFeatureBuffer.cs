using ESRI.ArcGIS.Geodatabase;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ZJH.BaseTools.BasicExtend;
using ZJH.EsriGIS.Geometry;

namespace ZJH.EsriGIS.Geodatabase
{
    public class ZFeatureBuffer : ZRowBuffer
    {
        public IFeatureBuffer buffer;
        ZGeometry _Shape = null;
        public ZFeatureBuffer(IFeatureBuffer buffer) : base(buffer)
        {
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
        /// 通过json更新图形
        /// </summary>
        /// <param name="geometryJson"></param>
        public void SetShapeByJson(JObject geometry) {
            try
            {
                ZGeometry zGeo = ZGeometryHelper.Parse(geometry.ToString());
                buffer.Shape = zGeo.geometry;
                _Shape = new ZGeometry(buffer.Shape);
            }
            catch (Exception e)
            {
                buffer.Shape = null;
                _Shape = null;
            }
        }
        /// <summary>
        /// 通过json设置属性
        /// </summary>
        /// <param name="attributes"></param>
        public void SetAttrsByJson(JObject attrs) {
            for (int i = 0; i < buffer.Fields.FieldCount; i++)
            {
                if (buffer.Fields.Field[i].Editable) {
                    string name = buffer.Fields.Field[i].Name;
                    if (name == "SHAPE" || name == "SHAPE_Area" || name == "SHAPE_Length" || name == "OBJECTID" || name == "GLOBALID" || name == "FID") continue;
                    object val = JTokenToValue(attrs.GetValue(name), buffer.Fields.Field[i]);
                    buffer.Value[i] = val;
                }
            }
        }
        object JTokenToValue(JToken valueObj, IField field)
        {
            if (valueObj == null || valueObj.ToString() == "") return null;
            switch (field.Type)
            {
                case esriFieldType.esriFieldTypeDate:
                    DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)); // 当地时区
                    return startTime.AddMilliseconds(valueObj.ToInt64());
                case esriFieldType.esriFieldTypeDouble:
                    return valueObj.ToDouble();
                case esriFieldType.esriFieldTypeInteger:
                    return valueObj.ToInt32();
                case esriFieldType.esriFieldTypeOID:
                    return valueObj.ToInt32();
                case esriFieldType.esriFieldTypeSingle:
                    return valueObj.ToInt16();
                case esriFieldType.esriFieldTypeSmallInteger:
                    return valueObj.ToInt16();
                case esriFieldType.esriFieldTypeString:
                    string str = valueObj.ToString();//.Replace("\n     ", "").Replace("\n    ", "").Replace("\n ", "");
                    if (str.Length > field.Length) {
                        str = str.Substring(0, field.Length);
                    }
                    return str;
                case esriFieldType.esriFieldTypeBlob:
                case esriFieldType.esriFieldTypeGeometry:
                case esriFieldType.esriFieldTypeGlobalID:
                case esriFieldType.esriFieldTypeGUID:
                case esriFieldType.esriFieldTypeRaster:
                case esriFieldType.esriFieldTypeXML:
                default:
                    return valueObj.ToString();
            }
        }

        /// <summary>
        /// 设置/获取图形
        /// </summary>
        /// <param name="zGeo"></param>
        public ZGeometry Shape {
            get {
                if (_Shape == null) {
                    _Shape= new ZGeometry(buffer.Shape);
                }
                return _Shape;
            }
            set {
                _Shape = value;
                buffer.Shape = value == null ? null : value.geometry;
            }
        }
    }
}
