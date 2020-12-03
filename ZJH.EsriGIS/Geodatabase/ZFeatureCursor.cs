using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ZJH.EsriGIS.Geodatabase
{
    public class ZFeatureCursor : IDisposable
    {
        IFeatureCursor cursor = null;
        public ZFeatureCursor(IFeatureCursor c) {
            cursor = c;
        }
        public void Dispose()
        {
            if (cursor != null)
            {
                Marshal.ReleaseComObject(cursor);
            }
        }

        /// <summary>
        /// 将缓冲区的数据存入
        /// </summary>
        public void Flush() {
            cursor.Flush();
        }
        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="zbuffer"></param>
        public void InsertFeature(ZFeatureBuffer zbuffer) {
            cursor.InsertFeature(zbuffer.buffer);
        }
        /// <summary>
        /// 获取下一条记录
        /// </summary>
        /// <returns></returns>
        public ZFeature NextFeature() {
            IFeature feature = cursor.NextFeature();
            return feature == null ? null : new ZFeature(feature);
        }
    }
}
