Safewhere.Samples.STS.Common
This project is to implement all the common libraries for all STS sample projects 

---
Safewhere.Samples.STS.ClaimAppConsole
This project is a console app which will execute a RST request to Identify via STS certificate mixed endpoint to negotiate a security token which will be used for acessing a secured service. 
All the configuration for RST are defined on its app.config file

---
project Safewhere.Samples.STS.WebsiteDemo
This project is actually the oiosaml .net sample which will demonstrate how to execute RST to Identify via STS certificate mixed endpoint.

---
project Safewhere.Samples.STS.ClaimAppService
This project is a wcf service which is secured by Identify STS service. It has operation GetClaims, which returns all the claims of identity user.

---
project Safewhere.Samples.STS.GenericProviderAppConsole
This project is a console app which will execute a RST request to Identify via STS username mixed endpoint to negotiate a security token which will be used for acessing a secured service. 

---
project Safewhere.Samples.STS.GenericCredentialsValidator
This is a sample of GenericCredentialsValidator which will be executed from Identify code to authenticate upn credentials 