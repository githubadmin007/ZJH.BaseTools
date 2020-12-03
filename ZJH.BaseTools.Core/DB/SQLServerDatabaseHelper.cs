using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace ZJH.BaseTools.DB
{
    public class SQLServerDatabaseHelper : DatabaseHelper
    {
        public SQLServerDatabaseHelper(string connStr) : base(connStr) { }

        protected override DbConnection CreateConnection()
        {
            return new SqlConnection(connStr);
        }

        protected override DbCommand CreateCommand()
        {
            DbCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            return cmd;
        }

        protected override DbDataAdapter CreateDataAdapter(string commandText, DbConnection connection)
        {
            DbDataAdapter adapter = new SqlDataAdapter(commandText, (SqlConnection)connection);
            return adapter;
        }
        protected override DbCommandBuilder CreateCommandBuilder(DbDataAdapter adapter)
        {
            DbCommandBuilder builder = new SqlCommandBuilder();
            builder.DataAdapter = adapter;
            return builder;
        }

        protected override IEnumerable<DbParameter> CreateParameters(params DbParam[] DbParams)
        {
            return DbParams.Select(kv => new SqlParameter(kv.Key, kv.Value));
        }
    }
}
