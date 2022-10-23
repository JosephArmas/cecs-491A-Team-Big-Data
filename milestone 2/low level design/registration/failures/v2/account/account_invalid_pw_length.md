# As a user I cannot register to create an account with a passphrase of length less than 8 
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account
    activate UI
    UI ->> Entry Point: accountView(view:obj) :obj
    activate Entry Point
    Entry Point ->> Manager: isValid(pw:string):bool
    activate Manager
    Manager -->> Entry Point: return False 
    deactivate Manager
    Entry Point -->> UI: return account view obj
    deactivate Entry Point
    UI -->> User: return passphrase must be minimum of 8 characters
    deactivate UI
```