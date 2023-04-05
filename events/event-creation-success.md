# As a Reputable User, I can create an event
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
    note over User: User must have an authenticated 
    User->>map.js: User clicks on map
    map.js->>map.js: checkUser(role,pinType) {} 
    map.js->>map.js: return true
    map.js->>events.js: showEventInputs()
    events.js-->>map.js: return a view of event creation 
    map.js-->>User: shows title, description input fields and post, cancel buttons 
    User ->> map.js: User fills out title, description and hits post button 
    map.js ->> events.js: isValidEventInputs(eventData obj)
    events.js->>events.js: return true
    events.js->>EntryPoint: sendEventsData(eventData obj){axios.post(endPoint,configs,eventData)}



    


    
    



```
