﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Scrambler.Api.Dtos
{
    public class JobKey
    {
        [Required]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "{0} must be between {2} and {1}")]
        public string Name { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "{0} must be between {2} and {1}")]
        public string Group { get; set; }
    }
}
