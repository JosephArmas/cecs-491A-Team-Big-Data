# As a system, I can check if a user is disabled
``` mermaid
sequenceDiagram
    actor User
    User ->> UI: authenticate user
    activate UI
    UI ->> UI: User(username: string, password: string): obj
    UI ->> EntryPoint: {https ajax post request}: json
    activate EntryPoint
    EntryPoint ->> Auth: SendInput(username:string, password: string): obj
    activate Auth
    Auth ->> Auth: IsValid(username: string, password: string): bool
    Auth -->> Auth: return true
    Auth ->> Auth: Attempts(trys: bool): int
    Auth -->> Auth: return integer value of Attempts method
    Auth ->> Auth: IsDisbled(username: string, password: string): obj
    Auth -->> EntryPoint: return instance result.IsSuccesul() 
    deactivate Auth
    EntryPoint -->> UI: https response: code 200
    deactivate EntryPoint
    UI -->> User: return "Account disabled. Perform account recovery or contact system admin"
    deactivate UI
```