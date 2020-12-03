using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Teigha.DatabaseServices;
using ZJH.BaseTools.IO;

namespace ZJH.TeighaCAD.Manager
{
    public class BlockManager
    {
        public DWGHelper dwgHelper { get; }
        public Database database { get; }
        public BlockTable BlockTbl { get; }
        public BlockManager(DWGHelper dwgHelper)
        {
            this.dwgHelper = dwgHelper;
            database = dwgHelper.database;
            BlockTbl = database.BlockTableId.GetObject(OpenMode.ForRead) as BlockTable;
        }
        /// <summary>
        /// 是否存在块
        /// </summary>
        /// <param name="BlockName"></param>
        /// <returns></returns>
        public bool HasBlock(string BlockName)
        {
            return BlockTbl.Has(BlockName);
        }
        /// <summary>
        /// 通过块名获取块Id
        /// </summary>
        /// <param name="BlockName"></param>
        /// <returns></returns>
        public ObjectId GetBlockIdByName(string BlockName)
        {
            BlockName = BlockName.ToUpper();
            if (HasBlock(BlockName))
            {
                foreach (ObjectId id in BlockTbl)
                {
                    BlockTableRecord l = id.GetObject(OpenMode.ForRead) as BlockTableRecord;
                    if (l.Name.ToUpper() == BlockName)
                    {
                        return id;
                    }
                }
            }
            return ObjectId.Null;
        }
        /// <summary>
        /// 块名与块ID对照关系
        /// </summary>
        Dictionary<string, ObjectId> BlockIdDict = new Dictionary<string, ObjectId>();
        /// <summary>
        /// 查找同名块并返回块Id（如果找不到，则克隆块）
        /// </summary>
        /// <param name="BlockId"></param>
        /// <returns></returns>
        public ObjectId FindOrCloneBlock(ObjectId BlockId)
        {
            BlockTableRecord block = BlockId.GetObject(OpenMode.ForRead) as BlockTableRecord;
            string blockname = block.Name;
            if (BlockIdDict.Keys.Contains(blockname)) return BlockIdDict[blockname];//先查询字典中有没有
            ObjectId NewBlockId = GetBlockIdByName(blockname);//查询块表中有没有
            if (NewBlockId.IsNull)
            {
                NewBlockId = CreateBlockByClone(BlockId);//克隆块
            }
            BlockIdDict.Add(blockname, NewBlockId);
            return NewBlockId;
        }
        /// <summary>
        /// 通过块Id来克隆块
        /// </summary>
        /// <param name="BlockId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ObjectId CreateBlockByClone(ObjectId BlockId)
        {
            try
            {
                using (Transaction tran = database.TransactionManager.StartTransaction())
                {
                    BlockTableRecord SourceBtr = BlockId.GetObject(OpenMode.ForRead) as BlockTableRecord;
                    BlockTbl.UpgradeOpen();//提升权限
                    BlockTableRecord newBlock = new BlockTableRecord();
                    newBlock.Name = SourceBtr.Name;
                    ObjectId NewBlockId = BlockTbl.Add(newBlock);
                    tran.AddNewlyCreatedDBObject(newBlock, true);
                    BlockTbl.DowngradeOpen();//降低权限
                    BlockHelper blockHelper = new BlockHelper(dwgHelper, newBlock);
                    blockHelper.CloneEntitiesFromBlock(BlockId);
                    tran.Commit();
                    return NewBlockId;
                }
            }
            catch (Exception ex)
            {
                Logger.log("CreateBlockByClone", ex.Message);
            }
            return ObjectId.Null;
        }



        /// <summary>
        /// 当前空间Id
        /// </summary>
        public ObjectId CurrentSpaceId
        {
            get
            {
                return database.CurrentSpaceId;
            }
        }
        /// <summary>
        /// 当前空间
        /// </summary>
        public BlockHelper CurrentSpace
        {
            get {
                return new BlockHelper(dwgHelper, CurrentSpaceId);
            }
        }
        /// <summary>
        /// 模型空间Id
        /// </summary>
        public ObjectId ModelSpaceId {
            get {
                ObjectId id = GetBlockIdByName("*Model_Space");
                return id;
            }
        }
        /// <summary>
        /// 模型空间
        /// </summary
        BlockHelper _ModelSpace = null;
        public BlockHelper ModelSpace
        {
            get
            {
                if (_ModelSpace == null) {
                    _ModelSpace = new BlockHelper(dwgHelper, ModelSpaceId);
                }
                return _ModelSpace;
            }
        }
    }
}
