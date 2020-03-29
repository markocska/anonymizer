using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Scrambler.Api.Dtos;
using Scrambler.Quartz.Configuration;
using Scrambler.Quartz.Interfaces;
using Scrambler.Quartz.Model;
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
        private readonly IMapper _mapper;

        public JobController(ISchedulingService schedulingService, SchedulerConfiguration schedulerConfiguration, IMapper mapper)
        {
            _schedulingService = schedulingService;
            _schedulerConfiguration = schedulerConfiguration;
            _mapper = mapper;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<JobKeyWithDescription>>> GetAll()
        {
            var jobKeysWithDescription = await _schedulingService.GetAllJobKeysWithDescription();

            var jobDescriptions = _mapper.Map<List<JobKeyWithDescription>, List<JobDescription>>(jobKeysWithDescription);

            return Ok(new JobDescriptionReportResponse { JobDescriptions = jobDescriptions, TotalNumber = jobDescriptions.Count });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobKeyWithDescription>>> Get([FromQuery] string groupName)
        {

            var jobKeysWithDescription = string.IsNullOrEmpty(groupName) ?
                await _schedulingService.GetAllJobKeysWithDescription() :
                await _schedulingService.GetJobKeysWithDescription(groupName);

            var jobDescriptions = _mapper.Map<List<JobKeyWithDescription>, List<JobDescription>>(jobKeysWithDescription);

            return Ok(new JobDescriptionReportResponse { JobDescriptions = jobDescriptions, TotalNumber = jobDescriptions.Count });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string groupName, [FromQuery] string jobName)
        { 
            var wasJobFoundAndDeleted = await _schedulingService.DeleteJob(jobName, groupName);

            if (!wasJobFoundAndDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}
