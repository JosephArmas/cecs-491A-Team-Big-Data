# as a user I cannot register to create an account with a exsisting username 
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
    DataAccess ->> DataStore: searchUser(email:string):string
    activate DataStore
    DataStore -->> DataAccess: return username
    deactivate DataStore
    DataAccess -->> Services: return True
    deactivate DataAccess
    Services -->> BuisnessLogic: return True
    deactivate Services
    BuisnessLogic -->> UI: return True
    deactivate BuisnessLogic
    UI -->> User: return user already exist 
    deactivate UI
```