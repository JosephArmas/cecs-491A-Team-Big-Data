# As a user, I cannot authenticate myself with an invalid username 
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
    Auth -->> Auth: return false
    Auth -->> EntryPoint: return result.isSuccessful()
    deactivate Auth
    EntryPoint -->> UI: https response: code 200
    deactivate EntryPoint
    UI -->> User: return "invalid username or password provided. Retry again or contact system administrator"
    deactivate UI
```