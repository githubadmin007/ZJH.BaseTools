using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Teigha.DatabaseServices;
using ZJH.BaseTools.IO;

namespace ZJH.TeighaCAD.Manager
{
    public class TextStyleManager
    {
        public DWGHelper dwgHelper { get; }
        public Database database { get; }
        public TextStyleTable TextStyleTbl { get; }
        public TextStyleManager(DWGHelper dwgHelper)
        {
            this.dwgHelper = dwgHelper;
            database = dwgHelper.database;
            TextStyleTbl = database.TextStyleTableId.GetObject(OpenMode.ForRead) as TextStyleTable;
        }

        /// <summary>
        /// 是否存在文字样式
        /// </summary>
        /// <param name="TextStyleName"></param>
        /// <returns></returns>
        public bool HasTextStyle(string TextStyleName)
        {
            return TextStyleTbl.Has(TextStyleName);
        }
        /// <summary>
        /// 通过文字样式名获取文字样式Id
        /// </summary>
        /// <param name="TextStyleName"></param>
        /// <returns></returns>
        public ObjectId GetTextStyleIdByName(string TextStyleName)
        {
            if (HasTextStyle(TextStyleName))
            {
                foreach (ObjectId id in TextStyleTbl)
                {
                    LayerTableRecord l = id.GetObject(OpenMode.ForRead) as LayerTableRecord;
                    if (l.Name == TextStyleName)
                    {
                        return id;
                    }
                }
            }
            return ObjectId.Null;
        }
        /// <summary>
        /// 文字样式名与文字样式ID对照关系
        /// </summary>
        Dictionary<string, ObjectId> TextStyleIdDict = new Dictionary<string, ObjectId>();
        /// <summary>
        /// 查找同名文字样式并返回文字样式Id（如果找不到，则克隆文字样式）
        /// </summary>
        /// <param name="TextStyleId"></param>
        /// <returns></returns>
        public ObjectId FindOrCloneTextStyle(ObjectId TextStyleId)
        {
            TextStyleTableRecord TextStyle = TextStyleId.GetObject(OpenMode.ForRead) as TextStyleTableRecord;
            string TextStyleName = TextStyle.Name;
            if (TextStyleIdDict.Keys.Contains(TextStyleName)) return TextStyleIdDict[TextStyleName];
            ObjectId NewTextStyleId = GetTextStyleIdByName(TextStyleName);
            if (NewTextStyleId.IsNull)
            {
                NewTextStyleId = CreateTextStyleByClone(TextStyleId);
            }
            TextStyleIdDict.Add(TextStyleName, NewTextStyleId);
            return NewTextStyleId;
        }
        /// <summary>
        /// 通过文字样式Id来克隆文字样式
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ObjectId CreateTextStyleByClone(ObjectId TextStyleId, string name = "")
        {
            try
            {
                TextStyleTableRecord TextStyle = TextStyleId.GetObject(OpenMode.ForRead) as TextStyleTableRecord;
                using (Transaction tran = database.TransactionManager.StartTransaction())
                {
                    TextStyleTableRecord newTextStyle = new TextStyleTableRecord();
                    newTextStyle.Name = name == "" ? TextStyle.Name : name;//如果没有指定名称，则设为被复制的名称
                    newTextStyle.BigFontFileName = TextStyle.BigFontFileName;
                    newTextStyle.FileName = TextStyle.FileName;
                    newTextStyle.FlagBits = TextStyle.FlagBits;
                    newTextStyle.Font = TextStyle.Font;
                    newTextStyle.IsShapeFile = TextStyle.IsShapeFile;
                    newTextStyle.IsVertical = TextStyle.IsVertical;
                    newTextStyle.ObliquingAngle = TextStyle.ObliquingAngle;
                    newTextStyle.PriorSize = TextStyle.PriorSize;
                    newTextStyle.TextSize = TextStyle.TextSize;
                    newTextStyle.XScale = TextStyle.XScale;
                    TextStyleTbl.UpgradeOpen();//提升权限
                    ObjectId NewTextStyleId = TextStyleTbl.Add(newTextStyle);
                    tran.AddNewlyCreatedDBObject(newTextStyle, true);
                    TextStyleTbl.DowngradeOpen();//降低权限
                    tran.Commit();
                    return NewTextStyleId;
                }
            }
            catch (Exception ex)
            {
                Logger.log("CreateLayerByClone", ex.Message);
            }
            return ObjectId.Null;
        }
    }
}
