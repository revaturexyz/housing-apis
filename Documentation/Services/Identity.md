# Identity API Service Documentation
  The Identity API Service deals directly with account creation, login, authentication and authorization for all users. Without this service the users (Coordinator, Provider, Tenant) wouldn ot be able to navigate throughout the Housing web page. In addtion, this service gives authorized users access to the database and CRUD operation capability, depending on the users okta role.
## Routes
```
/api/coordinator-accounts/id 
/api/coordinator-accounts/{id}
/api/coordinator-accounts/all
/api/provider-accounts/{id} 
/api/provider-accounts/{id}/approve
/api/tenant-accounts/{id}

Localhost:
  Port: 9100 
```
## Service Methods
### Coordinator:

```
/api/coordinator-accounts/id 
```
- Resource: Account Guid for signed in account 
- GET: Roles: All 
    - Updates okta with roles from local db, also updates local db for coordinators 
    - Consumed by the landing page of the front end SPA 

 ``` 
/api/coordinator-accounts/{id}
```
- Resource: Coordinator 
- Account Get: Roles: Coordinator 
    - Used to get the current coordinator account when logged in as coordinator 
    - Consumed in SPA 

```
/api/coordinator-accounts/all
```
- Resource: List 
- Get: Roles: Coordinator 
    - Used to get a list of all existing coordinators 
    - Currently not consumed 


### Provider: 

```
/api/provider-accounts/{id} 
```
- Resource: Provider 
- Get: Roles: Any 
    - Used to get a provider by id 
    - Consumed in SPA 

```    
/api/provider-accounts/{id} 
```
- Resource: Provider 
- Put("CoordinatorId, Name, Email"): Roles: coordinator 
    - Used to change the name, email, or coordinator in charge of a provider 
    - Consumed in SPA 

```    
/api/provider-accounts/{id}/approve
```
- Resource: Provider 
- Put(id): Roles: Coordinator 
    = Used to change the status of a provider to 
    - Approved Okta will be updated with this change on the next login by the provider 
    - Consumed in SPA 

```
/api/provider-accounts/{id} 
```
- Resource: Provider 
- Delete(id): Roles: Coordinator 
    - Used to delete a provider account 
    - Consumed in SPA 


### Tenant: 

```
/api/tenant-accounts/{id}
```
- Resource: Tenant 
- Get(id) Roles: Tenant, Coordinator, Tenant 
    - Return the details of a tenant by id 
    - Consumed in SPA 


### Notification:
- Currently not implemented
## Known Issues
1. Service Bus. The Service Bus purpose is to connect to the queue and listen/receive a message sent from the tenant service. Based on their message we will call upon the repository accordingly. Although the service bus consumer is implemented within the Identity service in the API project under the services file (ServiceBusConsumer.cs , IServiceBusConsumer.cs), it is not fully functionally and will require some attention. (This service bus will eventually be expanded to handle incoming notifications from the lodging and tenant services as well) 
2. Notifcations. The notifications feature serves as an event alert system usually for the coordinator to recieve notifications regarding all changes for provider complexes as well as tenant accounts and maintenance request, but should also be implemented for tenant and provider capabilities. Although code for the notifications feature is implemented within the project, it is not functional and will requrie attention and modification.
