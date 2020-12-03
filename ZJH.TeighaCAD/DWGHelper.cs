using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Teigha.DatabaseServices;
using Teigha.Export_Import;
using Teigha.GraphicsSystem;
using Teigha.Runtime;
using ZJH.BaseTools.IO;
using ZJH.TeighaCAD.Extend;
using ZJH.TeighaCAD.Manager;

namespace ZJH.TeighaCAD
{
    public class DWGHelper
    {
        static Services svcs = new Services();
        /// <summary>
        /// 是否为新建模式
        /// </summary>
        public bool IsNewDWG { get; }
        /// <summary>
        /// DWG文件路径
        /// </summary>
        public string dwgPath { get; }
        /// <summary>
        /// DWG数据库对象
        /// </summary>
        public Database database { get; }
        /// <summary>
        /// 图层管理器
        /// </summary>
        LayerManager _LayerMgr = null;
        public LayerManager LayerMgr {
            get {
                if (_LayerMgr == null) {
                    _LayerMgr = new LayerManager(this);
                }
                return _LayerMgr;
            }
        }
        /// <summary>
        /// 块管理器
        /// </summary>
        BlockManager _BlockMgr = null;
        public BlockManager BlockMgr {
            get {
                if (_BlockMgr == null) {
                    _BlockMgr = new BlockManager(this);
                }
                return _BlockMgr;
            }
        }
        /// <summary>
        /// 文字样式管理器
        /// </summary>
        TextStyleManager _TextStyleMgr = null;
        public TextStyleManager TextStyleMgr
        {
            get
            {
                if (_TextStyleMgr == null)
                {
                    _TextStyleMgr = new TextStyleManager(this);
                }
                return _TextStyleMgr;
            }
        }
        /// <summary>
        /// 应用程序管理器
        /// </summary>
        RegAppManager _RegAppMgr = null;
        public RegAppManager RegAppMgr {
            get {
                if (_RegAppMgr == null)
                {
                    _RegAppMgr = new RegAppManager(this);
                }
                return _RegAppMgr;
            }
        }

        /// <summary>
        /// 在指定路径创建一个dwg文件。并返回DWGHelper
        /// </summary>
        /// <param name="dwgPath"></param>
        /// <returns></returns>
        static public DWGHelper CreateDWGHelper(string dwgPath, Enum.DwgVersion ver = Enum.DwgVersion.AC1024) {
            DWGHelper helper = null;
            if (dwgPath.ToLower().EndsWith(".dwg"))
            {
                if (File.Exists(dwgPath))
                {
                    throw new System.Exception("文件已存在！");
                }
                else {
                    helper = new DWGHelper(dwgPath);
                    helper.SaveAs(dwgPath, (DwgVersion)ver);
                }
            }
            else {
                throw new System.Exception("文件格式错误，只支持dwg格式的文件！");
            }
            return helper;
        }

