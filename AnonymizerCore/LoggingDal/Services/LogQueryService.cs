using LoggingDal.Model;
using LoggingDal.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingDal.Services
{
    public class LogQueryService : ILogQueryService
    {
        private SerilogContext _dbContext;

        public LogQueryService(SerilogContext context)
        {
            _dbContext = context;
        }

        public async Task<List<Logs>> GetLogs(string severity, string jobKey, string groupKey, string description, DateTime? fromDate, DateTime? toDate)
        {
            IQueryable<Logs> query = _dbContext.Logs.Where(l => true);

            if (!string.IsNullOrEmpty(groupKey)) { query = query.Where(l => l.GroupKey == groupKey); }

            if (!string.IsNullOrEmpty(jobKey)) { query = query.Where(l => l.JobKey == jobKey); }

            if (!string.IsNullOrEmpty(severity)) { query = query.Where(l => l.Severity == severity); }

            if(!string.IsNullOrEmpty(description)) { query = query.Where(l => l.JobDescription.ToLower().Contains(description)); }
            
            if (fromDate != null) { query = query.Where(l => l.Timestamp >= fromDate); }

            if (toDate != null) { query = query.Where(l => l.Timestamp <= toDate); }

            return await query.ToListAsync();
        }
    }
}
