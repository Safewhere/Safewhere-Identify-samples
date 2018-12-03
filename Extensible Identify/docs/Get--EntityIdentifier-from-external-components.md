# Access Safewhere Identify data from external code

Sometimes you may need to get information about the in use protocol connection from within custom code executing on Identify e.g. a generic provider, a transformation, and an interceptor.
Although the most developer-friendly way is to expose them via well-defined data objects, the problem is that such approach needs two things:

- Change Identify code which incurs a lot of efforts.
- Update the Safewhere.External contract which forces all custom implementations to be recompiled.

Therefore, we take a more pragmatic approach to solve this business need by providing sample code for how to access those pieces of data using internal data structure.
You need different helpers for [passive](../ExternalSamples/PassiveContextService.cs) and [active](../ExternalSamples/ActiveContextService.cs) scenarios. You can see how they are used from various classes found in the ExternalSamples folder. 

