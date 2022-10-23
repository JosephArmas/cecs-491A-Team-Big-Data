# As a user I cannot register to create an account with an existing email 
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account
    activate UI
    UI ->> Entry Point: create an accountView(view:obj): obj
    activate Entry Point
    Entry Point ->> Manager: isValid(email:string,pw:string):bool
    activate Manager
    Manager ->> Services: fetch(email:string):bool
    activate Services
    Services ->> DataAccess: getEmail(e:string):bool 
    activate DataAccess
    DataAccess ->> DataStore: search(email:string):bool
    activate DataStore
    DataStore -->> DataAccess: return False
    deactivate DataStore
    DataAccess -->> Services: return False
    deactivate DataAccess
    Services -->> Manager: return False
    deactivate Services
    Manager -->> Entry Point: return False
    deactivate Manager
    Entry Point -->> UI: return account View obj
    UI -->> User: return email already exist 
    deactivate UI
```