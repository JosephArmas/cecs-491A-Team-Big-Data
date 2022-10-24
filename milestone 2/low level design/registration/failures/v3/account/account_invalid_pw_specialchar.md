# As a user I cannot register to create an account with a passphrase that consist with special characters out of the scope requirements
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account
    activate UI
    UI ->> Entry Point: accountView(view:obj): obj
    activate Entry Point
    Entry Point ->> Manager: isValid(pw:string):bool
    activate Manager
    Manager -->> Entry Point: return False 
    deactivate Manager
    Entry Point -->> UI: return account view
    UI -->> User: return passphrase valid special characters are: (.,@!-)
    deactivate UI
```