# as a user I can not register to create an account with a username taken
```mermaid
sequenceDiagram
    actor User
    User ->> UI: start
    activate UI
    Note over UI: create an account
    UI ->> BuisnessLogic: credentials(userName:string,pw:string):bool
    activate BuisnessLogic
    BuisnessLogic ->> DataAccess: userNameExist(userName:string):bool
    activate DataAccess
    DataAccess ->> DataStore: search(userName:string):bool 
    activate DataStore
    DataStore -->> UI: username already exist 
    deactivate DataAccess
    deactivate DataStore
    deactivate BuisnessLogic
    UI -->> User: False
    deactivate UI
```