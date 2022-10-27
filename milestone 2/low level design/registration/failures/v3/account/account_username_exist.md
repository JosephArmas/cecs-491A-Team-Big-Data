# As a user I cannot register to create an account with an existing username 
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account
    activate UI
    UI ->> Entry Point: https request
    activate Entry Point
    Entry Point ->> Manager: isValid(email:string,pw:string):int
    activate Manager
    Manager ->> Services: fetch(email:string):int
    activate Services
    Services ->> DataAccess: getEmail(e:string):int 
    activate DataAccess
    DataAccess ->> DataStore: searchUser(email:string):byte
    activate DataStore
    DataStore -->> DataAccess: return 1
    deactivate DataStore
    DataAccess -->> Services: return 1
    deactivate DataAccess
    Services -->> Manager: return 1
    deactivate Services
    Manager -->> Entry Point: return 1
    deactivate Manager
    Entry Point -->> UI: return https response
    deactivate Entry Point
    UI -->> User: return user already exist 
    deactivate UI
```