using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Scrambler.Api.Dtos;
using Scrambler.Quartz.Configuration;
using Scrambler.Quartz.Interfaces;

namespace Scrambler.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SchedulerController : ControllerBase
    {
        private readonly ISchedulingService _schedulingService;
        private readonly SchedulerConfiguration _schedulerConfiguration;

        public SchedulerController(ISchedulingService schedulingService, SchedulerConfiguration schedulerConfiguration)
        {
            _schedulingService = schedulingService;
            _schedulerConfiguration = schedulerConfiguration;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            var schedulingResult = await _schedulingService.ScheduleSqlScramblingJob("testjob", "testgroup", "testTrigger", "testTriggerGroup", "0,30 * * ? * MON-FRI", "testjob");

            if (!schedulingResult.IsSuccessful)
            {
                return BadRequest(new ErrorResponse { ErrorMessage = schedulingResult.ErrorMessage });
            }

            return Ok();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] CreateSchedulingJob newSchedulingJob)
        {
            _schedulingService.ScheduleSqlScramblingJob(newSchedulingJob.JobName, newSchedulingJob.JobGroup, newSchedulingJob.TriggerName, newSchedulingJob.TriggerGroup,
                newSchedulingJob.CronExpression, newSchedulingJob.Description);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
