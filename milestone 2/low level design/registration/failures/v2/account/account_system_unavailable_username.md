# As a user I want to register to create an account but system fails to distribute system wide username 
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account 
    activate UI 
    UI ->> Entry Point: accountView(view:obj): obj
    activate Entry Point
    Entry Point ->> Manager: isValid(email:string,pw:string): bool
    activate Manager
    Manager ->> Services: fetch(email:string):bool
    activate Services
    Services ->> DataAccess: getEmail(email:string):bool 
    activate DataAccess
    DataAccess ->> DataStore: insertCredentials(email:string,password:string):bool
    activate DataStore
    DataStore -->> DataAccess: return False
    deactivate DataStore
    DataAccess -->> Services: return False
    deactivate DataAccess
    Services -->> Manager: return False
    deactivate Services
    Manager -->> Entry Point: return False
    deactivate Manager
    Entry Point -->> UI: return account view obj
    deactivate Entry Point
    UI -->> User: return unable to give system wide username
    deactivate UI
```