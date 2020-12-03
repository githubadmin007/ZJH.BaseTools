using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Xml;
using ZJH.BaseTools.BasicExtend;
using ZJH.BaseTools.DB.Extend;
using ZJH.BaseTools.IO;

namespace ZJH.BaseTools.DB
{
    public abstract class DatabaseHelper : IDisposable
    {
        #region 工厂函数
        /// <summary>
        /// 根据配置文件来创建数据库帮助类
        /// </summary>
        /// <param name="name">配置文件中的数据库连接名称</param>
        /// <returns></returns>
        public static DatabaseHelper CreateByConnName(string name)
        {
            List<ConnConfig> lst = GlobalConfig.AppCfg.getListByPath<ConnConfig>("configuration/DB/ConnStrings/ConnString");
            ConnConfig cfg = lst.Find(l => l.name == name);
            if (cfg != null)
            {
                return CreateByConnStr(cfg.str, cfg.type);
            }
            return null;
        }
        /// <summary>
        /// 根据配置文件来创建数据库帮助类
        /// </summary>
        /// <param name="name">配置文件中的数据库连接名称</param>
        /// <returns></returns>
        public static T CreateByConnName<T>(string name) where T : DatabaseHelper
        {
            return CreateByConnName(name) as T;
        }
        /// <summary>
        /// 根据数据库连接字符串创建数据库帮助类
        /// </summary>
        /// <param name="connStr">数据库连接字符串</param>
        /// <param name="dbType">数据库类型（支持：Oracle、MySQL）</param>
        /// <returns></returns>
        public static DatabaseHelper CreateByConnStr(string connStr, string dbType)
        {
            switch (dbType)
            {
                case "Oracle":
                    return new OracleDatabaseHelper(connStr);
                case "SQLServer":
                    return new SQLServerDatabaseHelper(connStr);
                case "MySQL":
                    return new MySQLDatabaseHelper(connStr);
                case "SQLite":
                    string dbPath = PathHelper.Combine(GlobalConfig.BasePath, connStr);
                    connStr = $"Data Source={dbPath};Version=3;";
                    return new SQLiteDatabaseHelper(connStr);
            }
            return null;
        }
        /// <summary>
        /// 连接字符串配置类
        /// </summary>
        class ConnConfig : IO.XmlReader.IXmlNode
        {
            public string name;
            public string str;
            public string type;
            public void ParseXmlNode(XmlNode node)
            {
                name = node.Attributes["name"].Value;
                str = node.Attributes["str"].Value;
                type = node.Attributes["type"].Value;
            }
            public XmlNode ToXmlNode()
            {
                return null;
            }
        }
        #endregion

        #region 虚函数
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        protected abstract DbConnection CreateConnection();
        /// <summary>
        /// 创建命令执行器
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        protected abstract DbCommand CreateCommand();
        /// <summary>
        /// 创建数据适配器
        /// </summary>
        /// <param name="commandText">Select SQL语句</param>
        /// <param name="connection"></param>
        /// <returns></returns>
        protected abstract DbDataAdapter CreateDataAdapter(string commandText, DbConnection connection);
        /// <summary>
        /// 使用DataAdapter对数据库进行插入/删除/更新时，会要求提供InsertCommand/DeleteCommand/UpdateCommand，此方法用于自动创建Command
        /// 注：只适用于单个表的插入/删除/更新
        /// </summary>
        /// <param name="adapter"></param>
        /// <returns></returns>
        protected abstract DbCommandBuilder CreateCommandBuilder(DbDataAdapter adapter);
        /// <summary>
        /// 创建参数对象
        /// </summary>
        /// <param name="DbParams"></param>
        /// <returns></returns>
        protected abstract IEnumerable<DbParameter> CreateParameters(params DbParam[] DbParams);
        #endregion

