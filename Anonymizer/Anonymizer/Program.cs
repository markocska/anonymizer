using Anonymizer.Config;
using Anonymizer.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Anonymizer
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseConfigurationSection config =
                System.Configuration.ConfigurationManager.GetSection(DatabaseConfigurationSection.SectionName) as DatabaseConfigurationSection;

            //if (config != null)
            //{
            //    var dbs = config.Databases;
            //    for (int i = 0; i < config.Databases.Count; i++)
            //    {
            //        var db = config.Databases[i];
            //        Console.WriteLine(db.connectionString);
            //        for (int j = 0; i < db.Tables.Count; i++)
            //        {
            //            var table = db.Tables[j];
            //            Console.WriteLine($"\t {table.NameWithSchema}  {table.Index}");
            //            for (int k = 0; k < table.Columns.Count; k++)
            //            {
            //                var column = table.Columns[k];
            //                Console.WriteLine($"\t \t {column.Name}");
            //            }
            //        }
            //    }
            //}

            var sqlParams = new List<SqlParameter>();

            SqlHelper.ExecuteProcedure("Data Source=HUVDEV1;Initial Catalog=Anonymizer;Integrated Security=sspi;", sqlParams, Resources.Create_types);

            DataTable columnNameTable = new DataTable();
            columnNameTable.Columns.Add("column_name", typeof(string));
            columnNameTable.Rows.Add("Customer_Business_Id");

            var queryParams = new SqlParameter[]
            {
                new SqlParameter() {ParameterName = "@columns_table", SqlDbType = SqlDbType.Structured, Value = columnNameTable, TypeName = "dbo.AnonymizerColumnList"},
                new SqlParameter("@db_value", "cnfs_hun"),
                new SqlParameter("@schema_value", "dbo"),
                new SqlParameter("@table_value", "agreement_table")
            };

            DataTable result = SqlHelper.ExecuteQuery("Data Source=HUVDEV1;Initial Catalog=Anonymizer;Integrated Security=sspi;", queryParams, Resources.Get_Column_Types);

            Console.WriteLine(result.Rows[0][0] as string);

            Console.ReadKey();
        }
    }
}
