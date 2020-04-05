using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scrambler.Api.Dtos
{
    public class PaginationParams
    {
        public int PageNumber { get; set; }
        public int Offset { get; set; }
    }
}