        #region 创建对象
        /// <summary>
        /// 创建命令执行器
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandText"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        protected DbCommand CreateCommand(DbConnection connection, string commandText = "", DbTransaction trans = null) {
            DbCommand cmd = CreateCommand();
            cmd.Connection = connection;
            if (!commandText.IsNullOrWhiteSpace()) {
                cmd.CommandText = commandText;
            }
            if (trans != null) {
                cmd.Transaction = trans;
            }
            return cmd;
        }
        /// <summary>
        /// 创建数据适配器(支持参数化查询)
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandText"></param>
        /// <param name="DbParams"></param>
        /// <returns></returns>
        protected DbDataAdapter CreateDataAdapter(DbConnection connection, string commandText, params DbParam[] DbParams) {
            DbDataAdapter adapter = CreateDataAdapter(commandText, connection);
            if (adapter != null && adapter.SelectCommand != null) {
                var values = CreateParameters(DbParams).ToArray();
                adapter.SelectCommand.Parameters.AddRange(values);
            }
            return adapter;
        }
        #endregion

        #region 属性管理
        /// <summary>
        /// DbConnection使用者数量
        /// </summary>
        private int connUserCount = 0;
        /// <summary>
        /// 连接字符串
        /// </summary>
        protected string connStr = "";
        protected DbConnection conn = null; 
        protected DbCommand cmd = null;
        /// <summary>
        /// 打开连接(与关闭连接成对出现)
        /// </summary>
        protected void OpenConnection()
        {
            if (connUserCount == 0)
            {
                if (conn != null) conn.Open();
            }
            connUserCount++;
        }
        /// <summary>
        /// 关闭连接(与打开连接成对出现)
        /// </summary>
        protected void CloseConnection()
        {
            connUserCount--;
            if (connUserCount == 0)
            {
                if (conn != null) conn.Close();
            }
        }
        /// <summary>
        /// 构造函数，产生一个默认的DbConnection和DbCommand
        /// </summary>
        /// <param name="connStr"></param>
        protected DatabaseHelper(string connStr) {
            this.connStr = connStr;
            conn = CreateConnection();
            cmd = CreateCommand(); // 必须先创建conn再创建cmd
        }
        /// <summary>
        /// 注销对象
        /// </summary>
        public void Dispose() {
            if (conn != null)
            {
                conn.Dispose();
            }
            if (cmd != null)
            {
                cmd.Dispose();
            }
        }
        #endregion


