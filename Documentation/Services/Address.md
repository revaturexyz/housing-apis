# Address Service API Documentation
  The Address Service API deals with Addresses that are set up within the database. We are able to receive addresses from within the database, add new addresses that exist in the real world, and check the distances between two addresses. These addresses can be used with the Lodging and Tenant service.
  
## Endpoints
```
Dev Server:
  (???)

Locally:
  VS: localhost:9110
  Docker Compose: localhost:9110 or 192.168.99.100:9110 (depends on your version of Docker)
```

## Models:
- Id: Stored as a Guid
- Street: Stored as a String
- City: Stored as a String
- State: Stored as a String
- Country: Stored as a String
- ZipCode: Stored as a String

## Service Methods
### Address:
```
api/address/{id}
```
- Address id is a parameter
- GET: 
  - Finds the address that has an id value equal to the provided address id.
  - Used to find specific details of an address, as only the address id is stored in Lodging and Tenant. 
  - Consumed by the [Tenant] and [Lodging] services.
 ```
 api/address/checkdistance
 ```
 - Three parameters are provided. There is a "to" address, a "from" address, and a "distance" int that is set to 20.
 - GET:
    - Finds out if the two provided addresses are within a specific distance of each other. This is usually set to 20 miles.
    - To pass addresses, you will need to use query parameters of the form `to.street`. Just passing `street` will set it in both addresses.
    - Returns true if within the provided number of miles, and false if the addresses are farther than the provided number of miles.
    - Not consumed right now, but should be consumed by [Tenant] and [Lodging] services
  ```
  api/address
  ```
  - One parameter is provided, that being an address.
  - GET:
    - Acts as a normalized address cache
    - Checks whether the address exists in the DB
      - If it does, returns the normalized address with its GUID
      - If not, stores the normalized address with a new GUID and returns it
   
## Known issues
1. Training Center. The training center is not implemented as of right now. While checkdistance can be used on any two addresses, service method can be used to ensure that a specific address is within 20 miles of a training center. The lack of training center implementation means that checkdistance cannot be used as intended. Creating the training center will increase the use of the code.

[Tenant]: Tenant.md
[Lodging]: Lodging.md
