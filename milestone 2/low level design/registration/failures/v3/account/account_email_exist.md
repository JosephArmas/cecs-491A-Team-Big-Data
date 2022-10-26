# As a user I cannot register to create an account with an existing email 
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account
    activate UI
    UI ->> Entry Point: https request 
    activate Entry Point
    Entry Point ->> Manager: isValid(email:string,pw:string):bool
    activate Manager
    Manager ->> Services: EmailExist(email:string):bool
    activate Services
    Services ->> DataAccess: getEmail(e:string):bool 
    activate DataAccess
    DataAccess ->> DataStore: search(email:string): byte
    activate DataStore
    DataStore -->> DataAccess: return 1
    deactivate DataStore
    DataAccess -->> Services: return True
    deactivate DataAccess
    Services -->> Manager: return True
    deactivate Services
    Manager -->> Entry Point: return True
    deactivate Manager
    Entry Point -->> UI: https response
    deactivate Entry Point
    UI -->> User: return email already exist, would you like to use another email 
    deactivate UI
```