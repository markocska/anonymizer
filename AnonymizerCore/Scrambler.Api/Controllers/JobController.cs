using Microsoft.AspNetCore.Mvc;
using Scrambler.Quartz.Configuration;
using Scrambler.Quartz.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scrambler.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly ISchedulingService _schedulingService;
        private readonly SchedulerConfiguration _schedulerConfiguration;

        public JobController(ISchedulingService schedulingService, SchedulerConfiguration schedulerConfiguration)
        {
            _schedulingService = schedulingService;
            _schedulerConfiguration = schedulerConfiguration;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            var jobKeysWithDescription = await _schedulingService.GetAllJobKeysWithDescription();

            return Ok(jobKeysWithDescription);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string jobGroup, [FromQuery] string jobName)
        {
            var wasJobFoundAndDeleted = await _schedulingService.DeleteJob(jobName, jobGroup);

            if (!wasJobFoundAndDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
