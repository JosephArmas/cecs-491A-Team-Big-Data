# As a user I cannot reister to create an account without a valid email
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account
    activate UI
    UI ->> UI: User(email: string, password: string): obj
    UI ->> EntryPoint: {https ajax post request}: json
    activate EntryPoint
    EntryPoint ->> Register: SendInput(email:string, password: string):obj
    activate Register
    Register ->> Register: IsValidEmail(email:string):obj
    Register -->> EntryPoint: return result.isSuccessful()
    deactivate Register
    EntryPoint -->> UI: https response: code 200
    deactivate EntryPoint
    UI -->> User: return "preconditions for email not met, please enter new email"
    deactivate UI
```