using Microsoft.AspNetCore.Mvc;
using Scrambler.Quartz.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scrambler.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly ISchedulingService _schedulingService;

        public GroupController(ISchedulingService schedulingService)
        {
            _schedulingService = schedulingService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetAllGroups()
        {
            var groupNames = await _schedulingService.GetAllJobGroups();

            return Ok(groupNames);
        }

        [HttpGet("{groupName}/jobNames")]
        public async Task<ActionResult<IEnumerable<string>>> GetAllJobKeysForJobGroup([FromRoute] string groupName)
        {
            var jobNames = await _schedulingService.GetAllJobKeysForJobGroup(groupName);

            return Ok(jobNames);
        }
    }
}
