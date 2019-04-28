using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.TableInfo
{
    public interface ITableInfoBuilder
    {
        ITableInfo Build();
    }
}
