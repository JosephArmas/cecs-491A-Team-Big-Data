# as a user I cannot reister to create an account without a valid email
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account
    activate UI
    UI ->> BuisnessLogic: isValid(email:string):bool
    activate BuisnessLogic
    BuisnessLogic ->> Services: fetch(email:string):bool
    activate Services
    Services ->> DataAccess: getEmail(email:string):bool
    activate DataAccess
    DataAccess ->> DataStore: search(email:string):bool
    activate DataStore
    DataStore -->> DataAccess: return False
    deactivate DataStore
    DataAccess -->> Services: retrun False
    deactivate Services
    Services -->> BuisnessLogic: return False
    BuisnessLogic -->> UI: return False
    deactivate BuisnessLogic
    UI -->> User: return Invalid email
    deactivate UI
```