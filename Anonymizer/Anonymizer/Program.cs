using Anonymizer.Checkers;
using Anonymizer.Config;
using Anonymizer.DbObjects;
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

            try
            {

                #region GetColumnTypesQuery
                var sqlParams = new List<SqlParameter>();

                SqlHelper.ExecuteNonQuery("Data Source=HUVDEV1;Initial Catalog=CNFS_HUN;Integrated Security=sspi;", sqlParams, Resources.Create_types);

                var tableInfo = new TableInfo("Data Source=HUVDEV1;Initial Catalog=CNFS_HUN;Integrated Security=sspi;", "CNFS_HUN", "dbo", "agreement_table");
                ParameterChecker.CheckInputParams(tableInfo, new List<string> { "cancellation_week", "creation_week" }, new List<string> { "deleted_flag", "cancellation_week" });
                ParameterChecker.CheckPrimaryKeys(tableInfo, new List<string> { "cancellation_week", "creation_week" }, new List<string> { "deleted_flag", "customer_id" });

               // DataTable columnNameTable = new DataTable();
               // columnNameTable.Columns.Add("column_name", typeof(string));
               // columnNameTable.Rows.Add("Company_Rate_Cap_Id");


               // var constantColumnsQueryParams = new SqlParameter[]
               // {
               //     new SqlParameter() {ParameterName = "@columns_table", SqlDbType = SqlDbType.Structured, Value = columnNameTable, TypeName = "dbo.AnonymizerColumnList"},
               //     new SqlParameter("@db_value", "cnfs_hun"),
               //     new SqlParameter("@schema_value", "dbo"),
               //     new SqlParameter("@table_value", "agreement_table")
               // };

               // DataTable result = SqlHelper.ExecuteQuery("Data Source=HUVDEV1;Initial Catalog=Anonymizer;Integrated Security=sspi;", constantColumnsQueryParams, Resources.Get_Column_Types);

               // Console.WriteLine(result.Rows[0][0] as string);

               // string getConstantColumnsTypesQuery = result.Rows[0][0] as string;

               // columnNameTable.Rows.Clear();
               // columnNameTable.Rows.Add("agreement_num");

               // var scrambledColumnsQueryParams = new SqlParameter[]
               //{
               //     new SqlParameter() {ParameterName = "@columns_table", SqlDbType = SqlDbType.Structured, Value = columnNameTable, TypeName = "dbo.AnonymizerColumnList"},
               //     new SqlParameter("@db_value", "cnfs_hun"),
               //     new SqlParameter("@schema_value", "dbo"),
               //     new SqlParameter("@table_value", "agreement_table")
               //};


                //result = SqlHelper.ExecuteQuery("Data Source=HUVDEV1;Initial Catalog=Anonymizer;Integrated Security=sspi;", scrambledColumnsQueryParams, Resources.Get_Column_Types);

                //string getScrambledColumnsTypesQuery = result.Rows[0][0] as string;

             

                ////Console.WriteLine(result.Rows[0][0] as string);

                //#endregion

                //#region Disable Non PrimaryKey/Clustered/Unique indexes

                //var disableIndexesQueryParams = new SqlParameter[]
                //{
                //    new SqlParameter("@db_value", "cnfs_hun"),
                //    new SqlParameter("@schema_value", "dbo"),
                //    new SqlParameter("@table_value", "agreement_table"),
                //    new SqlParameter("@enable_value", false)
                //};

                //SqlHelper.ExecuteQuery("Data Source=HUVDEV1;Initial Catalog=Anonymizer;Integrated Security=sspi;", disableIndexesQueryParams,
                //                        Resources.enabledisable_non_PrkeyUniqueClust_indexes);

                //#endregion

                //#region GetTheScramblingScript
                //DataTable scrambledColumnNameTable = new DataTable();
                //scrambledColumnNameTable.Columns.Add("column_name", typeof(string));
                //scrambledColumnNameTable.Rows.Add("agreement_num");

                //DataTable constantColumnNameTable = new DataTable();
                //constantColumnNameTable.Columns.Add("column_name", typeof(string));
                //constantColumnNameTable.Columns.Add("column_value", typeof(string));

                //constantColumnNameTable.Rows.Add("Company_Rate_Cap_Id", "1");

                //var getScramblingScriptParams = new SqlParameter[]
                //{
                //new SqlParameter() {ParameterName = "@const_columns_and_values_table", SqlDbType = SqlDbType.Structured, Value = constantColumnNameTable, TypeName = "dbo.AnonymizerColumnAndValueList"},
                //new SqlParameter() {ParameterName = "@scrambled_columns_table", SqlDbType = SqlDbType.Structured, Value = scrambledColumnNameTable, TypeName = "dbo.AnonymizerColumnList"},
                //new SqlParameter("@sql_to_get_constant_types_value", getConstantColumnsTypesQuery),
                //new SqlParameter("@sql_to_get_scrambled_types_value", getScrambledColumnsTypesQuery),
                //new SqlParameter("@db_value", "cnfs_hun"),
                //new SqlParameter("@schema_value", "dbo"),
                //new SqlParameter("@table_value", "agreement_table"),
                //new SqlParameter("@where_value", "")
                //};

                //result = SqlHelper.ExecuteQuery("Data Source=HUVDEV1;Initial Catalog=Anonymizer;Integrated Security=sspi;", getScramblingScriptParams, Resources.Scramble);

                //Console.WriteLine(result.Rows[0][0] as string);

                //#endregion

                //#region Enable Non PrimaryKey/Clustered/Unique indexes

                //var enableIndexesQueryParams = new SqlParameter[]
                //{
                //    new SqlParameter("@db_value", "cnfs_hun"),
                //    new SqlParameter("@schema_value", "dbo"),
                //    new SqlParameter("@table_value", "agreement_table"),
                //    new SqlParameter("@enable_value", true)
                //};

                //SqlHelper.ExecuteQuery("Data Source=HUVDEV1;Initial Catalog=Anonymizer;Integrated Security=sspi;", enableIndexesQueryParams,
                //                        Resources.enabledisable_non_PrkeyUniqueClust_indexes);

                #endregion

                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
            }
        }
    }
}
