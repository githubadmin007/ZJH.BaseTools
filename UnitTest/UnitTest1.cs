using System;
using System.Data;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZJH.BaseTools.BasicExtend;
using ZJH.BaseTools.DB;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            StringSplitOptions test;
            test = "RemoveEmptyEntries".ToEnum<StringSplitOptions>(StringSplitOptions.None);
            test = "NotThisEnum".ToEnum<StringSplitOptions>(StringSplitOptions.None);
        }

        [TestMethod]
        public void TestGetDataSet() {
            string model_id = "99c11bef-24e4-465d-637f-01b143174d80";
            using (DatabaseHelper helper = DatabaseHelper.CreateByConnStr("Data Source=DB4;User ID=GISETL;Password=fsugic", "Oracle"))
            {
                // Test GetDataTable
                DataTable modelTable = helper.GetDataTable($"select * from etl_model where id='{model_id}'");
                // Test InsertDataTable
                modelTable.Rows[0]["ID"] = "12345678";
                helper.InsertDataTable(modelTable, "etl_model");
                // Test GetDataSet
                DataSet set =  helper.GetDataSet(new string[] {
                    "select * from etl_model where id ='99c11bef-24e4-465d-637f-01b143174d80'",
                    "select * from etl_step where model_id='99c11bef-24e4-465d-637f-01b143174d80'"
                });
                // Test InsertDataSet
                set.Tables[0].TableName = "etl_model";
                set.Tables[0].Rows[0]["ID"] = "12345";
                set.Tables[1].TableName = "etl_step";
                set.Tables[1].Rows[0]["ID"] = "12345";
                helper.InsertDataSet(set);
            }
        }

        [TestMethod]
        public void TestExecuteReader() {
            using (DatabaseHelper helper = DatabaseHelper.CreateByConnStr("Data Source=DB4;User ID=GISETL;Password=fsugic", "Oracle")) {
                var lst = helper.ExecuteReader_ToList("select * from etl_step where model_id='99c11bef-24e4-465d-637f-01b143174d80'");
            }
        }
    }
}
