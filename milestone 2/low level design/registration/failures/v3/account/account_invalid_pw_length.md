# As a user I cannot register to create an account with a passphrase of length less than 8 
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account
    activate UI
    UI ->> Entry Point: https request
    activate Entry Point
    Entry Point ->> Registration Manager: isValid(pw:string):int
    activate Registration Manager
    Registration Manager -->> Entry Point: return 0 
    deactivate Registration Manager
    Entry Point -->> UI: https response
    deactivate Entry Point
    UI -->> User: return passphrase must be minimum of 8 characters
    deactivate UI
```