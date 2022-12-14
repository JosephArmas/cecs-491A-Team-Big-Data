# As a user I cannot register to create an account with an existing email 
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account
    activate UI
    UI ->> UI: User(email:string, password: string): obj
    UI ->> EntryPoint: {https ajax post request}: json
    activate EntryPoint
    EntryPoint ->> AccountManager: SendInput(email: string, password: string):obj
    activate AccountManager
    AccountManager ->> AccountManager: IsValidEmail(email:string):bool
    AccountManager -->> AccountManager: return true
    AccountManager ->> AccountManager: IsValidPassword(password:string):bool
    AccountManager -->> AccountManager: return true
    AccountManager -->> SqlDAO: InsertUser(table: string, email: string, password: string): obj
    activate SqlDAO
    SqlDAO -->> MySQL: Insert(tableName: string, collumnNames: string [], values: string [] ): scalar
    activate MySQL
    MySQL -->> SqlDAO: return email column 
    deactivate MySQL
    SqlDAO -->> AccountManager: return result.isSuccessful()
    deactivate SqlDAO
    AccountManager -->> EntryPoint: return result.isSuccessful() 
    deactivate AccountManager
    EntryPoint -->> UI: https response: code 200
    deactivate EntryPoint
    UI -->> User: return "email already exist, would you like to use another email" 
    deactivate UI
```