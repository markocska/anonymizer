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

        public async Task<(List<Logs> logs, int totalNumber)> GetLogs(PaginationParams paginationParams, FilterParams filterParams)
        {
            IQueryable<Logs> query = _dbContext.Logs.Where(l => true);

            if (!string.IsNullOrEmpty(filterParams.Description)) { query = query.Where(l => l.JobDescription.ToLower().Contains(filterParams.Description)); }

            if (!string.IsNullOrEmpty(filterParams.GroupKey)) { query = query.Where(l => l.GroupKey == filterParams.GroupKey); }

            if (!string.IsNullOrEmpty(filterParams.JobKey)) { query = query.Where(l => l.JobKey == filterParams.JobKey); }

            if (!string.IsNullOrEmpty(filterParams.Severity)) { query = query.Where(l => l.Severity == filterParams.Severity); }

            if (filterParams.FromDate != null) { query = query.Where(l => l.Timestamp >= filterParams.FromDate); }

            if (filterParams.ToDate != null) { query = query.Where(l => l.Timestamp <= filterParams.ToDate); }

            var totalNumberTask = query.CountAsync();

            query = query.OrderByDescending(x => x.Timestamp).Skip((paginationParams.PageNumber-1) * paginationParams.Offset).Take(paginationParams.Offset);

            var logs = await query.ToListAsync();
            var totalNumber = await totalNumberTask;

            return (logs: logs, totalNumber: totalNumber);
        }
    }
}
