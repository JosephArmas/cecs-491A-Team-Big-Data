# As a user I cannot authenticate inputting the worong OTP code 
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
    Auth ->> Auth: GenCode(): obj
    Auth -->> EntryPoint: return instance of code
    EntryPoint -->> UI: https response: code 200
    UI -->> User: return code.display()
    UI ->> UI: UserCode(code: string): obj
    UI ->> EntryPoint: {https ajax post request}: json
    EntryPoint ->> Auth: SendCode(code: string): obj
    Auth ->> Auth: CodeValid(code): bool
    Auth -->> Auth: return false
    Auth -->> EntryPoint: return result.isSuccessful()
    deactivate Auth
    EntryPoint -->> UI: https response: code 200
    deactivate EntryPoint
    UI -->> User: return "Invalid username or password. Retry again or contact system admin"
    deactivate UI


```