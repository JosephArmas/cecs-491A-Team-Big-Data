# As a user I cannot register to create an account with an existing email 
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account
    activate UI
    UI ->> UI: User(email:string, pw: string): obj
    UI ->> EntryPoint: {https ajax post request}: json
    activate EntryPoint
    EntryPoint ->> RegManager: SendInput(userInput: obj):obj
    activate RegManager
    RegManager ->> DataAccess: EmailExist(email: obj):obj
    activate DataAccess
    DataAccess ->> MySQL: Search(email:obj):obj
    activate MySQL
    MySQL -->> DataAccess: return MySQL obj
    deactivate MySQL
    DataAccess -->> RegManager: return DataAccess obj 
    deactivate DataAccess
    RegManager -->> EntryPoint: return RegManager obj 
    deactivate RegManager
    EntryPoint -->> UI: https response
    deactivate EntryPoint
    UI -->> User: return "email already exist, would you like to use another email" 
    deactivate UI
```