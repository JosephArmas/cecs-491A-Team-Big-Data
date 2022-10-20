# As a user I cannot register to create an account in a different view 
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account
    activate UI
    UI ->> BuisnessLogic: checkView(view:obj):bool
    activate BuisnessLogic
    BuisnessLogic -->> UI: return False
    deactivate BuisnessLogic
    UI -->> User: return user account creatino view to create account
    deactivate UI
```