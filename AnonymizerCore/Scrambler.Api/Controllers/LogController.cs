using LoggingDal.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Scrambler.Api.Dtos;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scrambler.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ILogQueryService _logQueryService;

        public LogController(ILogQueryService logQueryService)
        {
            _logQueryService = logQueryService;
        }

        [HttpPost("filter")]
        public async Task<ActionResult<Log>> GetLogsWithFilter([FromBody] LogFilterRequest filterRequest)
        {
            //For performance reasons we don't use Automapper here.

            var logs = (await _logQueryService.GetLogs(
                new LoggingDal.PaginationParams
                {
                    Offset = filterRequest.PaginationParams.Offset,
                    PageNumber = filterRequest.PaginationParams.PageNumber
                },
                new LoggingDal.FilterParams
                {
                    GroupKey = filterRequest.GroupKey,
                    JobKey = filterRequest.JobKey,
                    Description = filterRequest.Description,
                    FromDate = filterRequest.FromDate,
                    ToDate = filterRequest.ToDate,
                    Severity = filterRequest.Severity
                }));


            return Ok(
                new LogReportResponse
                {
                    TotalNumber = logs.totalNumber,
                    Logs = logs.logs.Select(l => new Log
                    {
                        Id = l.Id,
                        GroupKey = l.GroupKey,
                        JobKey = l.JobKey,
                        JobDescription = l.JobDescription,
                        Message = l.Msg,
                        Severity = l.Severity,
                        TimeStamp = l.Timestamp
                    })
                });
        }

        [HttpGet("severity")]
        public ActionResult<List<string>> GetSeverityLevelNames()
        {
            return Ok(Enum.GetNames(typeof(LogEventLevel)));
        }
    }
}
