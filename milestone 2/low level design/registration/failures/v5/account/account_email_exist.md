# As a user I cannot register to create an account with an existing email 
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account
    activate UI
    UI ->> UI: User(email:string, pw: string): obj
    UI ->> EntryPoint: {https ajax post request}: json
    activate EntryPoint
    EntryPoint ->> EntryPoint: create userInput obj
    EntryPoint ->> RegManager: SendInput(userInput: obj):obj
    activate RegManager
    RegManager ->> RegManager: create email obj
    RegManager ->> DataAccess: EmailExist(email: obj):obj
    activate DataAccess
    DataAccess ->> MySQL: Search(email:obj): table
    activate MySQL
    MySQL -->> DataAccess: return email column 
    DataAccess ->> DataAccess: UserEmail(email: string): obj
    DataAccess ->> DataAccess: create Result object
    deactivate MySQL
    DataAccess -->> RegManager: return Result 
    deactivate DataAccess
    RegManager -->> EntryPoint: return Result 
    deactivate RegManager
    EntryPoint -->> UI: https response: code 200
    deactivate EntryPoint
    UI -->> User: return "email already exist, would you like to use another email" 
    deactivate UI
```