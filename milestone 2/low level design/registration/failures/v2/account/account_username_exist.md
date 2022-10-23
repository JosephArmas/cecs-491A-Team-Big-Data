# As a user I cannot register to create an account with an existing username 
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account
    activate UI
    UI ->> Entry Point: accountView(view:obj): obj
    activate Entry Point
    Entry Point ->> Manager: isValid(email:string,pw:string):bool
    activate Manager
    Manager ->> Services: fetch(email:string):bool
    activate Services
    Services ->> DataAccess: getEmail(e:string):bool 
    activate DataAccess
    DataAccess ->> DataStore: searchUser(email:string):string
    activate DataStore
    DataStore -->> DataAccess: return username
    deactivate DataStore
    DataAccess -->> Services: return True
    deactivate DataAccess
    Services -->> Manager: return True
    deactivate Services
    Manager -->> Entry Point: return True
    deactivate Manager
    Entry Point -->> UI: return account view obj
    deactivate Entry Point
    UI -->> User: return user already exist 
    deactivate UI
```