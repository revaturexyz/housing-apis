# Address Service API Documentation
  The Address Service API deals with Addresses that are set up within the database. We are able to receive addresses from within the database, add new addresses that exist in the real world, and check the distances between two addresses. These addresses can be used with the Lodging and Tenant service.

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
  - Finds the first address that has an id value equal to the provided address id.
  - Used to find specific details of an address, as only the address id is stored in Lodging and Tenant. 
  - Consumed by the Tenant and Lodging services.
 ```
 api/address/checkdistance
 ```
 - Three parameters are provided. There is a "to" address, a "from" address, and a "distance" int that is set to 20.
 - GET:
    - Finds out if the two provided addresses are within a specific distance of each other. This is usually set to 20 miles.
    - Returns true if within the provided number of miles, and false if the addresses are farther than the provided number of miles.
    - Consumed by the Tenant and Lodging services.
  ```
  api/address
  ```
  - One parameter is provided, that being an address.
  - GET:
    - Finds out if the provided address exists within the database.
    - If the address exists in the database, the address id is returned.
    - Otherwise, the method checks the Google API for the address.
  - POST:
    - If the address exists with the Google API, the address is added to the database, and the address id is returned.
    - Consumed by the Tenant and Lodging services.
   
## Known issues
1. Training Center. The training center is not implemented as of right now. While checkdistance can be used on any two addresses, service method can be used to ensure that a specific address is within 20 miles of a training center. The lack of training center implementation means that checkdistance cannot be used as intended. Creating the training center will increase the functionality of the code.
