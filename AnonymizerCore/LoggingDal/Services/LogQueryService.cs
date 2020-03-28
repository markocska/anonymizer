using LoggingDal.Model;
using LoggingDal.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoggingDal.Services
{
    public class LogQueryService : ILogQueryService
    {
        private SerilogContext _dbContext;

        public LogQueryService(SerilogContext context)
        {
            _dbContext = context;
        }

        public List<Logs> GetLogs(string severity, string jobKey, string groupKey, DateTimeOffset fromDate, DateTimeOffset toDate)
        {
            //var query = _dbContext.Logs.Where(l => l.JobKey != null && l.GroupKey != null);
            return new List<Logs>();
        }
    }
}
