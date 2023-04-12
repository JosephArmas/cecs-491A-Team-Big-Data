# As a Reputable User or Admin, I can modify the description of an event but it fails
```mermaid
sequenceDiagram
    actor User
    participant map.js
    participant events.js
    participant EntryPoint
    participant EventManager
    participant EventService
    participant Logger
    participant DataAccess
    participant DataStore
    note over User: User must have an authenticated session
    User->>+map.js: User clicks on event pin
    map.js->>map.js: checkUser(user: obj): bool
    map.js->>map.js: return true
    map.js->>+events.js: buildModifyEvent(): void
    events.js-->>-map.js: return a prompt 
    map.js-->>-User: returns view of title, description or cancel event
    User->>+map.js: User clicks or inputs keyboard action for description
    map.js->>map.js: isValidTitle(string: title): bool
    map.js->>map.js: return true
    map.js->>+events.js: sendEventDescription(description obj)
    events.js->>+EntryPoint: axios.post(string: endPoint, obj: eventDescription, obj: configs)
    EntryPoint->>+EventManager: app.Get()
    EventManager-->>EventManager: CheckEventDescription(string description, string userHash): Task<Response> obj
    note right of EventManager: validate input and check for role (authorization)
    EventManager-->>+EventService: UpdateEventDescription(string description, string userHash): Task<Response> obj
    EventService->>+Logger: var logger = new logger()
    Logger->>-EventService: return logger obj
    EventService-->>+DataAccess: UpdateEventTableDescription(string description): Task<Respone> obj
    DataAccess-->>+DataStore: ExecuteNonQuery
    note right of DataStore: sql exec
    DataStore-->>-DataAccess: return 1
    DataAccess-->>EventService: return response obj
    EventService->>EventService:IdValidDescription(response obj): Task<Response>
    EventService-->>+DataStore: log = new log()<br> logger.log(logId,CorrelationId,Loglevel,UserHash,User,TimeStamp,Event,Category,Message)
    note right of DataStore: sql executed
    DataStore-->>-EventService: return 1
    EventService-->>-EventManager: retrun response obj
    EventManager-->>-EntryPoint: return response obj
    EntryPoint-->>-events.js: return axios json obj
    events.js->>events.js: check json obj
    events.js->>-events.js: return true
    events.js-->>map.js: return data.errorMessage
    map.js-->>-User: display "Event description did not update. Please try again"
    

```