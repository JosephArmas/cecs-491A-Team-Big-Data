# As a user I cannot register to create an account if there exist an active session
```mermaid
sequenceDiagram
    actor User
    User ->> Security: create an account 
    activate Security
    Security ->> UI: userActive(user:obj): bool
    activate UI 
    UI -->> Security: return True 
    deactivate UI
    Security -->> User: return user is currently active.
    deactivate Security
```