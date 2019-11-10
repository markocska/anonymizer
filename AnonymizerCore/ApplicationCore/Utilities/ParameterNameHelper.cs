using Scrambler.TableInfo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scrambler.Utilities
{
    public static class ParameterNameHelper
    {
        public static string RemoveParenthesises(string str)
        {
            if (str.StartsWith("[") && str.EndsWith("]"))
            {
                var newStr = str.TrimStart('[').TrimEnd(']');
                return newStr;
            }
            else
            {
                return str;
            }
        }

        public static List<string> RemoveParenthesisesFromStringList(List<string> strList)
        {
            return strList.Select(str => RemoveParenthesises(str)).ToList();
        }

        public static string AddParenthesises(string str)
        {   
            if (str == "" || str == null) { return str; }

            if (!str.StartsWith("[") && !str.EndsWith("]"))
            {
                return "[" + str + "]";
            }
            else
            {
                return str;
            }
        }

        public static List<string> AddParenthesisesToStrList(List<string> strList)
        {
            return strList.Select(str => AddParenthesises(str)).ToList();
        }

        public static ITableInfo AddParenthesisToTableInfoConnectionParams(ITableInfo tableInfo)
        {
            tableInfo.DbName = AddParenthesises(tableInfo.DbName);
            tableInfo.SchemaName = AddParenthesises(tableInfo.SchemaName);
            tableInfo.TableName = AddParenthesises(tableInfo.TableName);

            return tableInfo;
        }

        public static ITableInfo RemoveParenthesisFromTableInfoConnectionParams(ITableInfo tableInfo)
        {
            tableInfo.DbName = RemoveParenthesises(tableInfo.DbName);
            tableInfo.SchemaName = RemoveParenthesises(tableInfo.SchemaName);
            tableInfo.TableName = RemoveParenthesises(tableInfo.TableName);

            return tableInfo;
        }

        public static string RemoveParenthesisFromFullTableName(string tableNameWithSchema)
        {
            return tableNameWithSchema.Replace("[", "").Replace("]", "");
        }

        public static string AddParenthesisToFullTableName(string tableNameWithSchema)
        {
            if (tableNameWithSchema.Contains('[') && tableNameWithSchema.Contains(']'))
            {
                return tableNameWithSchema;
            }
    
            return "[" + tableNameWithSchema.Replace(".", "].[") + "]"; 
        }
    }
}
