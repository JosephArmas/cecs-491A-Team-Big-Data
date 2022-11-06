# As a user I cannot register to create an account with an existing username 
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account
    activate UI
    UI ->> UI: User(email:string, password:string): obj 
    UI ->> EntryPoint: {https ajax post request}: json
    activate EntryPoint
    EntryPoint ->> EntryPoint: create user obj
    EntryPoint ->> RegManager: SendInput(user:obj):obj
    activate RegManager
    RegManager ->> RegManager: create email obj
    RegManager ->> DataAccess: EmailExist(email:obj):obj
    activate DataAccess
    DataAccess ->> MySQL: InsertCredentials(email:obj): table
    activate MySQL
    MySQL -->> DataAccess: return string value in email column 
    DataAccess ->> DataAccess: UserEmail(email: string): obj
    DataAccess ->> DataAccess: create emailExist obj
    DataAccess -->> RegManager: return emailExist 
    RegManager ->>  RegManager: GenUsername(emailExist:obj): obj 
    RegManager ->> DataAccess: UserExist(genUserName:obj):obj
    DataAccess ->> MySQL: InsertUsername(genUserName:obj):table
    MySQL -->> DataAccess: return value in username column 
    deactivate MySQL
    DataAccess ->> DataAccess: create Result obj
    DataAccess -->> RegManager: return Result 
    deactivate DataAccess
    RegManager -->> EntryPoint: return Result
    deactivate RegManager
    EntryPoint -->> UI: https response: code 200
    deactivate EntryPoint
    UI -->> User: return "user already exist"
    deactivate UI
```