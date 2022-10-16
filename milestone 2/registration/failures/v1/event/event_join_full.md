# as a user I can not register to join an event that is full
```mermaid
sequenceDiagram
    actor User
    User ->> UI: start
    activate UI
    Note over UI: join event
    UI ->> BuisnessLogic: join(e:string):bool
    activate BuisnessLogic
    BuisnessLogic ->> DataAccess: getEvent(e:string):bool
    activate DataAccess
    DataAccess ->> DataStore: eventExist(e:string):bool
    activate DataStore
    DataStore -->> DataAccess: event exist
    deactivate DataStore
    DataAccess -->> BuisnessLogic: eventLimit(n:int):bool
    deactivate DataAccess
    BuisnessLogic -->> UI: event full
    deactivate BuisnessLogic
    UI -->> User: False
    deactivate UI
```