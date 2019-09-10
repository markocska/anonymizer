using Scrambler.TableInfo.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.DatabaseServices.Indexes
{
    public interface IIndexService
    {
        void TurnOffIndexes(ITableInfo tableInfo);
        void TurnOnIndexes(ITableInfo tableInfo);
    }
}
