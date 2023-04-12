# As a Reputable, Service, Regular User or Admin, I can see the attendance of an event pin
```mermaid
sequenceDiagram
    actor User
    participant map.js
    participant events.js
    participant EntryPoint
    participant EventManager
    participant EventService
    participant Response
    participant Logger
    participant DataAccess
    participant DataStore
    note over User: User must have an authenticated session
    User->>+map.js: User clicks on event pin
    map.js->>map.js: checkUser(user: obj): bool
    map.js->>map.js: return true
    map.js->>+events.js: displayEventPin(pinPos)
    events.js-->>-map.js: return build function  
    map.js-->>-User: returns "show attendance button"
    User->>+map.js: User clicks on "show attendance"
    map.js->>+events.js: getAttendance(pinEvent obj): int
    events.js->>+EntryPoint: axios.post(string: endPoint, obj: eventDescription, obj: configs)
    EntryPoint->>+EventManager: app.Run()
    EventManager-->>EventManager: GetEvent(int eventID, string userHash, UserProfile userProfile): Task<Response> obj
    EventManager-->>+EventService: GetEventCount(int eventID, string userHash): Task<Response> obj
    EventService->>+Logger: Logger logger = new Logger()
    Logger->>-EventService: return logger obj
    EventService->>+DataAccess: SelectEventCount(int eventID): Task<Response> obj
    DataAccess-->>+DataStore: ExecNonQuery
    note right of DataStore: exec sql
    DataStore-->>-DataAccess: return 1 
    DataAccess-->>-EventService: return response obj
    EventService-->>+DataStore: log = new log()<br> logger.log(logId,CorrelationId,Loglevel,UserHash,User,TimeStamp,Event,Category,Message)
    note right of DataStore: exec sql
    DataStore-->>-EventService: return 1
    EventService->>EventService: CheckCount(Response response): bool
    EventService-->>-EventManager: return response obj
    EventManager-->>-EntryPoint: return response obj
    EntryPoint-->>-events.js: return axios json obj
    events.js->>events.js: checkCount(json obj): bool
    events.js-->>-map.js: return json obj 
    map.js->>map.js: parse json obj {"count": }
    map.js-->>-User: return show number of attendees
    

```