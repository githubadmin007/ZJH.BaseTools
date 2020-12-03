using ESRI.ArcGIS.Geodatabase;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ZJH.BaseTools.IO;
using ZJH.EsriGIS.Enum;
using ZJH.EsriGIS.Geometry;

namespace ZJH.EsriGIS.Geodatabase
{
    public class ZFeatureClass : IDisposable
    {
        IFeatureClass featureClass = null;
        public ZFields zFields = null;
        public ZFeatureClass(IFeatureClass fc)
        {
            if (fc == null)
            {
                throw new Exception("IFeatureClass不能为空");
            }
            featureClass = fc;
            zFields = new ZFields(fc);
        }
        public void Dispose()
        {
            if (featureClass != null)
            {
                Marshal.ReleaseComObject(featureClass);
            }
        }

        public FeatureType FeatureType
        {
            get {
                return (FeatureType)featureClass.FeatureType;
            }
        }
        public string ShapeFieldName {
            get {
                return featureClass.ShapeFieldName;
            }
        }

        /// <summary>
        /// 获取所有的字段名
        /// </summary>
        /// <returns></returns>
        public List<string> getAllFieldName()
        {
            List<string> lst = new List<string>();
            IFields fields = featureClass.Fields;
            for (int i = 0; i < fields.FieldCount; i++)
            {
                lst.Add(fields.Field[i].Name);
            }
            return lst;
        }
        /// <summary>
        /// 获取所有的字段别名
        /// </summary>
        /// <returns></returns>
        public List<string> getAllFieldAliasName()
        {
            List<string> lst = new List<string>();
            IFields fields = featureClass.Fields;
            for (int i = 0; i < fields.FieldCount; i++)
            {
                string name = fields.Field[i].AliasName;
                lst.Add(string.IsNullOrWhiteSpace(name) ? fields.Field[i].Name : name);
            }
            return lst;
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="name">字段名</param>
        /// <param name="alias">字段别名</param>
        /// <param name="type">字段类型</param>
        /// <param name="length">长度</param>
        /// <param name="precision">精度</param>
        /// <param name="nullable">是否可为空</param>
        /// <param name="editable">是否可编辑</param>
        /// <param name="defaultvalue">默认值</param>
        public void AddField(string name, string alias, Enum.FieldType type, int length = 0, int precision = 0, bool nullable = true, bool editable = true, object defaultvalue = null)
        {
            zFields.AddField(name, alias, type, length, precision, nullable, editable, defaultvalue);
        }
        /// <summary>
        /// 重命名
        /// </summary>
        /// <param name="newName"></param>
        public void ReName(string newName) {
            IDataset ds = featureClass as IDataset;
            if (ds.CanRename()) {
                ds.Rename(newName);
            }
        }


        /// <summary>
        /// 是否存在记录
        /// </summary>
        /// <param name="where"></param>
        /// <param name="zGeo"></param>
        /// <param name="rel"></param>
        /// <returns></returns>
        public bool HasRecord(string where = "1=1", ZGeometry zGeo = null, SpatialRelEnum rel = SpatialRelEnum.esriSpatialRelIntersects)
        {
            ZFilter filter = new ZFilter(where, zGeo, featureClass.ShapeFieldName, rel);
            return HasRecord(filter);
        }
        /// <summary>
        /// 是否存在记录
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public bool HasRecord(ZFilter filter)
        {
            using (ZFeatureCursor cursor = Search(filter))
            {
                return cursor.NextFeature() != null;
            }
        }
        /// <summary>
        /// The number of features selected by the specified query.
        /// </summary>
        /// <param name="where"></param>
        /// <param name="zGeo"></param>
        /// <param name="rel"></param>
        /// <returns></returns>
        public int RecordCount(string where = "1=1", ZGeometry zGeo = null, SpatialRelEnum rel = SpatialRelEnum.esriSpatialRelIntersects) {
            ZFilter filter = new ZFilter(where, zGeo, featureClass.ShapeFieldName, rel);
            return RecordCount(filter);
        }
        /// <summary>
        /// The number of features selected by the specified query.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public int RecordCount(ZFilter filter)
        {
            return featureClass.FeatureCount(filter.Value);
        }

        /// <summary>
        /// 空间属性查询
        /// </summary>
        /// <param name="where"></param>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public ZFeatureCursor Search(string where = "1=1", ZGeometry zGeo = null, SpatialRelEnum rel = SpatialRelEnum.esriSpatialRelIntersects)
        {
            ZFilter filter = new ZFilter(where, zGeo, featureClass.ShapeFieldName, rel);
            return Search(filter);
        }
        /// <summary>
        /// 空间属性查询
        /// </summary>
        /// <param name="where"></param>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public ZFeatureCursor Search(ZFilter filter)
        {
            IFeatureCursor cursor = featureClass.Search(filter.Value, false);
            return new ZFeatureCursor(cursor);
        }
        /// <summary>
        /// Returns a cursor that can be used to update features selected by the specified
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="useBuffering"></param>
        /// <returns></returns>
        public ZFeatureCursor Update(string where = "1=1", ZGeometry zGeo = null, SpatialRelEnum rel = SpatialRelEnum.esriSpatialRelIntersects, bool useBuffering = true)
        {
            ZFilter filter = new ZFilter(where, zGeo, featureClass.ShapeFieldName, rel);
            return Update(filter, useBuffering);
        }
        /// <summary>
        /// Returns a cursor that can be used to update features selected by the specified
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="useBuffering"></param>
        /// <returns></returns>
        public ZFeatureCursor Update(ZFilter filter, bool useBuffering)
        {
            IFeatureCursor cursor = featureClass.Update(filter.Value, useBuffering);
            return new ZFeatureCursor(cursor); 
        }
        /// <summary>
        /// Delete the Rows in the database selected by the specified query.
        /// </summary>
        /// <param name="where"></param>
        /// <param name="zGeo"></param>
        /// <param name="rel"></param>
        public bool Delete(string where = "1=1", ZGeometry zGeo = null, SpatialRelEnum rel = SpatialRelEnum.esriSpatialRelIntersects)
        {
            ZFilter filter = new ZFilter(where, zGeo, featureClass.ShapeFieldName, rel);
            return Delete(filter);
        }
        /// <summary>
        /// Delete the Rows in the database selected by the specified query.
        /// </summary>
        /// <param name="filter"></param>
        public bool Delete(ZFilter filter) {
            try
            {
                ITable table = featureClass as ITable;
                table.DeleteSearchedRows(filter.Value);
                return true;
            }
            catch {
                return false;
            }
        }
        /// <summary>
        /// Create a new feature, with a system assigned object ID and null property values.
        /// </summary>
        /// <returns></returns>
        public ZFeature CreateFeature()
        {
            IFeature feature = featureClass.CreateFeature();
            return new ZFeature(feature);
        }
        /// <summary>
        /// Create a feature buffer that can be used with an insert cursor.
        /// </summary>
        /// <returns></returns>
        public ZFeatureBuffer CreateFeatureBuffer()
        {
            IFeatureBuffer buffer = featureClass.CreateFeatureBuffer();
            return new ZFeatureBuffer(buffer);
        }
        /// <summary>
        /// Returns a cursor that can be used to insert new features.
        /// </summary>
        /// <param name="useBuffering"></param>
        /// <returns></returns>
        public ZFeatureCursor Insert(bool useBuffering)
        {
            IFeatureCursor cursor = featureClass.Insert(useBuffering);
            return new ZFeatureCursor(cursor);
        }
        /// <summary>
        /// 复制一条记录并插入
        /// </summary>
        /// <param name="zFeature"></param>
        public void CopyAndInsert(ZFeature zFeature)
        {
            IFeatureCursor cursor = featureClass.Insert(false);
            IFeature feature = featureClass.CreateFeature();
            feature.Shape = zFeature.ShapeCopy.geometry;
            int n = feature.Fields.FieldCount;
            for (int i = 0; i < n; i++)
            {
                IField field = feature.Fields.Field[i];
                if (field.Editable)
                {
                    feature.Value[i] = zFeature.GetValue(i);
                }
            }
            feature.Store();
        }
        /// <summary>
        /// 从查询结果复制数据
        /// </summary>
        /// <param name="cursor"></param>
        public void CopyFrom(ZFeatureCursor cursor) {
            IFeatureCursor this_cursor = featureClass.Insert(true);
            ZFeature feature = null;
            while (null != (feature = cursor.NextFeature())) {
                IFeatureBuffer buffer = featureClass.CreateFeatureBuffer();
                buffer.Shape = feature.Shape.geometry;
                for (int i = 0; i < featureClass.Fields.FieldCount; i++) {
                    buffer.Value[i] = feature.GetValue(i);
                }
                this_cursor.InsertFeature(buffer);
            }
            this_cursor.Flush();
        }
        /// <summary>
        /// 从另一张表复制数据
        /// </summary>
        /// <param name="ZFClass"></param>
        /// <param name="clear">是否清空原数据</param>
        public void CopyFrom(ZFeatureClass ZFClass, bool clear = false) {
            if (clear) {
                Delete();
            }
            ZFeatureCursor cursor = ZFClass.Search();
            CopyFrom(cursor);
        }
        /// <summary>
        /// 复制数据到另一张表
        /// </summary>
        /// <param name="outZWorkspace">目标工作空间</param>
        /// <param name="name">目标图层名</param>
        /// <param name="filter">查询条件</param>
        public void CopyTo(ZWorkspace outZWorkspace, string name, ZFilter filter) {
            // 数据源
            IFeatureClassName inputFClassName = ((IDataset)featureClass).FullName as IFeatureClassName;
            // 目标数据集
            IDataset outDataSet = outZWorkspace.workspace as IDataset;
            IWorkspaceName outWorkspaceName = outDataSet.FullName as IWorkspaceName;
            // 目标图层
            IFeatureClassName outputFClassName = new FeatureClassNameClass();
            IDatasetName dataSetName = outputFClassName as IDatasetName;
            dataSetName.WorkspaceName = outWorkspaceName;
            dataSetName.Name = name;
            // 导出
            FeatureDataConverterClass converter = new FeatureDataConverterClass();
            converter.ConvertFeatureClass(inputFClassName, filter.Value, null, outputFClassName, null, featureClass.Fields, "", 100, 0);
            // 释放资源
            Marshal.ReleaseComObject(converter);
        }

        /// <summary>
        /// 插入从MapServer获取到的JSON数据
        /// </summary>
        /// <param name="features"></param>
        /// <returns></returns>
        public int InsertFeaturesByJson(JArray features) {
            int featureNum = 0;
            try {
                ZFeatureCursor cursor = Insert(true);
                foreach (JObject feature in features)
                {
                    try
                    {
                        ZFeatureBuffer buffer = CreateFeatureBuffer();
                        buffer.SetShapeByJson(feature.Value<JObject>("geometry"));
                        buffer.SetAttrsByJson(feature.Value<JObject>("attributes"));
                        cursor.InsertFeature(buffer);
                    }
                    catch (Exception ex)
                    {
                        Logger.log("ZFeatureClass.InsertFeaturesByJson", ex);
                    }
                    featureNum++;
                }
                cursor.Flush();
            }
            catch (Exception ex)
            {
                Logger.log("ZFeatureClass.InsertFeaturesByJson", ex);
            }
            return featureNum;
        }

        [Obsolete("未完成，勿用")]
        public bool InsertFeatureByJsion(JObject feature) {
            return false;
        }
    }
}
