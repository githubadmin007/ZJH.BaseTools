using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Teigha.DatabaseServices;
using ZJH.BaseTools.IO;

namespace ZJH.TeighaCAD.Manager
{
    public class LayerManager
    {
        public DWGHelper dwgHelper { get; }
        public Database database { get; }
        public LayerTable LayerTbl { get; }
        public LayerManager(DWGHelper dwgHelper) {
            this.dwgHelper = dwgHelper;
            database = dwgHelper.database;
            LayerTbl = database.LayerTableId.GetObject(OpenMode.ForRead) as LayerTable;
        }

        /// <summary>
        /// 是否存在图层
        /// </summary>
        /// <param name="LayerName"></param>
        /// <returns></returns>
        public bool HasLayer(string LayerName)
        {
            return LayerTbl.Has(LayerName);
        }
        /// <summary>
        /// 通过图层名获取图层Id
        /// </summary>
        /// <param name="LayerName"></param>
        /// <returns></returns>
        public ObjectId GetLayerIdByName(string LayerName)
        {
            if (HasLayer(LayerName))
            {
                foreach (ObjectId id in LayerTbl)
                {
                    LayerTableRecord l = id.GetObject(OpenMode.ForRead) as LayerTableRecord;
                    if (l.Name == LayerName)
                    {
                        return id;
                    }
                }
            }
            return ObjectId.Null;
        }
        /// <summary>
        /// 图层名与图层ID对照关系
        /// </summary>
        Dictionary<string, ObjectId> LayerIdDict = new Dictionary<string, ObjectId>();
        /// <summary>
        /// 查找同名图层并返回图层Id（如果找不到，则克隆图层）
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public ObjectId FindOrCloneLayer(ObjectId LayerId) {
            LayerTableRecord layer = LayerId.GetObject(OpenMode.ForRead) as LayerTableRecord;
            string layername = layer.Name;
            if (LayerIdDict.Keys.Contains(layername)) return LayerIdDict[layername];
            ObjectId NewLayerId = GetLayerIdByName(layername);
            if (NewLayerId.IsNull) {
                NewLayerId = CreateLayerByClone(LayerId);
            }
            LayerIdDict.Add(layername, NewLayerId);
            return NewLayerId;
        }
        /// <summary>
        /// 通过图层Id来克隆图层
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ObjectId CreateLayerByClone(ObjectId LayerId, string name = "") {
            try
            {
                LayerTableRecord layer = LayerId.GetObject(OpenMode.ForRead) as LayerTableRecord;
                using (Transaction tran = database.TransactionManager.StartTransaction())
                {
                    LayerTableRecord newLayer = new LayerTableRecord();
                    newLayer.Name = name == "" ? layer.Name : name;//如果没有指定图层名，则设为被复制的图层名
                    newLayer.Color = layer.Color;
                    newLayer.IsFrozen = layer.IsFrozen;//是否冻结
                    newLayer.IsHidden = layer.IsHidden;//是否隐藏
                    newLayer.IsLocked = layer.IsLocked;//是否锁定
                    newLayer.Transparency = layer.Transparency;//透明度
                    LayerTbl.UpgradeOpen();//提升权限
                    ObjectId NewLayerId = LayerTbl.Add(newLayer);
                    tran.AddNewlyCreatedDBObject(newLayer, true);
                    LayerTbl.DowngradeOpen();//降低权限
                    tran.Commit();
                    return NewLayerId;
                }
            }
            catch (Exception ex)
            {
                Logger.log("CreateLayerByClone", ex.Message);
            }
            return ObjectId.Null;
        }


        /// <summary>
        /// 图层状态快照
        /// </summary>
        List<LayerState> LayerStateSnapshot = null;
        /// <summary>
        /// 保存图层状态快照
        /// </summary>
        public void SaveLayerStateSnapshot() {
            LayerStateSnapshot = GetLayerStates();
        }
        /// <summary>
        /// 恢复图层状态快照
        /// </summary>
        public void RecoveryLayerStateSnapshot() {
            SetLayerStates(LayerStateSnapshot);
        }

        /// <summary>
        /// 获取图层状态
        /// </summary>
        /// <returns></returns>
        public List<LayerState> GetLayerStates() {
            List<LayerState> LayerStateList = new List<LayerState>();
            foreach (ObjectId id in LayerTbl)
            {
                LayerStateList.Add(new LayerState(id));
            }
            return LayerStateList;
        }
        /// <summary>
        /// 设置图层状态
        /// </summary>
        /// <param name="LayerStateList"></param>
        /// <returns></returns>
        public bool SetLayerStates(List<LayerState> LayerStateList) {
            try
            {
                using (Transaction tran = database.TransactionManager.StartTransaction()) {
                    foreach (LayerState State in LayerStateList) {
                        LayerTableRecord Layer = State.LayerId.GetObject(OpenMode.ForWrite) as LayerTableRecord;
                        Layer.IsLocked = State.IsLocked;
                        Layer.IsHidden = State.IsHidden;
                        Layer.IsFrozen = State.IsFrozen;
                        Layer.DowngradeOpen();
                    }
                    tran.Commit();
                }
            }
            catch (Exception ex) {
                Logger.log("SetLayerStates", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 激活图层（取消冻结、锁定、隐藏）
        /// </summary>
        /// <param name="LayerName"></param>
        /// <returns></returns>
        public bool ActivationLayer(string LayerName = "") {
            try
            {
                using (Transaction tran = database.TransactionManager.StartTransaction())
                {
                    foreach (ObjectId LayerId in LayerTbl)
                    {
                        LayerTableRecord Layer = LayerId.GetObject(OpenMode.ForWrite) as LayerTableRecord;
                        if(string.IsNullOrWhiteSpace(LayerName) ||  Layer.Name== LayerName){
                            Layer.IsLocked = false;
                            Layer.IsHidden = false;
                            Layer.IsFrozen = false;
                        }
                        Layer.DowngradeOpen();
                    }
                    tran.Commit();
                }
            }
            catch (Exception ex)
            {
                Logger.log("ActivationLayer", ex.Message);
            }
            return false;
        }
    }

    public class LayerState{
        public string Name;
        public ObjectId LayerId;
        public bool IsLocked;
        public bool IsHidden;
        public bool IsFrozen;
        public LayerState(ObjectId LayerId) {
            this.LayerId = LayerId;
            LayerTableRecord Layer = LayerId.GetObject(OpenMode.ForRead) as LayerTableRecord;
            Name = Layer.Name;
            IsLocked = Layer.IsLocked;
            IsHidden = Layer.IsHidden;
            IsFrozen = Layer.IsFrozen;
        }
    }
}
