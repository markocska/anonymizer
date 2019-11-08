﻿using System;
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
        [HttpGet("jobkeyswithdescription")]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            var jobKeysWithDescription = await _schedulingService.GetAllJobKeysWithDescription(); 

            return Ok(jobKeysWithDescription);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateSchedulingJob newSchedulingJob)
        {
           var schedulingResult = await _schedulingService.ScheduleSqlScramblingJob(newSchedulingJob.JobName, newSchedulingJob.JobGroup, newSchedulingJob.TriggerName, newSchedulingJob.TriggerGroup,
                newSchedulingJob.CronExpression, newSchedulingJob.Description);

            if (!schedulingResult.IsSuccessful)
            {
                return BadRequest(new ErrorResponse { ErrorMessage = schedulingResult.ErrorMessage });
            }

            return Ok();
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
