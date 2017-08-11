using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safewhere.Samples.STS.GenericCredentialsValidator
{
    public class GenericProviderSampleException: Exception
    {
        public GenericProviderSampleException(string exception) : base(exception) { }
    }
}
