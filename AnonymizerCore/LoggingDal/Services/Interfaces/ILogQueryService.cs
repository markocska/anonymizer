using LoggingDal.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LoggingDal.Services.Interfaces
{
    public interface ILogQueryService
    {
        Task<(List<Logs> logs, int totalNumber)> GetLogs(PaginationParams paginationParams, FilterParams filterParams);
    }
}
