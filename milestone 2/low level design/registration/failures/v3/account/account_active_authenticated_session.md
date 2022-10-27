# As a user I cannot register to create an account if there exist an active session
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account 
    activate UI
    UI ->> Security: userActive(user:obj): int
    activate Security
    Security -->> UI: return 1 
    UI -->> User: return user is currently active.
    deactivate UI 
```