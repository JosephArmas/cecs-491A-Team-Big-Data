# as a user I cannot register to create an event as a regular user type

```mermaid
sequenceDiagram
    actor User
    User ->> UI: start
    activate UI
    Note over UI: create event 
    UI ->> BuisnessLogic: createEvent(title:string,description):bool
    activate BuisnessLogic
    BuisnessLogic -->> UI: invalid title or description
    deactivate BuisnessLogic    
    UI -->> User: False
    deactivate UI
```