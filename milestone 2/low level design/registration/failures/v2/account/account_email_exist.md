# As a user I cannot register to create an account with an existing email 
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account
    activate UI
    UI ->> BuisnessLogic: isValid(email:string,pw:string):bool
    activate BuisnessLogic
    BuisnessLogic ->> Services: fetch(email:string):bool
    activate Services
    Services ->> DataAccess: getEmail(e:string):bool 
    activate DataAccess
    DataAccess ->> DataStore: search(email:string):bool
    activate DataStore
    DataStore -->> DataAccess: return False
    deactivate DataStore
    DataAccess -->> Services: return False
    deactivate DataAccess
    Services -->> BuisnessLogic: return False
    deactivate Services
    BuisnessLogic -->> UI: return False
    deactivate BuisnessLogic
    UI -->> User: return email already exist 
    deactivate UI
```