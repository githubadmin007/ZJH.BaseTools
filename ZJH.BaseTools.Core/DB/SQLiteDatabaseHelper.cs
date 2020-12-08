using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace ZJH.BaseTools.DB
{
    public class SQLiteDatabaseHelper : DatabaseHelper
    {
        public SQLiteDatabaseHelper(string connStr) : base(connStr) { }

        protected override DbConnection CreateConnection()
        {
            return new SQLiteConnection(connStr);
        }

        protected override DbCommand CreateCommand()
        {
            DbCommand cmd = new SQLiteCommand();
            cmd.Connection = conn;
            return cmd;
        }

        protected override DbDataAdapter CreateDataAdapter(string commandText)
        {
            DbDataAdapter adapter = new SQLiteDataAdapter(commandText, (SQLiteConnection)conn);
            return adapter;
        }
        protected override DbCommandBuilder CreateCommandBuilder(DbDataAdapter adapter)
        {
            DbCommandBuilder builder = new SQLiteCommandBuilder();
            builder.DataAdapter = adapter;
            return builder;
        }

        protected override IEnumerable<DbParameter> CreateParameters(params DbParam[] DbParams)
        {
            return DbParams.Select(kv => new SQLiteParameter(kv.Key, kv.Value));
        }
    }
}
