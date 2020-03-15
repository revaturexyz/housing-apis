# Overview of Services
## Disclaimer
This section of the document is not a replacement for Swagger, and is more intended for giving an overview of how the services interact with, and depend on, each other. If you want to know how to call a service, please refer to the Swagger documentation for that service. 
## Inter-Service Communication
// TODO: Talk about service buses and how they work, the various classes which use HTTPRequest, when you should use each, and that there are good examples in the code. \
![something](./Images/APIrelationship.png "servicemap")
## Dependencies
Tenant depends on Identity, Address, and Lodging \
Lodging depends on Identity and Address \
Identity and Address depend on nothing \
UI depends on Identity, Lodging, and Tenant