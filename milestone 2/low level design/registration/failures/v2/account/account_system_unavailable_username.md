# As a user I want to register to create an account but system fails to distribute system wide username 
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account 
    activate UI 
    UI ->> BuisnessLogic: isValid(email:string,pw:string): bool
    activate BuisnessLogic
    BuisnessLogic ->> Services: fetch(email:string):bool
    activate Services
    Services ->> DataAccess: getEmail(email:string):bool 
    activate DataAccess
    DataAccess ->> DataStore: insertCredentials(email:string,password:string):bool
    activate DataStore
    DataStore -->> DataAccess: return False
    deactivate DataStore
    DataAccess -->> Services: return False
    deactivate DataAccess
    Services -->> BuisnessLogic: return False
    deactivate Services
    BuisnessLogic -->> UI: return False
    deactivate BuisnessLogic
    UI -->> User: return unable to give system wide username
```