        #region SQL相关
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, params DbParam[] DbParams)
        {
            int result = 0;
            try
            {
                OpenConnection();
                cmd.CommandText = sql;
                var values = CreateParameters(DbParams).ToArray();
                cmd.Parameters.AddRange(values);
                result = cmd.ExecuteNonQuery(); ;
            }
            catch (Exception ex)
            {
                Logger.log("ExecuteNonQuery", ex.Message);
            }
            finally
            {
                CloseConnection();
                cmd.CommandText = "";
                cmd.Parameters.Clear();
            }
            return result;
        }
        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, params DbParam[] DbParams)
        {
            object result = null;
            try
            {
                OpenConnection();
                cmd.CommandText = sql;
                var values = CreateParameters(DbParams).ToArray();
                cmd.Parameters.AddRange(values);
                result = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Logger.log("ExecuteScalar", ex.Message);
            }
            finally
            {
                CloseConnection();
                cmd.CommandText = "";
                cmd.Parameters.Clear();
            }
            return result;
        }
        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列,并转为string格式
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string ExecuteScalarToString(string sql, params DbParam[] DbParams)
        {
            object result = ExecuteScalar(sql, DbParams);
            return result == null ? "" : result.ToString();
        }
        /// <summary>
        /// 检查指定表是否存在符合条件的记录
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool HasRecord(string tablename, string where = "1=1", params DbParam[] DbParams)
        {
            string sql = string.Format("select count(*) from {0} where {1}", tablename, where);
            string result = ExecuteScalarToString(sql, DbParams);
            int num = 0;
            int.TryParse(result, out num);
            return num > 0;
        }
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="sqls">多条SQL语句</param>
        public bool ExecuteSqlTran(IEnumerable<string> sqls) {
            DbTransaction tran = null;
            try
            {
                OpenConnection();
                cmd.Transaction = tran = conn.BeginTransaction();
                sqls = sqls.Where(sql => !sql.IsNullOrWhiteSpace());
                foreach (string sql in sqls) {
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
                tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                if (tran != null) tran.Rollback();
                Logger.log("ExecuteSqlTran", ex.Message);
            }
            finally
            {
                CloseConnection();
                cmd.CommandText = "";
                cmd.Transaction = null;
            }
            return false;
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string sql, params DbParam[] DbParams)
        {
            DbConnection conn = null;
            DbDataReader reader = null;
            try
            {
                conn = CreateConnection();
                // TODO:不知道这里不释放DbCommand会不会占用资源
                // 这里不对DbCommand使用using是因为：在MySQL中using结束时DbConnection就会被关闭
                DbCommand cmd = CreateCommand(conn, sql);
                conn.Open();
                var values = CreateParameters(DbParams).ToArray();
                cmd.Parameters.AddRange(values);
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                Logger.log("ExecuteReader", ex.Message);
                if (conn != null) conn.Close();
            }
            return reader;
        }
        #endregion

        #region 转换相关
        /// <summary>
        /// 将首行转换为Dictionary对象
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public Dictionary<string, object> ExecuteReader_ToDict(string sql, params DbParam[] DbParams)
        {
            using (IDataReader reader = ExecuteReader(sql, DbParams))
            {
                return DBConvert.IDataReader_to_Dict(reader);
            }
        }
        /// <summary>
        /// 将所有记录转换为Dictionary数组
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public List<Dictionary<string, object>> ExecuteReader_ToList(string sql, params DbParam[] DbParams)
        {
            var results = new List<Dictionary<string, object>>();
            using (IDataReader reader = ExecuteReader(sql, DbParams))
            {
                Dictionary<string, object> dict;
                while (null != (dict = DBConvert.IDataReader_to_Dict(reader)))
                {
                    results.Add(dict);
                }
            }
            return results;
        }
        #endregion


        #region DataTable相关
        /// <summary>
        /// 执行查询语句，返回DataTable
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public DataTable GetDataTable(string sql, params DbParam[] DbParams)
        {
            DataTable table = null;
            try
            {
                OpenConnection();
                using (DbDataAdapter adapter = CreateDataAdapter(conn, sql, DbParams)) {
                    table = new DataTable();
                    adapter.Fill(table);
                }
            }
            catch (Exception ex)
            {
                Logger.log("GetDataTable", ex.Message);
            }
            finally
            {
                CloseConnection();
            }
            return table;
        }

        /// <summary>
        /// 将DataTable插入数据库。（注：使用DataAdapter实现，数据量大时可能效率较低）
        /// </summary>
        /// <param name="insertDataTable">要插入的数据</param>
        /// <param name="insertTableName">要插入的表名，如果DataTable有名字可以不填</param>
        /// <returns></returns>
        public bool InsertDataTable(DataTable insertDataTable, string insertTableName = "") {
            try
            {
                if (insertTableName.IsNullOrWhiteSpace())
                {
                    insertTableName = insertDataTable.TableName;
                }
                OpenConnection();
                // TODO:insertTableName未做防SQL注入
                using (DbDataAdapter adapter = CreateDataAdapter($"select * from {insertTableName} where 1=0", conn)) // 只获取表结构
                using (CreateCommandBuilder(adapter))
                {
                    DataTable dbTable = new DataTable();
                    adapter.Fill(dbTable);
                    insertDataTable.CopyTo(dbTable);
                    adapter.Update(dbTable);
                }
            }
            catch (Exception ex)
            {
                Logger.log("InsertDataTable", ex.Message);
                return false;
            }
            finally
            {
                CloseConnection();
            }
            return true;
        }

        /// <summary>
        /// 执行多个查询语句，返回DataSet
        /// </summary>
        /// <param name="sqls"></param>
        /// <returns></returns>
        public DataSet GetDataSet(IEnumerable<string> sqls) {
            try
            {
                OpenConnection(); // 提前打开连接，避免在循环中反复开关连接
                DataSet dtSet = new DataSet();
                foreach (string sql in sqls) {
                    DataTable table = GetDataTable(sql);
                    dtSet.Tables.Add(table);
                }
                return dtSet;
            }
            catch (Exception ex)
            {
                Logger.log("GetDataSet", ex.Message);
            }
            finally
            {
                CloseConnection();
            }
            return null;
        }

        /// <summary>
        /// 插入多张表到数据库，其中Datatable的TableName为要插入的数据库表名
        /// </summary>
        /// <param name="insertDataSet"></param>
        public void InsertDataSet(IEnumerable<DataTable> insertTables) {
            DbTransaction tran = null;
            try
            {
                OpenConnection();
                cmd.Transaction = tran = conn.BeginTransaction();
                foreach (DataTable table in insertTables)
                {
                    using (DbDataAdapter adapter = CreateDataAdapter($"select * from {table.TableName} where 1=0", conn)) // 只获取表结构
                    using (DbCommandBuilder builder = CreateCommandBuilder(adapter))
                    {
                        adapter.SelectCommand.Transaction = tran;
                        builder.GetInsertCommand().Transaction = tran;
                        DataTable dbTable = new DataTable();
                        adapter.Fill(dbTable);
                        table.CopyTo(dbTable);
                        adapter.Update(dbTable);
                    }
                }
                tran.Commit();
            }
            catch (Exception ex)
            {
                if (tran != null) tran.Rollback();
                Logger.log("InsertDataSet", ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// 执行查询语句，返回第一行数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="DbParams"></param>
        /// <returns></returns>
        public DataRow GetFirstRow(string sql, params DbParam[] DbParams) {
            try
            {
                DataTable table = GetDataTable(sql, DbParams);
                if (table.Rows.Count > 0) {
                    return table.Rows[0];
                }
            }
            catch (Exception ex)
            {
                Logger.log("GetFirstRow", ex.Message);
            }
            return null;
        }

        /// <summary>
        /// 将DataRow插入数据库。
        /// </summary>
        /// <param name="row"></param>
        /// <param name="insertTableName">要插入的表名，如果DataTable有名字可以不填</param>
        /// <returns></returns>
        public bool InsertDataRow(DataRow row, string insertTableName)
        {
            try
            {
                if (row == null) {
                    return false;
                }
                // 移除其他行
                row.Table.RemoveRows(_row => _row != row);
                return InsertDataTable(row.Table, insertTableName);
            }
            catch (Exception ex)
            {
                Logger.log("InsertDataRow", ex.Message);
            }
            return false;
        }

        public bool UpdateDataRow(DataRow row, string updateTableName) {
            try
            {
                //if (row == null)
                //{
                //    return false;
                //}
                //// 移除其他行
                //row.Table.RemoveRows(_row => _row != row);
                //return InsertDataTable(row.Table, insertTableName);
            }
            catch (Exception ex)
            {
                Logger.log("InsertDataRow", ex.Message);
            }
            return false;
        }
        #endregion













        /// <summary>
        /// 查询数据，并构建插入SQL语句
        /// </summary>
        /// <param name="sql">查询SQL语句</param>
        /// <param name="tablename">要插入的表名</param>
        /// <param name="map">字段映射关系,为空时使用原字段名称</param>
        /// <returns></returns>
        [Obsolete("函数不太通用，不应该放在这里")]
        public List<string> GetInsertSQL(string sql, string tablename, Dictionary<string, string> map = null)
        {
            List<string> sqls = new List<string>();
            using (IDataReader reader = ExecuteReader(sql))
            {
                string fields = reader.JoinAllName(map);
                while (reader.Read())
                {
                    List<string> values = new List<string>();
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        object value = reader.GetValue(i);
                        string typename = reader.GetFieldType(i).Name;
                        switch (typename)
                        {
                            case "Decimal":
                                values.Add(value is DBNull ? "0" : value.ToString());
                                break;
                            case "DateTime":
                                values.Add($"to_date('{value}','yyyy-mm-dd hh24:mi:ss')");
                                break;
                            case "String":
                            default:
                                values.Add($"'{value}'");
                                break;
                        }
                    }
                    sqls.Add($"insert into {tablename}({fields}) values({values.Join(",")})");
                }
            }
            return sqls;
        }
    }

    public class DbParam
    {
        public string Key { get; }
        public object Value { get; }
        public DbParam(string key, object value) {
            Key = key;
            Value = value;
        }
    }
}
