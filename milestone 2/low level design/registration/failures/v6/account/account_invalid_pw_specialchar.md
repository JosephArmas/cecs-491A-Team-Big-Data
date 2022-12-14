# As a user I cannot register to create an account with a passphrase that consist with special characters out of the scope requirements
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account
    activate UI
    UI ->> EntryPoint: {https ajax post request}: json
    activate EntryPoint
    EntryPoint ->> EntryPoint: create pw obj
    EntryPoint ->> RegManager: SendInput(pw:string): obj
    activate RegManager
    RegManager ->> RegManager: IsValidPassword(pw:string): obj
    RegManager -->> EntryPoint: return result.isSuccessful()  
    deactivate RegManager
    EntryPoint -->> UI: https response: code 200
    UI -->> User: return "passphrase valid special characters are: (.,@!-)"
    deactivate UI
```