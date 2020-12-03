using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Teigha.DatabaseServices;
using ZJH.BaseTools.IO;

namespace ZJH.TeighaCAD.Manager
{
    public class RegAppManager
    {
        public DWGHelper dwgHelper { get; }
        public Database database { get; }
        public RegAppTable RegAppTbl { get; }
        public RegAppManager(DWGHelper dwgHelper)
        {
            this.dwgHelper = dwgHelper;
            database = dwgHelper.database;
            RegAppTbl = database.RegAppTableId.GetObject(OpenMode.ForRead) as RegAppTable;
        }


        /// <summary>
        /// 是否存在应用程序
        /// </summary>
        /// <param name="RegAppName"></param>
        /// <returns></returns>
        public bool HasRegApp(string RegAppName)
        {
            return RegAppTbl.Has(RegAppName);
        }
        /// <summary>
        /// 注册新的应用程序名
        /// </summary>
        /// <param name="db"></param>
        /// <param name="regAppName"></param>
        public bool AddRegAppName(string regAppName)
        {
            using (Transaction trans = database.TransactionManager.StartTransaction())
            {
                try
                {
                    if (!HasRegApp(regAppName))
                    {
                        RegAppTbl.UpgradeOpen();//提升权限
                        RegAppTableRecord regRecord = new RegAppTableRecord() {
                            Name = regAppName
                        };
                        RegAppTbl.Add(regRecord);
                        trans.AddNewlyCreatedDBObject(regRecord, true);
                        RegAppTbl.DowngradeOpen();//降低权限
                    }
                    trans.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Abort();
                    Logger.log("AddRegAppName", ex.Message);
                }
                return false;
            }
        }
    }
}
