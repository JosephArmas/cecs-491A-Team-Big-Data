# As a user I cannot reister to create an account inserting a null string
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account
    activate UI
    UI ->> Entry Point: https request
    activate Entry Point
    Entry Point ->> Manager: isValid(email:string):bool
    activate Manager
    Manager ->> Services: isNull(email:string): bool
    activate Services
    Services -->> Manager: return True
    deactivate Services
    Manager -->> Entry Point: return False
    deactivate Manager
    Entry Point -->> UI: https response
    deactivate Entry Point
    UI -->> User: return preconditions for email not met, please enter new email
    deactivate UI
```