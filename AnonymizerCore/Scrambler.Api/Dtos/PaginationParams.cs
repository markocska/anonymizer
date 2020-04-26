using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Scrambler.Api.Dtos
{
    public class PaginationParams
    {
        [Range(1,double.MaxValue, ErrorMessage = "Page number must be bigger than 0.")]
        public int PageNumber { get; set; }
        [Range(1, double.MaxValue, ErrorMessage = "Offset must be bigger than 0.")]
        public int Offset { get; set; }
    }
}
