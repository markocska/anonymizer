using Scrambler.TableInfo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scrambler.MySql
{
    public class ParameterNameHelper
    {
        public static string RemoveQuotationMarks(string str)
        {
            if (str.StartsWith("`") && str.EndsWith("`"))
            {
                var newStr = str.Replace("`","");
                return newStr;
            }
            else
            {
                return str;
            }
        }

        public static List<string> RemoveQuotationMarksFromStringList(List<string> strList)
        {
            return strList.Select(str => RemoveQuotationMarks(str)).ToList();
        }

        public static string AddQuotationMarks(string str)
        {
            if (str == "" || str == null) { return str; }

            if (!str.StartsWith("`") && !str.EndsWith("`"))
            {
                return "`" + str + "`";
            }
            else
            {
                return str;
            }
        }

        public static List<string> AddQuotationMarksToStrList(List<string> strList)
        {
            return strList.Select(str => AddQuotationMarks(str)).ToList();
        }

        public static ITableInfo AddQuotationMarksToTableInfoConnectionParams(ITableInfo tableInfo)
        {
            tableInfo.DbName = AddQuotationMarks(tableInfo.DbName);
            tableInfo.SchemaName = AddQuotationMarks(tableInfo.SchemaName);
            tableInfo.TableName = AddQuotationMarks(tableInfo.TableName);

            return tableInfo;
        }

        public static ITableInfo RemoveQuotationMarksFromTableInfoConnectionParams(ITableInfo tableInfo)
        {
            tableInfo.DbName = RemoveQuotationMarks(tableInfo.DbName);
            tableInfo.SchemaName = RemoveQuotationMarks(tableInfo.SchemaName);
            tableInfo.TableName = RemoveQuotationMarks(tableInfo.TableName);

            return tableInfo;
        }

        public static string RemoveQuotationMarksFromFullTableName(string tableNameWithSchema)
        {
            return tableNameWithSchema.Replace("`", "");
        }

        public static string AddQuotationMarksToFullTableName(string tableNameWithSchema)
        {
            if (tableNameWithSchema.Contains('`'))
            {
                return tableNameWithSchema;
            }

            return "`" + tableNameWithSchema.Replace(".", "`.`") + "`";
        }
    }
}
