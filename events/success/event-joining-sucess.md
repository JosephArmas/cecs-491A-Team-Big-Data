# As a User type Reputable, Service or Regular, I can join an event 
```mermaid
sequenceDiagram
    actor User
    participant map.js
    participant events.js
    participant EntryPoint
    participant EventManager
    participant EventService
    participant DataAccess
    participant DataStore
    note over User: User must have an authenticated session
    User->>map.js: User cliks on an event pin on the map
    map.js->>events.js: joinEvent(user, role): obj
    events.js->>events.js: checkUser(role): bool
    events.js->>events.js: return true
    events.js->>EntryPoint: axios.post(endpoint,configs,data)
    EntryPoint->>EventManager: try{call method from manager}catch
    EventManager->>EventManager: checkEventData(data): bool
    EventManager->>EventManager: checkUser(UserProfile profile): bool
    EventManager->>EventService: JoinEvent(EntityModel): obj //logging occurs
    EventService->>DataAccess: IDBInsert dao = new SqlDAO(string conString)
    DataAccess->>DataStore: Sql to store a user joing an event and increment count
    DataStore-->>DataAccess: return raw data User | Count | title | description | Event ID 
    DataAccess->>DataAccess: convert into entity model
    DataAccess-->>EventService: return Response
    EventService-->>EventManager: return Response
    EventManager-->>EntryPoint: return entitymodel join event
    EntryPoint-->>events.js: return json Obj from axios {User: , Count: , title: , description: , EventId:}
    events.js->>events.js: check data return
    events.js-->>map.js: return join event data
    map.js-->>User: "Sucessfully joined event"

```