# As a user I cannot register to create an account with a passphrase of length less than 8 
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account
    activate UI
    Note over UI: create an account
    UI ->> BuisnessLogic: isValid(pw:string):bool
    activate BuisnessLogic
    BuisnessLogic -->> UI: return False 
    deactivate BuisnessLogic
    UI -->> User: return passphrase must be minimum of 8 characters
    deactivate UI
```