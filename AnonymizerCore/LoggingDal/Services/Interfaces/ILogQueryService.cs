using LoggingDal.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoggingDal.Services.Interfaces
{
    public interface ILogQueryService
    {
        List<Logs> GetLogs(string severity, string jobKey, string groupKey, DateTimeOffset fromDate, DateTimeOffset toDate);
    }
}
