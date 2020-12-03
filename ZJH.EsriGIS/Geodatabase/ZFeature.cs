using ESRI.ArcGIS.Geodatabase;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using ZJH.EsriGIS.Geometry;

namespace ZJH.EsriGIS.Geodatabase
{
    public class ZFeature : ZRowBuffer
    {
        IFeature feature = null;
        ZGeometry _Shape = null;
        public ZFeature(IFeature feature) : base(feature)
        {
            if (feature == null) {
                throw new Exception("传入空的IFeature");
            }
            this.feature = feature;
        }
        [Obsolete("似乎不释放也可以")]
        public void Dispose()
        {
            if (feature != null)
            {
                Marshal.ReleaseComObject(feature);
            }
        }
        /// <summary>
        /// 保存修改
        /// </summary>
        public void Store() {
            feature.Store();
        }
        /// <summary>
        /// 设置/获取图形
        /// </summary>
        /// <param name="zGeo"></param>
        public ZGeometry Shape
        {
            get
            {
                if (_Shape == null)
                {
                    _Shape = new ZGeometry(feature.Shape);
                }
                return _Shape;
            }
            set
            {
                feature.Shape = value.geometry;
            }
        }
        /// <summary>
        /// 设置/获取图形
        /// </summary>
        /// <param name="zGeo"></param>
        public ZGeometry ShapeCopy
        {
            get
            {
                return new ZGeometry(feature.ShapeCopy);
            }
        }
    }
}
