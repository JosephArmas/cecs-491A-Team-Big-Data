# As a user I cannot register to create an account with a passphrase that consist with special characters out of the scope requirements
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account
    activate UI
    UI ->> EntryPoint: {https ajax post request}: json
    activate EntryPoint
    EntryPoint ->> EntryPoint: create pw obj
    EntryPoint ->> RegManager: SendInput(pw:obj): obj
    activate RegManager
    RegManager ->> RegManager: IsValid(pw:obj): obj
    RegManager ->> RegManager: create Result obj 
    RegManager -->> EntryPoint: return Result 
    deactivate RegManager
    EntryPoint -->> UI: https response: code 200
    UI -->> User: return "passphrase valid special characters are: (.,@!-)"
    deactivate UI
```