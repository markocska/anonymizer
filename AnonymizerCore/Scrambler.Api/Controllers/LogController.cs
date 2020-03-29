using LoggingDal.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Scrambler.Api.Dtos;
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
        public async Task<ActionResult<LogDto>> GetLogsWithFilter([FromBody] LogFilter filter)
        {
            //For performance reasons we don't use Automapper here.
            var logs = (await _logQueryService.GetLogs(filter.Severity, filter.JobKey, filter.GroupKey, filter.Description, filter.FromDate, filter.ToDate))
                .Select(l => new LogDto
                {
                    Id = l.Id,
                    GroupKey = l.GroupKey,
                    JobKey = l.JobKey,
                    JobDescription = l.JobDescription,
                    Message = l.Msg,
                    Severity = l.Severity, 
                    TimeStamp = l.Timestamp
                });

            return Ok(logs);
        }
    }
}
