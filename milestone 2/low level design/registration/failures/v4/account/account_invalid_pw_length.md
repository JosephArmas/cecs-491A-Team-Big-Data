# As a user I cannot register to create an account with a passphrase of length less than 8 
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account
    activate UI
    UI ->> UI: User(email: string, password: string): obj
    UI ->> Entry Point: {https ajax post request}: json
    activate Entry Point
    Entry Point ->> Registration Manager: SendInput(userInput:obj):obj
    activate Registration Manager
    Registration Manager ->> Registration Manager: IsValid(pw:obj):obj
    Registration Manager -->> Entry Point: return RegManager obj 
    deactivate Registration Manager
    Entry Point -->> UI: https response
    deactivate Entry Point
    UI -->> User: return "passphrase must be minimum of 8 characters"
    deactivate UI
```