        /// <summary>
        /// 构造函数（通过dwg路径初始化）
        /// </summary>
        /// <param name="dwgPath">DWG文件路径</param>
        public DWGHelper(string dwgPath)
        {
            if (dwgPath.ToLower().EndsWith(".dwg"))
            {
                this.dwgPath = dwgPath;
                IsNewDWG = !File.Exists(dwgPath);//不存在文件，采用新建模式
                if (IsNewDWG)
                {
                    database = new Database(true, true);
                }
                else {
                    database = new Database(false, false);
                    database.ReadDwgFile(dwgPath, FileShare.ReadWrite, false, null);
                }
            }
            else {
                throw new System.Exception("文件格式错误，只支持dwg格式的文件！");
            }
        }
        /// <summary>
        /// 构造函数（通过Teigha.DatabaseServices.Database对象初始化）
        /// </summary>
        /// <param name="db"></param>
        public DWGHelper(object db) {
            if (db == null)
            {
                throw new System.Exception("Database不能为null！");
            }
            else {
                database = db as Database;
                dwgPath = database.Filename;
                IsNewDWG = !File.Exists(dwgPath);
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        public void Save(Enum.DwgVersion ver = Enum.DwgVersion.AC1024) {
            if (IsNewDWG)
            {
                database.SaveAs(dwgPath, (DwgVersion)ver);
            }
            else {
                database.Save();
            }
        }
        /// <summary>
        /// 另存为
        /// </summary>
        /// <param name="path">保存路径</param>
        /// <param name="version">DWG版本</param>
        public void SaveAs(string path, DwgVersion version) {
            database.SaveAs(path, version);
        }


        public bool MergeDWG(string SourcePath, EntityFilter filter = null) {
            if (File.Exists(SourcePath))
            {
                BlockHelper TargetSpace = BlockMgr.ModelSpace;//目标DWG模型空间
                TargetSpace.StartTransaction();//开启事务
                using (Database SourceDB = new Database(false, false))
                {
                    SourceDB.ReadDwgFile(SourcePath, FileShare.Read, false, null);
                    BlockTableRecord SourceBtr = SourceDB.CurrentSpaceId.GetObject(OpenMode.ForRead) as BlockTableRecord;//来源DWG模型空间
                    foreach (ObjectId ObjId in SourceBtr)
                    {
                        Entity ent = ObjId.GetObject(OpenMode.ForRead) as Entity;
                        if (filter == null || filter.Check(ent)) {
                            TargetSpace.CloneEntity(ent);
                        }
                    }
                }
                TargetSpace.CommitTransaction();//提交事务
                return true;
            }
            else {
                Logger.log("MergeDWG","DWG文件不存在");
            }
            return false;
        }

        /// <summary>
        /// 对dwg中的实体进行坐标转换。注意：此方法会导致Database无法被再次Save();
        /// </summary>
        /// <param name="TransFunc">坐标转换函数</param>
        /// <param name="OutputPath">转换后实体导出路径，为空则保留在原文件</param>
        /// <param name="ErrorPath">错误实体保存路径，为空则不导出错误实体</param>
        /// <returns></returns>
        public bool Transform(TransformHelper.TransformDelegate TransFunc, string OutputPath = "", string ErrorPath = "") {
            LayerMgr.SaveLayerStateSnapshot();//保存图层状态
            LayerMgr.ActivationLayer();//激活所有图层
            TransformHelper.BooleanStatis statis = TransformHelper.TransformDWG(this, TransFunc);
            LayerMgr.RecoveryLayerStateSnapshot();//将图层恢复到激活前的状态
            //保存转换后的实体
            if (string.IsNullOrWhiteSpace(OutputPath))
            {
                //Save();
            }
            else {
                SaveAs(OutputPath, database.OriginalFileVersion);
            }
            //导出错误实体
            if (!string.IsNullOrWhiteSpace(ErrorPath) && statis.FalseCount > 0)
            {
                DWGHelper successHelper= string.IsNullOrWhiteSpace(OutputPath)? this: new DWGHelper(OutputPath);
                File.Copy(string.IsNullOrWhiteSpace(OutputPath) ? this.dwgPath : OutputPath, ErrorPath, true);//复制文件
                DWGHelper errorHelper = new DWGHelper(ErrorPath);
                successHelper.BlockMgr.ModelSpace.DeleteEntityByXData(TransformHelper.TransformStatus, "False");
                successHelper.Save();
                errorHelper.BlockMgr.ModelSpace.DeleteEntityByXData(TransformHelper.TransformStatus, "True");
                errorHelper.Save();
            }
            statis.Log();
            return statis.FalseCount == 0;
        }


        public void ExportPDF() {
            try
            {
                mPDFExportParams param = new mPDFExportParams();
                param.Author = "FSSG";
                param.BackgroundColor = Color.White;
                param.Creator = "FSSG";
                param.Database = database;
                param.Flags = PDFExportFlags.Default;
                param.FlateCompression = true;
                param.imageDPI = 72;
                param.Keywords = "";
                param.Layouts.Add("Layout1");

                StreamBuf stream = new FileStreamBuf(@"C:\Users\Tdme\Desktop\2555.00-718.00.pdf", false, FileShareMode.DenyReadWrite, FileCreationDisposition.CreateAlways);
                param.OutputStream = stream;

                PageParams page = new PageParams();
                page.setParams(200, 200);
                param.PageParams.Add(page);

                param.Palette = new Color[] { Color.Red, Color.Green, Color.Blue };
                param.Producer = "FSSG";
                param.Subject = "FSSG";
                param.Title = "FSSG";
                param.UseHLR = true;
                param.Versions = PDFExportVersions.Last;

                Export_Import.ExportPDF(param);

                ulong i = stream.Length;
                stream.Dispose();
            }
            catch (System.Exception ex) {
                Logger.log("", ex);
            }
        }
    }
}
