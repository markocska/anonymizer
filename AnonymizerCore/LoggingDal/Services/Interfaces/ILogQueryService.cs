using LoggingDal.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LoggingDal.Services.Interfaces
{
    public interface ILogQueryService
    {
        Task<List<Logs>> GetLogs(string severity, string jobKey, string groupKey, string description, DateTimeOffset fromDate, DateTimeOffset toDate);
    }
}
