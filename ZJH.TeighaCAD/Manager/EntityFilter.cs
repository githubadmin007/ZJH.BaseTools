using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Teigha.DatabaseServices;
using ZJH.TeighaCAD.Extend;

namespace ZJH.TeighaCAD.Manager
{
    public class EntityFilter
    {
        string RegAppName = "";
        public List<string> Layers = new List<string>();
        public List<string> Types = new List<string>();
        public Dictionary<string, string> Attrs = new Dictionary<string, string>();
        public Entity SpatialEntity = null;
        double xmin = 0, ymin = 0, xmax = 0, ymax = 0;


        public EntityFilter() {

        }

        public void AddLayer(string name) {
            if (!Layers.Contains(name)) {
                Layers.Add(name);
            }
        }
        public void AddAttr(string key,string val) {
            if (!Attrs.Keys.Contains(key)) {
                Attrs.Add(key, val);
            }
        }
        public void AddType(string type) {
            if (!Types.Contains(type))
            {
                Types.Add(type);
            }
        }
        public void SetSpatial(Entity ent) {
            if (ent.GeometricExtents != null && ent.GeometricExtents.MaxPoint.DistanceTo(ent.GeometricExtents.MinPoint)>0) {
                SpatialEntity = ent;
            }
        }
        public void SetSpatial(ObjectId id)
        {
            Entity ent = id.GetObject(OpenMode.ForRead) as Entity;
            SetSpatial(ent);
        }
        public void SetExtent(double xmin, double ymin, double xmax, double ymax) {
            this.xmin = xmin;
            this.ymin = ymin;
            this.xmax = xmax;
            this.ymax = ymax;
        }
        public bool Check(Entity ent) {
            if (ent == null) {
                return false;
            }
            //图层是否符合
            if (Layers.Count > 0 && !Layers.Contains(ent.Layer))
            {
                return false;
            }
            //实体类型是否符合
            if (Types.Count > 0 && !Types.Contains(ent.GetType().Name))
            {
                return false;
            }
            //属性查询
            if (Attrs.Keys.Count > 0) {
                foreach (KeyValuePair<string, string> pair in Attrs) {
                    if (ent.GetXData(RegAppName, pair.Key) != pair.Value) {
                        return false;
                    }
                }
            }
            // 范围查询
            if (xmin != 0 && ymin != 0 && xmax != 0 && ymax != 0) {
                if (!InExtent(ent))
                {
                    return false;
                }
            }
            //空间查询
            if (SpatialEntity != null) {
                if (!IsIntersects(SpatialEntity, ent)) {
                    return false;
                }
            }
            return true;
        }

        bool InExtent(Entity ent) {
            try
            {
                Extents3d entent = ent.GeometricExtents;
                if (
                    entent.MinPoint.X > xmax ||
                    entent.MinPoint.Y > ymax ||
                    entent.MaxPoint.X < xmin ||
                    entent.MaxPoint.Y < ymin
                )
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                //throw new Exception($"type:【{ent.GetType().ToString()}】,error:【{ex.Message}】");
                return false;
            }
        }

        bool IsIntersects(Entity outEnt,Entity inEnt) {


            return true;
        }
    }
}
