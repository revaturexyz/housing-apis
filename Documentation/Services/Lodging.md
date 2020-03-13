Port: 9120

## METHODS

# Complex

/api/complex
•	Resource: Complex
•	Get: Roles: (#######)
o	Gets all existing complexes from the database

/api/complex/{complexid}
 - Resource: Complex
 - Get: Roles: (########)
	- Gets an existing complex from the database based on complex id

/api/complex/providerid/{providerid}
 - Resource: Complex
 - Get: Roles: (#######)
    - Gets all existing complexes with the given providerid

/api/complex
 - Resource: Complex
 - Post: Roles: Provider
    - Adds a complex to the database
    - Return the added complex if successful

/api/complex
 - Resource: Complex
 - Put: Roles: Provider
    - Edits a complex and replaces the associated complex amenities with the passed list of amenities

/api/complex/{complexid}
 - Resource: Complex
 - Delete: Roles: Provider
    - Deletes a Complex and its amenities from the database based on complexid

# Room

/api/room/complexId/{complexId}
 - Resource: Room
 - Get: Roles: (#####)
    - Gets rooms that match a query string

api/room/{roomid}
 - Resource: Room
 - Get: Roles: (######)
    - Gets a room by room id

api/room
 - Resource: Room
 - Post(ApiRoom): Roles: (######)
    - Adds a new room the the database

api/room/{roomId}
 - Resource: Room
 - Put(roomId, APiRoom): Roles: (######)
    - Edits an existing room with the given room id, to the given room object

api/room/{roomId}
 - Resource: Room
 - Delete: Roles: (######)
    - Deletes the room with the given id

## CONNECTIONS
 - ComplexController accesses the Address API using the AddressRequest service.

## KNOWN ISSUES
 - Mapping from library models to API models are done in the RoomController rather than using a mapper.
