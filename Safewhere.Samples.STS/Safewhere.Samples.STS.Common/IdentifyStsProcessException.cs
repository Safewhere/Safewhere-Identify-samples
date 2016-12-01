using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safewhere.Samples.STS.Common
{
    public class IdentifyStsProcessException : Exception
    {
        public IdentifyStsProcessException(string message) : base(message)
        {
        }
    }
}
