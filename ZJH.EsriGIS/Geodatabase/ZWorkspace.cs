using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ZJH.BaseTools.IO;
using ZJH.EsriGIS.Enum;
using ZJH.EsriGIS.GeodatabaseUI;

namespace ZJH.EsriGIS.Geodatabase
{
    public class ZWorkspace: IDisposable
    {
        public IWorkspace workspace;
        public string PathName;
        public ZWorkspace(IWorkspace space) {
            if (space == null) {
                throw new Exception("传入的IWorkspace为null");
            }
            workspace = space;
            PathName = workspace.PathName;
        }

        public void Dispose()
        {
            if (workspace != null) {
                Marshal.ReleaseComObject(workspace);
            }
        }

        /// <summary>
        /// 获取workspace中的所有FeatureClass名称
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllFeatureClassName() {
            IEnumDatasetName datasetNameEnum = workspace.DatasetNames[esriDatasetType.esriDTAny];
            return GetAllFeatureClassName(datasetNameEnum);
        }
        /// <summary>
        /// 判断FeatureClass是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasFeatureClass(string name) {
            List<string> lst = GetAllFeatureClassName();
            return lst.Contains(name);
        }
        /// <summary>
        /// 打开FeatureClass
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ZFeatureClass OpenFeatureClass(string name) {
            try
            {
                IFeatureWorkspace space = workspace as IFeatureWorkspace;
                IFeatureClass fc = space.OpenFeatureClass(name);
                return new ZFeatureClass(fc);
            }
            catch (Exception ex){
                Logger.log("OpenFeatureClass", ex);
                return null;
            }
        }
        /// <summary>
        /// 创建FeatureClass
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="fieldsHelper"></param>
        /// <param name="iFeatureType"></param>
        /// <param name="ShapeFieldName"></param>
        /// <param name="ConfigKeyword"></param>
        public ZFeatureClass CreateFeatureClass(string Name, ZFields fields, FeatureType type = FeatureType.esriFTSimple, string ShapeFieldName= "SHAPE", string ConfigKeyword = "") {
            try
            {
                IFeatureWorkspace space = workspace as IFeatureWorkspace;
                IFeatureClass fc = space.CreateFeatureClass(Name, fields.Value, null, null, (esriFeatureType)type, ShapeFieldName, ConfigKeyword);
                return new ZFeatureClass(fc);
            }
            catch (Exception ex)
            {
                Logger.log("ZWorkspace.CreateFeatureClass", ex);
                return null;
            }
        }
        /// <summary>
        /// 复制图层
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        /// <param name="copyReords">思否复制记录</param>
        public ZFeatureClass CopyFeatureClass(string oldName, string newName, bool copyReords = false) {
            ZFeatureClass oldFClass = OpenFeatureClass(oldName);
            if (oldFClass != null) {
                ZFields fields = oldFClass.zFields.Clone();
                ZFeatureClass newFClass = CreateFeatureClass(newName, fields, oldFClass.FeatureType, oldFClass.ShapeFieldName);
                if (copyReords)
                {
                    newFClass.CopyFrom(oldFClass);
                }
                return newFClass;
            }
            return null;
        }
        /// <summary>
        /// 删除图层
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool DeleteFeatureClass(string name)
        {
            try
            {
                IEnumDatasetName pEnumDsName = workspace.get_DatasetNames(esriDatasetType.esriDTFeatureClass);
                IDatasetName datasetName = pEnumDsName.Next();
                while (datasetName != null)
                {
                    string[] name_arr = datasetName.Name.Split(new char[] { '.', '/', '\\' });
                    if (name_arr[name_arr.Length - 1].ToUpper() == (name.ToUpper()))
                    {
                        IFeatureWorkspaceManage pFWSM = workspace as IFeatureWorkspaceManage;
                        if (pFWSM.CanDelete((IName)datasetName))
                        {
                            pFWSM.DeleteByName(datasetName); 
                            break;
                        }
                    }
                    datasetName = pEnumDsName.Next();
                }
                return true;
            }
            catch {
                return false;
            }
        }
        /// <summary>
        /// 重命名FeatureClass
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public bool ReName(string oldName,string newName) {
            try
            {
                ZFeatureClass fc = OpenFeatureClass(oldName);
                fc.ReName(newName);
            }
            catch {
                return false;
            }
            return true;
        }




