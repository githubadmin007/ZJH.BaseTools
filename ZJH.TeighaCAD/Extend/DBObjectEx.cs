using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Teigha.DatabaseServices;
using ZJH.BaseTools.IO;

namespace ZJH.TeighaCAD.Extend
{
    public static class DBObjectEx
    {
        /// <summary>
        /// 删除指定注册应用程序下的扩展数据
        /// </summary>
        /// <param name="id">对象的Id</param>
        /// <param name="regAppName">注册应用程序名</param>
        public static void RemoveXData(this DBObject obj, string regAppName)
        {
            Database db = obj.Database;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    obj.UpgradeOpen();
                    TypedValueList XData = obj.GetXDataForApplication(regAppName);
                    if (XData != null)// 如果有扩展数据
                    {
                        TypedValueList newXData = new TypedValueList();
                        newXData.Add(DxfCode.ExtendedDataRegAppName, regAppName);
                        obj.XData = newXData; //为对象的XData属性重新赋值，从而删除扩展数据 
                    }
                    obj.DowngradeOpen();
                }
                catch (Exception ex)
                {
                    trans.Abort();
                    Logger.log("RemoveXData", ex.Message);
                }
            }
        }
        /// <summary>
        /// 新增或更新XData键值对
        /// </summary>
        /// <param name="id"></param>
        /// <param name="regAppName"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetXData(this DBObject obj, string regAppName, string key, string value)
        {
            Database db = obj.Database;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    obj.UpgradeOpen();
                    TypedValueList XData = obj.GetXDataForApplication(regAppName);
                    if (XData == null)
                    {
                        XData = new TypedValueList();
                        XData.Add(DxfCode.ExtendedDataRegAppName, regAppName);
                    }
                    TypedValue tvKey = XData.FirstOrDefault(item => item.Value.Equals(key));//查找key
                    int indexKey = XData.IndexOf(tvKey);//Key索引
                    int indexValue = indexKey + 1;//Value索引
                    if (indexKey >= 0 && indexValue < XData.Count)
                    {
                        //键值对已存在，更新
                        TypedValue tvValue = XData[indexValue];
                        XData[indexValue] = new TypedValue(tvValue.TypeCode, value);
                    }
                    else {
                        //键值对不存在，新增
                        XData.Add(new TypedValue((int)DxfCode.ExtendedDataAsciiString, key));
                        XData.Add(new TypedValue((int)DxfCode.ExtendedDataAsciiString, value));
                    }
                    obj.XData = XData; // 覆盖原来的扩展数据，达到修改的目的
                    obj.DowngradeOpen();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Abort();
                    Logger.log("SetXData", ex.Message);
                }
            }
        }
        /// <summary>
        /// 获取XData值
        /// </summary>
        /// <param name="id"></param>
        /// <param name="regAppName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetXData(this DBObject obj, string regAppName, string key)
        {
            Database db = obj.Database;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    TypedValueList XData = obj.GetXDataForApplication(regAppName);
                    if (XData != null)
                    {
                        TypedValue tvKey = XData.FirstOrDefault(item => item.Value.Equals(key));//查找key
                        int indexKey = XData.IndexOf(tvKey);//Key索引
                        int indexValue = indexKey + 1;//Value索引
                        if (indexKey >= 0 && indexValue < XData.Count)
                        {
                            //键值对已存在,返回
                            object value = XData[indexValue].Value;
                            return value == null ? null : value.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.log("GetXData", ex.Message);
                }
            }
            return null;
        }
    }
}
