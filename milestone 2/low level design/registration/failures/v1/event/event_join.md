# as a user I can not register to join an event that has max capacity of 100
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
    DataStore -->> UI: event does not exist
    deactivate DataStore
    deactivate DataAccess
    deactivate BuisnessLogic
    UI -->> User: False
    deactivate UI
```