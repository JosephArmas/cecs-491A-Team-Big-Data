# As a user I cannot register to create an account in a different view 
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account
    activate UI
    UI ->> UI: checkView(view: obj):int
    UI -->> UI: return 0
    UI -->> User: return user account creatino view to create account
    deactivate UI
```