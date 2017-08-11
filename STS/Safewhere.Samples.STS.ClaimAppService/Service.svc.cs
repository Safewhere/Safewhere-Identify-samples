using System.Collections.Generic;
using System.Security.Claims;
using System.ServiceModel;

namespace Safewhere.Samples.STS.ClaimAppService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service : IService
    {
        public List<Claim> GetActorClaims()
        {
            var ret = new List<Claim>();
            //var identity = (ClaimsIdentity) (Thread.CurrentPrincipal.Identity);
            var identity = (ClaimsIdentity)(OperationContext.Current.ClaimsPrincipal.Identity);
            if (identity.Actor == null)
                return null;

            foreach (var claim in identity.Actor.Claims)
            {
                ret.Add(new Claim
                {
                    ClaimType = claim.Type,
                    Issuer = claim.Issuer,
                    Subject = claim.Subject.Name,
                    Value = claim.Value,
                    ValueType = claim.ValueType
                });
            }
            return ret;
        }
        public List<Claim> GetClaims()
        {

            var ret = new List<Claim>();
            //var identity = (ClaimsIdentity) (Thread.CurrentPrincipal.Identity);
            var identity = (ClaimsIdentity)(OperationContext.Current.ClaimsPrincipal.Identity);
            foreach (var claim in identity.Claims)
            {
                ret.Add(new Claim
                {
                    ClaimType = claim.Type,
                    Issuer = claim.Issuer,
                    Subject = claim.Subject.Name,
                    Value = claim.Value,
                    ValueType = claim.ValueType
                });
            }
            return ret;
        }
    }
}
