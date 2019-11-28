using Scrambler.DatabaseServices.Indexes;
using Scrambler.MySql.SqlScripts;
using Scrambler.TableInfo.Interfaces;
using Scrambler.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.MySql.DatabaseServices
{
    public class MySqlIndexServices : IIndexService
    {
        private readonly IQueryHelper _queryHelper;

        public MySqlIndexServices(IQueryHelper queryHelper)
        {
            _queryHelper = queryHelper;
        }

        public void TurnOffIndexes(ITableInfo tableInfo)
        {
            throw new NotImplementedException();
        }

        public void TurnOnIndexes(ITableInfo tableInfo)
        {
            throw new NotImplementedException();
        }

        private void EnableDisableIndexes(ITableInfo tableInfo, bool enable)
        {
            if (tableInfo == null) { throw new ArgumentException("The table info parameter can not be null."); };

            var indexOnOffTemplate = new TurnOnOffIndexes(enable);

            string indexOnOffQuery;
            if (enable)
            {
                indexOnOffQuery = indexOnOffTemplate.TransformText();
            }
            else
            {
                indexOnOffQuery = indexOnOffTemplate.TransformText();
            }

            Console.WriteLine(indexOnOffQuery);
            try
            {
                _queryHelper.ExecuteNonQueryWithoutParams(tableInfo.DbConnectionString, indexOnOffQuery);
            }
            catch (Exception ex)
            {
                var enableDisableStr = enable ? "enable" : "disable";
                throw new IndexServiceException($"Error while trying to {enableDisableStr} indexes.", ex);
            }

        }
    }
}
