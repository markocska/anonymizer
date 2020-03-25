using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scrambler.Api.Dtos;
using Scrambler.Quartz.Interfaces;

namespace Scrambler.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TriggerController : ControllerBase
    {
        private readonly ISchedulingService _schedulingService;

        public TriggerController(ISchedulingService schedulingService)
        {
            _schedulingService = schedulingService;
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string triggerGroup, [FromQuery] string triggerName)
        {
            var wasFoundAndDeleted = await _schedulingService.DeleteTrigger(triggerName, triggerGroup);

            if (!wasFoundAndDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTrigger createTrigger)
        {
            var schedulingResult = await  _schedulingService.AddJobSchedule(createTrigger.JobGroup, createTrigger.JobName, createTrigger.CronExpression,
                createTrigger.TriggerDescription);

            if (!schedulingResult.IsSuccessful)
            {
                return BadRequest(new ErrorResponse { ErrorMessage = schedulingResult.ErrorMessage });
            }

            return Ok(new TriggerSuccessfullyCreated
            {
                JobGroup = schedulingResult.JobKey.Group,
                JobName = schedulingResult.JobKey.Name,
                TriggerGroup = schedulingResult.TriggerKey.Group,
                TriggerName = schedulingResult.TriggerKey.Name
            });
        }

       
    }
}