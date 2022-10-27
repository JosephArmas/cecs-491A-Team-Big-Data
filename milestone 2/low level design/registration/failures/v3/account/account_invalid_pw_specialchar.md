# As a user I cannot register to create an account with a passphrase that consist with special characters out of the scope requirements
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account
    activate UI
    UI ->> Entry Point: https request
    activate Entry Point
    Entry Point ->> Registration Manager: isValid(pw:string): int
    activate Registration Manager
    Registration Manager -->> Entry Point: return 0 
    deactivate Registration Manager
    Entry Point -->> UI: https response
    UI -->> User: return passphrase valid special characters are: (.,@!-)
    deactivate UI
```