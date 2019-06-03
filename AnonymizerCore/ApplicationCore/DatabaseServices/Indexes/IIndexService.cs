using ApplicationCore.TableInfo.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.DatabaseServices.Indexes
{
    public interface IIndexService
    {
        void TurnOffIndexes(ITableInfo tableInfo);
        void TurnOnIndexes(ITableInfo tableInfo);
    }
}
