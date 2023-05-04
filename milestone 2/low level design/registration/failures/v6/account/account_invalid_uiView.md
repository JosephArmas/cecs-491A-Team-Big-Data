# As a user I cannot register to create an account in a different view 
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account
    activate UI
    UI ->> UI: CheckView(view:obj):obj
    UI ->> UI: return View instance
    UI -->> User: return "user account creation view to create account"
    deactivate UI
```