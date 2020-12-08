#if FW4_0
//using Oracle.DataAccess.Client;
using System.Data.OracleClient;
#elif CORE
using Oracle.ManagedDataAccess.Client;
#endif
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace ZJH.BaseTools.DB
{
    public class OracleDatabaseHelper : DatabaseHelper
    {
        public OracleDatabaseHelper(string connStr) : base(connStr) { }

        protected override DbConnection CreateConnection()
        {
            return new OracleConnection(connStr);
        }

        protected override DbCommand CreateCommand()
        {
            DbCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            return cmd;
        }
        protected override DbDataAdapter CreateDataAdapter(string commandText)
        {
            DbDataAdapter adapter = new OracleDataAdapter(commandText, (OracleConnection)conn);
            return adapter;
        }
        protected override DbCommandBuilder CreateCommandBuilder(DbDataAdapter adapter)
        {
            DbCommandBuilder builder = new OracleCommandBuilder();
            builder.DataAdapter = adapter;
            return builder;
        }

        protected override IEnumerable<DbParameter> CreateParameters(params DbParam[] DbParams) {
            return DbParams.Select(kv => new OracleParameter(kv.Key, kv.Value));
        }
    }
}
