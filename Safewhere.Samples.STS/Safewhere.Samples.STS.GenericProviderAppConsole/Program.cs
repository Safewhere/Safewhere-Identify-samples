using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Safewhere.Samples.STS.Common;
using Safewhere.Samples.STS.Common.ClaimAppService;

namespace Safewhere.Samples.STS.GenericProviderAppConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                RequestSecurityTokenConfiguration rstConfig = RequestSecurityTokenConfiguration.Get();
                //Send request to issue security token from Samples.STS
                var token = IdentifyStsUsernameEndpointHandler.GetSecurityToken(rstConfig);

                //Use the issued token to execute the server on Samples.Service
                var service = IdentifyStsServiceCommon.GetClaimAppService<IService>(token, rstConfig.AppliesTo);
                var claims = service.GetClaims();

                Console.WriteLine("Ping to service successfully.");
                Console.WriteLine("There are '{0}' claims retrieved.", claims.Length);
                if (claims.Length > 0)
                {
                    int i = 1;
                    foreach (var claim in claims)
                    {
                        Console.WriteLine("Claim '{0}': '{1}' - '{2}'", i, claim.ClaimType, claim.Value);
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception thrown: " + ex);
            }
            Console.ReadLine();
        }
    }
}
