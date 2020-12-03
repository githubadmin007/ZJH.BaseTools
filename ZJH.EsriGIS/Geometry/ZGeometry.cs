using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ZJH.EsriGIS.Geometry
{
    public class ZGeometry : IDisposable
    {
        public IGeometry geometry = null;
        public ZGeometry(IGeometry geo) {
            geometry = geo;
        }
        [Obsolete("似乎不释放也可以")]
        public void Dispose()
        {
            if (geometry != null)
            {
                Marshal.ReleaseComObject(geometry);
            }
        }

        public double CenterX {
            get {
                return (geometry.Envelope.XMax + geometry.Envelope.XMin) / 2;
            }
        }
        public double CenterY
        {
            get
            {
                return (geometry.Envelope.YMax + geometry.Envelope.YMin) / 2;
            }
        }
    }
}
