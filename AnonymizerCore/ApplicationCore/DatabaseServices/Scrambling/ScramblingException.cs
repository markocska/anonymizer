using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.DatabaseServices.Scrambling
{
    public class ScramblingException : Exception
    {
        public ScramblingException(string message): base(message)
        {
        }

        public ScramblingException(string message, Exception innerException): base(message, innerException)
        {

        }
    }
}
