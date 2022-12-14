# As a user I cannot register to create an account with a passphrase of length less than 8 
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account
    activate UI
    UI ->> UI: User(email: string, password: string): obj
    UI ->> EntryPoint: {https ajax post request}: json
    activate EntryPoint
    EntryPoint ->> EntryPoint: create pw obj
    EntryPoint ->> RegManager: SendInput(pw:obj):obj
    activate RegManager
    RegManager ->> RegManager: IsValid(pw:obj):obj
    RegManager ->> RegManager: create Result obj
    RegManager -->> EntryPoint: return Result 
    deactivate RegManager
    EntryPoint -->> UI: https response: code 200
    deactivate EntryPoint
    UI -->> User: return "passphrase must be minimum of 8 characters"
    deactivate UI
```