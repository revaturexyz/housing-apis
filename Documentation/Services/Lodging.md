# Lodging API Service Documentation
## Endpoints
```
Dev Server:
  https://complex-aspnet-dev.azurewebsites.net/

Locally:
  VS: localhost:9120
  Docker Compose: localhost:9120 or 192.168.99.100:9120 (depends on your version of Docker)
```
## Complex
### /api/complex
- Resource: Complex
  - Get: Roles: Coordinator, Provider
    - Gets all existing complexes from the database
  - Post: Roles: Provider
    - Adds a complex to the database
    - Return the added complex if successful
  - Put: Roles: Provider
    - Edits a complex and replaces the associated complex amenities with the passed list of amenities

### /api/complex/{complexid}
- Resource: Complex
  - Get: Roles: Coordinator, Provider
	  - Gets an existing complex from the database based on complex id
  - Delete: Roles: Provider
    - Deletes a Complex and its amenities from the database based on complexid

### /api/complex/providerid/{providerid}
- Resource: Complex
  - Get: Roles: Coordinator, Provider
    - Gets all existing complexes with the given providerid
## Room

### /api/room/complexId/{complexId}
- Resource: Room
  - Get: Roles: Coordinator, Provider
    - Gets rooms that match a query string

### api/room/{roomid}
- Resource: Room
  - Get: Roles:
    - Gets a room by room id
  - Put(roomId, APiRoom): Roles: Provider
    - Edits an existing room with the given room id, to the given room object
  - Delete: Roles: Provider
    - Deletes the room with the given id

### api/room
- Resource: Room
  - Post(ApiRoom): Roles: Provider
    - Adds a new room the the database

## Consumption
This service is consumed exclusively by the [lodging] service
Some methods consume the [Address] service

## KNOWN ISSUES
 - Mapping from library models to API models are done in the RoomController rather than using a mapper.

[lodging]: https://github.com/revaturexyz/housing/blob/master/Documentation/Services/Lodging/Lodging.md
[Address]: Address.md
