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

                var sqlParams = new List<SqlParameter>();

                SqlHelper.ExecuteNonQuery("Data Source=HUVDEV1;Initial Catalog=CNFS_HUN;Integrated Security=sspi;", sqlParams, Resources.Create_types);

                var constantColumnNameList = new List<string> { "company_rate_cap_id", "refinancing_agreement_id" };
                var scrambledColumnNameList = new List<string> { "agreement_num", "creation_week"};

                var tableInfo = new TableInfo("Data Source=HUVDEV1;Initial Catalog=CNFS_HUN;Integrated Security=sspi;", "CNFS_HUN", "dbo", "agreement_table");
                ParameterChecker.CheckInputParams(tableInfo, constantColumnNameList, scrambledColumnNameList);
                ParameterChecker.CheckPrimaryKeys(tableInfo, constantColumnNameList, scrambledColumnNameList);

                IndexManager.TurnIndexesOnOff(tableInfo, false);

                
                string sqlToGetConstantTypes = ColumnTypeManager.GenerateColumnTypeSqlQuery(tableInfo, constantColumnNameList);

                string sqlToGetScrambledTypes = ColumnTypeManager.GenerateColumnTypeSqlQuery(tableInfo, scrambledColumnNameList);

                var constantColumnsAndValuesDict = new Dictionary<string, string>();

                constantColumnsAndValuesDict.Add("company_rate_cap_id", "5");
                constantColumnsAndValuesDict.Add("refinancing_agreement_id", "4");

                ScrambleManager.Scramble(tableInfo, constantColumnsAndValuesDict, scrambledColumnNameList, sqlToGetConstantTypes, sqlToGetScrambledTypes, "");

                IndexManager.TurnIndexesOnOff(tableInfo, true);


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
