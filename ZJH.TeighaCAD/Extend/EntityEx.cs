using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Teigha.DatabaseServices;
using Teigha.Geometry;
using ZJH.BaseTools.IO;

namespace ZJH.TeighaCAD.Extend
{
    public static class EntityEx
    {
        public static BlockReference Test(this BlockReference oldEnt) {
            try
            {
                //oldEnt.BlockName
            }
            catch (Exception ex)
            {
                Logger.log("BlockReference.NewOneByClone", ex.Message);
            }
            return null;
        }





        public static double[][] ToPointArray(this Polyline ent)
        {
            List<double[]> points = new List<double[]>();
            for (int i = 0; i < ent.NumberOfVertices; i++)
            {
                Point3d p3d = ent.GetPoint3dAt(i);
                double[] point = new double[2] { p3d.X, p3d.Y };
                points.Add(point);
            }
            // 按ESRI的格式，闭合面的首尾点坐标应该一致
            if (ent.Closed) {
                double[] p1 = points[0];
                double[] p2 = points[points.Count - 1];
                if (p1[0] != p2[0] || p1[1] != p2[1]) {
                    points.Add(p1);
                }
            }
            return points.ToArray();
        }
    }
}
