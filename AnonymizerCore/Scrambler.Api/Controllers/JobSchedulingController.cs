﻿using Microsoft.AspNetCore.Mvc;
using Scrambler.Api.Dtos;
using Scrambler.Quartz.Configuration;
using Scrambler.Quartz.Interfaces;
using System.Threading.Tasks;

namespace Scrambler.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class JobSchedulingController : ControllerBase
    {

        private readonly ISchedulingService _schedulingService;
        private readonly SchedulerConfiguration _schedulerConfiguration;

        public JobSchedulingController(ISchedulingService schedulingService, SchedulerConfiguration schedulerConfiguration)
        {
            _schedulingService = schedulingService;
            _schedulerConfiguration = schedulerConfiguration;
        }

        [HttpPost("sql")]
        public async Task<IActionResult> PostSql([FromBody] CreateScheduledJob newScheduledJob)
        {
            var schedulingResult = await _schedulingService.ScheduleSqlScramblingJob(newScheduledJob.JobName, newScheduledJob.JobGroup, newScheduledJob.TriggerDescription,
                 newScheduledJob.CronExpression, newScheduledJob.Description, newScheduledJob.JobConfig);

            if (!schedulingResult.IsSuccessful)
            {
                return BadRequest(new ErrorResponse { ErrorMessage = schedulingResult.ErrorMessage });
            }

            return Ok(new JobSuccessfullyCreated
            {
                JobGroup = schedulingResult.JobKey.Group,
                JobName = schedulingResult.JobKey.Name,
                TriggerGroup = schedulingResult.TriggerKey.Group,
                TriggerName = schedulingResult.TriggerKey.Name
            });
        }

        [HttpPost("mysql")]
        public async Task<IActionResult> PostMySql([FromBody] CreateScheduledJob newScheduledJob)
        {
            var schedulingResult = await _schedulingService.ScheduleMySqlScramblingJob(newScheduledJob.JobName, newScheduledJob.JobGroup, newScheduledJob.TriggerDescription,
                 newScheduledJob.CronExpression, newScheduledJob.Description, newScheduledJob.JobConfig);

            if (!schedulingResult.IsSuccessful)
            {
                return BadRequest(new ErrorResponse { ErrorMessage = schedulingResult.ErrorMessage });
            }

            return Ok(new JobSuccessfullyCreated
            {
                JobGroup = schedulingResult.JobKey.Group,
                JobName = schedulingResult.JobKey.Name,
                TriggerGroup = schedulingResult.TriggerKey.Group,
                TriggerName = schedulingResult.TriggerKey.Name
            });
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateJobSchedule updatedJobSchedule)
        {

            var jobReschedulingResult =
                await _schedulingService.RescheduleJob(updatedJobSchedule.TriggerGroup, updatedJobSchedule.TriggerName, updatedJobSchedule.CronExpression,
                  updatedJobSchedule.TriggerDescription);

            if (!jobReschedulingResult.IsSuccessful)
            {
                return BadRequest(new ErrorResponse { ErrorMessage = jobReschedulingResult.ErrorMessage });
            }

            return NoContent();
        }
    }
}