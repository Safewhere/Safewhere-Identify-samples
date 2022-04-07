# Access Safewhere Identify data from external code

Sometimes you may need to get information about the in use protocol connection from within custom code executing on Identify e.g. a generic provider, a transformation, and an interceptor.
Although the most developer-friendly way is to expose them via well-defined data objects, the problem is that such approach needs two things:

- Change Identify code which incurs a lot of efforts.
- Update the Safewhere.External contract which forces all custom implementations to be recompiled. Having to update Safewhere.External every time that we need to expose a single new property is not a good choice.

Therefore, we take a more pragmatic approach to solve this business need by providing extension methods and sample code for how to access those pieces of data using internal data structure.
You need different helpers for [passive](../ExternalSamples/PassiveContextService.cs) and [active](../ExternalSamples/ActiveContextService.cs) scenarios. You can see how they are used from various classes found in the ExternalSamples folder.

## Using IIdentifyRequestInformation and ISessionLoginContext

The PassiveContextService class shows you how to an object of type IIdentifyRequestInformation or ISessionLoginContext from HttpContext. Some external interfaces such as interceptors have an IIdentifyRequestInformation object available as an input parameter already.

After having access to those objects, you can use them to get other important data:

```CSharp
// Examples for how to use extension methods to access Identify internal data structure
// Need to add using Safewhere.External.Services;
var sessionLoginContext = requestInformation.IdentifyLoginContext;
var endpointContext = requestInformation.GetEndpointContext();
string entityId = sessionLoginContext.GetProtocolConnectionEntityId();
Guid protocolConnectionId = sessionLoginContext.GetProtocolConnectionId();
var authenticationConnectionEntityId = requestInformation.GetAuthenticationConnectionEntityId();
Guid authenticationConnectionId = requestInformation.GetAuthenticationConnectionId();   // there are two ways to get authenticationConnectionId
Guid authenticationConnectionId2 = sessionLoginContext.GetAuthenticationConnectionId();
```

Sample code:

- [SocialSecurityNumberConfirmationInterceptorService](../ExternalSamples/SocialSecurityNumberConfirmationInterceptorService.cs)
- [PartnerSelectionInterceptorService](../ExternalSamples/PartnerSelectionInterceptorService.cs)
- [AddDummyClaimTransformation](../ExternalSamples/AddDummyClaimTransformation.cs)
- [DummyGenericValidatorUsernamePassword](../ExternalSamples/DummyGenericValidatorUsernamePassword.cs)

Note: After adding extension methods to facilitate working with internal Identify data structure, we removed most sample code from the PassiveContextService class. If you want to keep using old way, you can check the old version out at https://github.com/Safewhere/Safewhere-Identify-samples/blob/4773ab981b5ee4a7c370b92750c18697177dd7b4/Extensible%20Identify/ExternalSamples/PassiveContextService.cs.