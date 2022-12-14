# As a user I cannot register to create an account with an existing username 
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account
    activate UI
    UI ->> UI: User(email:string, password:string): obj 
    UI ->> Entry Point: {https ajax post request}: json
    activate Entry Point
    Entry Point ->> RegManager: SendInput(userInput:obj):obj
    activate RegManager
    RegManager ->> DataAccess: IsValid(userInput:obj):obj
    activate DataAccess
    DataAccess ->> MySQL: InsertCredentials(user:obj):obj
    activate MySQL
    MySQL ->> MySQL: Insert(user: obj)
    MySQL -->> DataAccess: return MySQL obj
    RegManager ->>  RegManager: GenUsername(userName:string): obj
    RegManager ->> DataAccess: UserExist(userName:obj):obj
    DataAccess ->> MySQL: InsertUsername(userName:obj):obj
    MySQL -->> MySQL: Insert(userName: obj): obj
    MySQL -->> DataAccess: return MySQL obj
    deactivate MySQL
    DataAccess -->> RegManager: return DataAccess obj 
    deactivate DataAccess
    RegManager -->> Entry Point: return Registration Manager obj
    deactivate RegManager
    Entry Point -->> UI: https response
    deactivate Entry Point
    UI -->> User: return "user already exist"
    deactivate UI
```