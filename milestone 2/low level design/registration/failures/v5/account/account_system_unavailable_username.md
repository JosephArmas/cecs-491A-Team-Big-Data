# As a user I want to register to create an account but system fails to distribute system wide username 
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account 
    activate UI
    UI ->> UI: User(email:string, password:string): obj
    UI ->> EntryPoint: {https ajax post request}: json
    activate EntryPoint
    EntryPoint ->> EntryPoint: create user obj
    EntryPoint ->> RegManager: SendInput(user: obj): obj
    activate RegManager
    RegManager ->> RegManager: create email obj
    RegManager ->> DataAccess: EmailExist(email:obj): obj
    activate DataAccess
    DataAccess ->> MySQL: InsertCredentials(user: obj): table
    activate MySQL
    MySQL -->> DataAccess: return string vlaue in email column 
    DataAccess ->> DataAccess: UserEmail(email: string): obj
    DataAccess ->> DataAccess: create emailExist obj
    DataAccess -->> RegManager: return emailExist obj
    RegManager ->> RegManager: GenUsername(emailExist: obj): obj
    RegManager ->> DataAccess: UserExist(genUserName:obj): obj
    DataAccess ->> MySQL: InserUser(userName: obj): table
    MySQL -->> DataAccess: return username column 
    deactivate MySQL
    DataAccess ->> DataAccess: create Result obj
    DataAccess -->> RegManager: return Result
    deactivate DataAccess
    RegManager -->> EntryPoint: return Result 
    deactivate RegManager 
    EntryPoint -->> UI: return https response: code 200
    deactivate EntryPoint
    UI -->> User: return "unable to give system wide username, please try again"
    deactivate UI
```