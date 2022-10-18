# as a user I can not register to create an account without password requirements 
```mermaid
sequenceDiagram
    actor User
    User ->> UI: start
    activate UI
    Note over UI: create an account
    UI ->> BuisnessLogic: isValid(pw:string):bool
    activate BuisnessLogic
    BuisnessLogic -->> UI: password requirement not met 
    deactivate BuisnessLogic
    UI -->> User: False
    deactivate UI


```