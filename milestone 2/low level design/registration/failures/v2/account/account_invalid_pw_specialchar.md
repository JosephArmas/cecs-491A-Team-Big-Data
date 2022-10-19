# as a user I cannot register to create an account with a passphrase that consist with special characters out of the scope requirements
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account
    activate UI
    UI ->> BuisnessLogic: isValid(pw:string):bool
    activate BuisnessLogic
    BuisnessLogic -->> UI: return False 
    deactivate BuisnessLogic
    UI -->> User: return passphrase valid special characters are: (.,@!-)
    deactivate UI
```