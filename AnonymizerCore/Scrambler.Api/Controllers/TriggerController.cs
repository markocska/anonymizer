using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scrambler.Api.Dtos;
using Scrambler.Quartz;
using Scrambler.Quartz.Interfaces;

namespace Scrambler.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TriggerController : ControllerBase
    {
        private readonly ISchedulingService _schedulingService;
        private readonly IMapper _mapper;

        public TriggerController(ISchedulingService schedulingService, IMapper mapper)
        {
            _schedulingService = schedulingService;
            _mapper = mapper;
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

            return Ok(_mapper.Map<SchedulingResult, TriggerSuccessfullyCreated>(schedulingResult));
        }

       
    }
}