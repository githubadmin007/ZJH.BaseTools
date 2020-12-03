using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZJH.EsriGIS.Enum;

namespace ZJH.EsriGIS.Geodatabase
{
    public class ZFields
    {
        public IFields Value {
            get {
                return fields;
            }
        }
        private IFeatureClass featureClass = null;
        private IFields fields = null;
        private IFieldsEdit fieldsEdit = null;
        List<ZField> ZFieldList = new List<ZField>();
        public ZFields() {
            fields = new FieldsClass();
            fieldsEdit = (IFieldsEdit)fields;
            initZFieldList();
        }
        public ZFields(IFeatureClass fc)
        {
            featureClass = fc;
            fields = featureClass.Fields;
            fieldsEdit = (IFieldsEdit)fields;
            initZFieldList();
        }
        /// <summary>
        /// 初始化字段帮助
        /// </summary>
        void initZFieldList() {
            int num = fields.FieldCount;
            for (int i = 0; i < num; i++) {
                ZFieldList.Add(new ZField(fields.Field[i]));
            }
        }
        /// <summary>
        /// 添加字段(新创建的IFields与FeatureClass中的IFields处理方式不同)
        /// </summary>
        /// <param name="fieldHelper"></param>
        public void AddField(ZField fieldHelper) {
            if (featureClass == null)
            {
                fieldsEdit.AddField(fieldHelper.field);
            }
            else {
                featureClass.AddField(fieldHelper.field);
            }
            ZFieldList.Add(fieldHelper);
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
        public void AddField(string name, string alias, Enum.FieldType type, int length = 0, int precision = 0, bool nullable = true, bool editable = true, object dValue = null) {
            ZField zfield = new ZField(name, alias, type, length, precision, nullable, editable, dValue);
            AddField(zfield);
        }
        /// <summary>
        /// 添加图形字段
        /// </summary>
        /// <param name="geoType"></param>
        /// <param name="name"></param>
        public void AddShapeField(GeometryType geoType, string name = "SHAPE")
        {
            AddShapeField(geoType, ZSpatialReference.Unknown, name);
        }
        /// <summary>
        /// 添加图形字段
        /// </summary>
        /// <param name="geoType"></param>
        /// <param name="srHelper"></param>
        /// <param name="name"></param>
        public void AddShapeField(GeometryType geoType, ZSpatialReference sr, string name = "SHAPE") {
            ZField zfield = new ZField(geoType, sr, name);
            AddField(zfield);
        }
        /// <summary>
        /// 复制字段
        /// </summary>
        /// <param name="filter">是否过滤</param>
        /// <returns></returns>
        public ZFields Clone(bool filter = true) {
            List<ZField> _lst = ZFieldList;
            ZFields zFields = new ZFields();
            if (filter) {
                _lst = _lst.Where(delegate (ZField zField)
                {
                    return !zField.Name.Contains(".");
                }).ToList();
            }
            _lst.ForEach(zField => zFields.AddField(zField.Clone()));
            return zFields;
        }
    }



    public class ZField {
        public IField field;
        public string Name {
            get {
                return field == null ? "" : field.Name;
            }
        }
        public ZField(IField field)
        {
            this.field = field;
        }
        /// <summary>
        /// 属性字段
        /// </summary>
        /// <param name="name"></param>
        /// <param name="alias"></param>
        /// <param name="type"></param>
        /// <param name="length"></param>
        /// <param name="precision"></param>
        /// <param name="nullable"></param>
        /// <param name="editable"></param>
        /// <param name="dValue"></param>
        public ZField(string name,string alias, Enum.FieldType type, int length = 0, int precision = 0, bool nullable = true, bool editable = true, object dValue = null) {
            field = new FieldClass();
            IFieldEdit fieldEdit = (IFieldEdit)field;
            fieldEdit.Name_2 = name;
            fieldEdit.AliasName_2 = alias;
            fieldEdit.Type_2 = (esriFieldType)type;
            fieldEdit.IsNullable_2 = nullable;
            fieldEdit.Editable_2 = editable;
            if (length > 0) fieldEdit.Length_2 = length;
            if (precision > 0) fieldEdit.Precision_2 = precision;
            if (dValue != null) fieldEdit.DefaultValue_2 = dValue;
        }
        /// <summary>
        /// 创建图形字段
        /// </summary>
        /// <param name="geoType"></param>
        /// <param name="sr"></param>
        /// <param name="name"></param>
        public ZField(GeometryType geoType, ZSpatialReference sr, string name = "SHAPE") {
            field = new FieldClass();
            IFieldEdit pFieldEdit = (IFieldEdit)field;
            pFieldEdit.Name_2 = name;
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
            //设置图形类型
            IGeometryDefEdit pGeoDef = new GeometryDefClass();
            IGeometryDefEdit pGeoDefEdit = pGeoDef as IGeometryDefEdit;
            pGeoDefEdit.GeometryType_2 = (esriGeometryType)geoType;
            if (sr != null)
            {
                pGeoDefEdit.SpatialReference_2 = sr.Value;
            }
            pFieldEdit.GeometryDef_2 = pGeoDef;
        }
        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public ZField Clone() {
            if (field.Type == esriFieldType.esriFieldTypeGeometry)
            {
                IGeometryDef pGeoDefEdit = field.GeometryDef; 
                ZSpatialReference sr = new ZSpatialReference(pGeoDefEdit.SpatialReference);
                return new ZField((GeometryType)pGeoDefEdit.GeometryType, sr, field.Name);
            }
            else {
                return new ZField(field.Name, field.AliasName, (Enum.FieldType)field.Type, field.Length, field.Precision, field.IsNullable, field.Editable, field.DefaultValue);
            }
        }
    }
}
