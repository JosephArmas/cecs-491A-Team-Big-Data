# as a user I can not register to create an account without username requirements 
```mermaid
sequenceDiagram
    actor User
    User ->> UI: start
    activate UI
    Note over UI: create an account
    UI ->> BuisnessLogic: isValid(userName:string):bool
    activate BuisnessLogic
    BuisnessLogic -->> UI: username requirements not met 
    deactivate BuisnessLogic
    UI -->> User: False
    deactivate UI
```