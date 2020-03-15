# Tenant
## Endpoints
```
Dev Server:
  https://tenant-aspnet-dev.azurewebsites.net/

Locally:
  VS: localhost:9140
  Docker Compose: localhost:9140 or 192.168.99.100:9140 (depends on your version of Docker)
```
## Tenant
### /api/tenant/
* Resource: Tenant
* GET: Roles: Coordinator
  * Returns a filtered list of tenants (See Swagger for possible filters)
  * Shouldn't consume any servies
  * Consumed in [tenant-searcher]
### /api/tenant/{id}
* Resource: Tenant
* GET: Roles: Coordinator, Tenant (with matching id)
  * Returns a tenant and associated objects based on id
  * Consumes [Address]
  * Consumed in [tenant]
### /api/tenant/batch
* Resource: Batch
* Get: Roles: Coordinator
  * Returns a list of batches potentially filtered by training location
  * Doesn't consume
  * Consumed in [tenant]
### /api/tenant/register
* Resource: Tenant
* Post: Roles: Coordinator
  * Registers a tenant
  * Consumes [Address]
  * Consumed by [Identity] via service bus TenantCRUD
  * Consumed by [tenant]
### /api/tenant/update
* Resource: Tenant
* Put: Roles: Coordinator (Tenant should be able to put in update request for self, but that might be a different endpoint)
  * Updates Existing tenant
  * Consumes [Address]
  * Consumed by [Identity] via service bus TenantCRUD
  * Consumed by [tenant]
### /api/tenant/delete/{id}
* Resource: Tenant
* Delete: Roles: Coordinator
  * Deletes existing tenant
  * Consumed by [Identity] via service bus TenantCRUD
  * Consumed by [tenant-searcher]
## TenantRoom
### /api/tenant/unassigned
* Resource: Tenant
* Get: Roles: Coordinator
  * Returns a list of tenants not assigned to a room
  * Shouldn't consume anything
  * Consumed by [tenant-assign]
### /api/tenant/assign/availablerooms
* Resource: Room
* Get: Roles: Coordinator
  * Returns a filtered list of rooms (See Swagger for allowed filters)
  * Consumes [Lodging] (probably broken)
  * Consumed by [tenant-assign]
### /api/tenant/assign/{tenantid}
* Resource: Tenant
* Put: Roles: Coordinator
  * Updates tenant room id
  * Consumes [Lodging] via service bus queue AssignedRoom
  * Consumed by [tenant-assign]

## Known issues
1. Puts/Posts don't update batch information, and there is no repository method to handle that.
2. The endpoints should be combined to be restful. ie. There don't need to be different endpoints for update, register, and delete
3. The search for unassigned tenants should probably be incorporated into the normal search, so a coordinator can find "unassigned tenants who are male" which might be done to find people who could fit in a vacancy
4. Available rooms is doing 2 things which should be split up
  - doing a search which is replicated from [Lodging]
  - getting the tenants currently assigned to that room which is done nowhere
5. [tenant-assign] and the methods it calls have not been shown to work

[Identity]: Identity.md
[Lodging]: Lodging.md
[Address]: Address.md
[tenant]: https://github.com/revaturexyz/housing/blob/master/Documentation/Services/Tenant/Tenant.md
[tenant-searcher]: https://github.com/revaturexyz/housing/blob/master/Documentation/Services/Tenant/Tenant-searcher.md
[tenant-assign]: https://github.com/revaturexyz/housing/blob/master/Documentation/Services/Tenant/Tenant-assign.md