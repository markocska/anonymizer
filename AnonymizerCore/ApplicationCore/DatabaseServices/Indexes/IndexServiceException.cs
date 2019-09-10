using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.DatabaseServices.Indexes
{
    public class IndexServiceException : Exception
    {
        public IndexServiceException(string message) : base(message)
        {
        }

        public IndexServiceException(string message, Exception innerException): base(message, innerException)
        {

        }
    }
}