        /// <summary>
        /// 获取工作空间（打开窗口手动选择）
        /// </summary>
        /// <returns></returns>
        public static ZWorkspace GetWorkspace()
        {
            SelectOrCreateWorkspace frm = new SelectOrCreateWorkspace();
            if (DialogResult.OK == frm.ShowDialog())
            {
                return frm.workspaceHelper;
            }
            return null;
        }
        /// <summary>
        /// 获取Access工作空间
        /// </summary>
        /// <param name="mdbPath">Access文件路径</param>
        /// <returns></returns>
        public static ZWorkspace GetAccessWorkspace(string mdbPath)
        {
            if (string.IsNullOrWhiteSpace(mdbPath) || !File.Exists(mdbPath))
            {
                throw new Exception(string.Format("MDB文件不存在,文件路径：“{0}”", mdbPath));
            }
            else {
                try
                {
                    IWorkspaceFactory pWorkspaceFac = new AccessWorkspaceFactory();
                    IWorkspace workspace = pWorkspaceFac.OpenFromFile(mdbPath, 0);
                    return new ZWorkspace(workspace);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("读取MDB文件失败,文件路径：“{0}”", mdbPath), ex);
                }
            }
        }
        /// <summary>
        /// 获取文件地理空间GDB数据库
        /// </summary>
        /// <param name="gdbPath"></param>
        /// <returns></returns>
        public static ZWorkspace GetFileGDBWorkspace(string gdbPath)
        {
            if (string.IsNullOrWhiteSpace(gdbPath) || !Directory.Exists(gdbPath))
            {
                throw new Exception(string.Format("GDB文件不存在,文件路径：“{0}”", gdbPath));
            }
            else {
                try
                {
                    IWorkspaceFactory pWorkspaceFac = new FileGDBWorkspaceFactoryClass();
                    IWorkspace workspace = pWorkspaceFac.OpenFromFile(gdbPath, 0);
                    return new ZWorkspace(workspace);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("读取GDB文件失败,文件路径：“{0}”", gdbPath), ex);
                }
            }
        }
        /// <summary>
        /// 获取SDE数据库工作空间
        /// </summary>
        /// <param name="server">服务器名称或者IP</param>
        /// <param name="instance">数据库实例名</param>
        /// <param name="database">SDE数据库名称</param>
        /// <param name="user">用户名</param>
        /// <param name="password">用户密码</param>
        /// <param name="version">连接版本</param>
        /// <returns></returns>
        public static ZWorkspace GetSdeWorkspace(string server, string instance, string database, string user, string password, string version = "sde.DEFAULT")
        {
            if (string.IsNullOrWhiteSpace(server) || string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("缺少必要参数，无法打开SDE工作空间");
            }
            else {
                //SDE数据库采用直连方式，必须加上“sde:oracle11g:”前缀；并且要连上目标机，必须加上服务器名称或者IP
                if (string.IsNullOrWhiteSpace(instance)) instance = string.IsNullOrWhiteSpace(server) ? "sde:oracle11g:orcl" : string.Format("sde:oracle11g:{0}/orcl", server);//sde:oracle11g:orcl为连接oracle client配置的主机数据库
                else if (!instance.StartsWith("sde:oracle11g:", StringComparison.OrdinalIgnoreCase)) instance = string.IsNullOrWhiteSpace(server) ? "sde:oracle11g:" + instance : string.Format("sde:oracle11g:{0}/{1}", server, instance);
                try
                {
                    //sde数据库连接属性设置
                    IPropertySet pProperty = new PropertySetClass();
                    pProperty.SetProperty("Server", server);//服务器名称或者IP
                    pProperty.SetProperty("Instance", instance);
                    pProperty.SetProperty("Database", database);//sde数据库名称
                    pProperty.SetProperty("User", user);//用户名称
                    pProperty.SetProperty("Password", password);//用户密码
                    pProperty.SetProperty("Version", version);//连接版本
                    return GetSdeWorkspace(pProperty);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("打开SDE数据库失败"), ex);
                }
            }
        }
        /// <summary>
        /// 获取SDE数据库工作空间
        /// </summary>
        /// <param name="propertySet">数据库连接信息</param>
        /// <returns></returns>
        private static ZWorkspace GetSdeWorkspace(IPropertySet propertySet)
        {
            if (propertySet == null)
            {
                throw new Exception(string.Format("参数IPropertySet不能为空"));
            }
            try
            {
                IWorkspaceFactory2 wf = new SdeWorkspaceFactoryClass() as IWorkspaceFactory2;
                IWorkspace workspace = wf.Open(propertySet, 0);
                return new ZWorkspace(workspace);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("打开SDE数据库失败"), ex);
            }
        }
        /// <summary>
        /// 获取SDE数据库工作空间
        /// </summary>
        /// <param name="sdePath">.sde文件</param>
        /// <returns></returns>
        public static ZWorkspace GetSdeWorkspace(string sdePath)
        {
            if (sdePath == "" || !sdePath.EndsWith(".sde") || !File.Exists(sdePath))
            {
                throw new Exception(string.Format("请选择sde文件"));
            }
            try
            {
                IWorkspaceFactory2 wf = new SdeWorkspaceFactoryClass() as IWorkspaceFactory2;
                IWorkspace workspace = wf.OpenFromFile(sdePath, 0);
                return new ZWorkspace(workspace);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("打开SDE数据库失败"), ex);
            }
        }
        /// <summary>
        /// 获取Shapefile数据工作空间
        /// </summary>
        /// <param name="shpPath"></param>
        /// <returns></returns>
        public static ZWorkspace GetShapefileWorkspace(string shpPath)
        {
            if (string.IsNullOrWhiteSpace(shpPath) || !File.Exists(shpPath))
            {
                throw new Exception(string.Format("SHP文件不存在,文件路径：“{0}”", shpPath));
            }
            else {
                try
                {
                    IWorkspaceFactory pWorkspaceFac = new ShapefileWorkspaceFactoryClass();
                    IWorkspace workspace = pWorkspaceFac.OpenFromFile(shpPath, 0);
                    return new ZWorkspace(workspace);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("读取SHP文件失败,文件路径：“{0}”", shpPath), ex);
                }
            }
        }
        /// <summary>
        /// 获取影像数据工作空间
        /// </summary>
        /// <param name="rasterFolder">影像文件所在文件夹</param>
        /// <returns></returns>
        public static ZWorkspace GetRasterWorkspace(string rasterFolder)
        {
            if (string.IsNullOrWhiteSpace(rasterFolder) || !Directory.Exists(rasterFolder))
            {
                throw new Exception(string.Format("文件夹不存在,文件路径：“{0}”", rasterFolder));
            }
            else {
                try
                {
                    IWorkspaceFactory pWorkspaceFac = new RasterWorkspaceFactoryClass();
                    IWorkspace workspace = pWorkspaceFac.OpenFromFile(rasterFolder, 0);
                    return new ZWorkspace(workspace);
                    //IRasterDataset rasterDataset = rasterWorkspace.OpenRasterDataset(rasterFileName);
                    //IRasterLayer rasterLayer = new RasterLayerClass();
                    //rasterLayer.CreateFromDataset(rasterDataset);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("读取文件夹失败,文件夹路径：“{0}”", rasterFolder), ex);
                }
            }
        }
        /// <summary>
        /// 获取CAD数据工作空间
        /// </summary>
        /// <param name="cadPath"></param>
        /// <returns></returns>
        public static ZWorkspace GetCADWorkspcae(string cadPath) {
            if (string.IsNullOrWhiteSpace(cadPath) || !File.Exists(cadPath))
            {
                throw new Exception(string.Format("CAD文件不存在,文件路径：“{0}”", cadPath));
            }
            else {
                try
                {
                    IWorkspaceFactory pWorkspaceFac = new CadWorkspaceFactoryClass();
                    IWorkspace workspace = pWorkspaceFac.OpenFromFile(cadPath, 0);
                    return new ZWorkspace(workspace);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("读取CAD文件失败,文件路径：“{0}”", cadPath), ex);
                }
            }
        }


        /// <summary>
        /// 获取IEnumDataset中的所有FeatureClass名称
        /// </summary>
        /// <param name="datasetsEnum"></param>
        /// <returns></returns>
        public static List<string> GetAllFeatureClassName(IEnumDatasetName datasetsEnumName)
        {
            List<string> lst = new List<string>();
            if (datasetsEnumName == null)
            {
                return lst;
            }
            else {
                IDatasetName datasetName = datasetsEnumName.Next();
                while (datasetName != null)
                {
                    if (datasetName.Type == esriDatasetType.esriDTFeatureClass)
                    {
                        lst.Add(datasetName.Name);
                    }
                    else if (datasetName.Type == esriDatasetType.esriDTFeatureDataset)
                    {
                        lst.AddRange(GetAllFeatureClassName(datasetName.SubsetNames));
                    }
                    datasetName = datasetsEnumName.Next();
                }
            }
            return lst;
        }




        /// <summary>
        /// 创建GDB
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string CreateGDB(string fullPath)
        {
            if (string.IsNullOrWhiteSpace(fullPath)) {
                throw new Exception("路径不能为空");
            }
            return CreateGDB(Path.GetDirectoryName(fullPath), Path.GetFileName(fullPath));
        }
        /// <summary>
        /// 创建GDB
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="gdbname"></param>
        /// <returns></returns>
        public static string CreateGDB(string folder,string gdbname) {
            if (string.IsNullOrWhiteSpace(folder) || string.IsNullOrWhiteSpace(gdbname))
            {
                throw new Exception("路径不能为空");
            }
            if (System.Text.RegularExpressions.Regex.IsMatch(gdbname,"[/\\, ]")) {
                throw new Exception("路径不能含有特殊字符");
            }
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            IWorkspaceFactory2 wsFctry = new FileGDBWorkspaceFactoryClass();
            if (!gdbname.ToLower().EndsWith(".gdb")) gdbname += ".gdb";
            IWorkspaceName wsName = wsFctry.Create(folder, gdbname, null, 0);
            string gdbPath = wsName == null ? "" : wsName.PathName;
            Marshal.ReleaseComObject(wsName);
            Marshal.ReleaseComObject(wsFctry);
            return gdbPath;
        }


    }
}
