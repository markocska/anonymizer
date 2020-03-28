using LoggingDal.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
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
    }
}
