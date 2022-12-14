# As a user I cannot register to create an account with an existing username 
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account
    activate UI
    UI ->> UI: User(email:string, password:string): obj 
    UI ->> EntryPoint: {https ajax post request}: json
    activate EntryPoint
    EntryPoint ->> Register: SendInput(email: string, password: string) :obj
    activate Register
    Register ->> Register: IsValidEmail(email:string): bool
    Register -->> Register: return true
    Register ->> Register: IsValidPassword(password:string): bool
    Register -->> Register: return true
    Register ->> Register: SetUserName(newUserName: string): bool 
    Register -->> Register: return true
    Register -->> SqlDAO: InsertUser(table: string, email: string, password: string): obj
    activate SqlDAO
    SqlDAO -->> MySQL: Insert(tableName: string, collumnNames: string [], values: string [] ): scalar
    activate MySQL
    MySQL -->> SqlDAO: return string value in username column 
    deactivate MySQL
    SqlDAO -->> Register: return result.isSuccessful() 
    deactivate SqlDAO
    Register -->> EntryPoint: return result.isSuccessful() 
    deactivate Register
    EntryPoint -->> UI: https response: code 200
    deactivate EntryPoint
    UI -->> User: return "user already exist"
    deactivate UI
```