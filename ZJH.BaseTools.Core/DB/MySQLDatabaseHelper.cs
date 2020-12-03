using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace ZJH.BaseTools.DB
{
    public class MySQLDatabaseHelper : DatabaseHelper
    {
        public MySQLDatabaseHelper(string connStr) : base(connStr) { }

        protected override DbConnection CreateConnection() { 
            return new MySqlConnection(connStr);
        }

        protected override DbCommand CreateCommand() {
            DbCommand cmd =  new MySqlCommand();
            cmd.Connection = conn;
            return cmd;
        }

        protected override DbDataAdapter CreateDataAdapter(string commandText, DbConnection connection)
        {
            DbDataAdapter adapter = new MySqlDataAdapter(commandText, (MySqlConnection)connection);
            return adapter;
        }
        protected override DbCommandBuilder CreateCommandBuilder(DbDataAdapter adapter) {
            DbCommandBuilder builder = new MySqlCommandBuilder();
            builder.DataAdapter = adapter;
            return builder;
        }

        protected override IEnumerable<DbParameter> CreateParameters(params DbParam[] DbParams)
        {
            return DbParams.Select(kv => new MySqlParameter(kv.Key, kv.Value));
        }
    }
}
