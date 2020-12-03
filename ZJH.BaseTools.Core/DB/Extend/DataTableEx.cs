using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace ZJH.BaseTools.DB.Extend
{
    public static class DataTableEx
    {
        /// <summary>
        /// 将表的结构复制到指定表
        /// </summary>
        /// <param name="sourceTable">源表</param>
        /// <param name="targetTable">目标表</param>
        /// <param name="isReplace">如果目标表中列已存在，是否替换为源表的列</param>
        /// <param name="isDeleteMismatchColumn">如果目标表中的列在源表中不存在，是否删除目标表的列</param>
        public static void CopyColumnsTo(this DataTable sourceTable, DataTable targetTable, bool isReplace = false, bool isDeleteMismatchColumn = false) {
            // 移除目标表中的不匹配列
            if (isDeleteMismatchColumn) {
                for (int i = 0; i < targetTable.Columns.Count; i++)
                {
                    DataColumn targetColumn = targetTable.Columns[i];
                    if (!sourceTable.Columns.Contains(targetColumn.ColumnName)) {
                        targetTable.Columns.RemoveAt(i);
                        i--;
                    }
                }
            }
            // 遍历源表的列并复制
            foreach (DataColumn sourceColumn in sourceTable.Columns)
            {
                if (targetTable.Columns.Contains(sourceColumn.ColumnName))
                {
                    if (isReplace)
                    {
                        targetTable.Columns.Remove(sourceColumn.ColumnName);
                        targetTable.Columns.Add(sourceColumn);
                    }
                }
                else {
                    targetTable.Columns.Add(sourceColumn);
                }
            }
        }

        /// <summary>
        /// 将表中的数据复制到指定表
        /// </summary>
        /// <param name="sourceTable">源表</param>
        /// <param name="targetTable">目标表</param>
        /// <param name="isCreateColumn">如果目标表不存在某些列，是否创建这些列</param>
        public static void CopyTo(this DataTable sourceTable, DataTable targetTable,bool isCreateColumn = false) {
            // 创建列
            if (isCreateColumn)
            {
                sourceTable.CopyColumnsTo(targetTable);
            }
            // 复制数据
            foreach (DataRow sourceRow in sourceTable.Rows)
            {
                DataRow newRow = targetTable.NewRow();
                foreach (DataColumn sourceColumn in sourceTable.Columns)
                {
                    if (targetTable.Columns.Contains(sourceColumn.ColumnName)) {
                        newRow[sourceColumn.ColumnName] = sourceRow[sourceColumn.ColumnName];
                    }
                }
                targetTable.Rows.Add(newRow);
            }
        }

        /// <summary>
        /// 移除一些行
        /// </summary>
        /// <param name="sourceTable"></param>
        /// <param name="predicate">返回true的行将被移除</param>
        /// <returns></returns>
        public static int RemoveRows(this DataTable sourceTable, Predicate<DataRow> predicate)
        {
            int num = 0;
            for (int i = 0; i < sourceTable.Rows.Count; i++)
            {
                DataRow row = sourceTable.Rows[i];
                if (predicate(row))
                {
                    sourceTable.Rows.RemoveAt(i);
                    i--;
                    num++;
                }
            }
            return num;
        }
    }
}
