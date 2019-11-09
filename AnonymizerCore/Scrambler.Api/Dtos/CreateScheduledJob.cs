using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Scrambler.Api.Dtos
{
    public class CreateScheduledJob
    {
        [Required]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "{0} must be between {2} and {1}")]
        public string JobName { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "{0} must be between {2} and {1}")]
        public string JobGroup { get; set; }
        public string TriggerDescription { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "{0} must be between {2} and {1}")]
        public string CronExpression { get; set; }
        [Required]
        [StringLength(5000, ErrorMessage = "{0} must be between {2} and {1}")]
        public string Description { get; set; }
    }
}
