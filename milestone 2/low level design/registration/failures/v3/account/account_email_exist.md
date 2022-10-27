# As a user I cannot register to create an account with an existing email 
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account
    activate UI
    UI ->> Entry Point: https request 
    activate Entry Point
    Entry Point ->> Registration Manager: isValid(email:string,pw:string):int
    activate Registration Manager
    Registration Manager ->> Registrations Services: EmailExist(email:string):int
    activate Registrations Services
    Registrations Services ->> DataAccess: getEmail(e:string):int 
    activate DataAccess
    DataAccess ->> MySQL: search(email:string): byte
    activate MySQL
    MySQL -->> DataAccess: return 1
    deactivate MySQL
    DataAccess -->> Registrations Services: return 1
    deactivate DataAccess
    Registrations Services -->> Registration Manager: return 1
    deactivate Registrations Services
    Registration Manager -->> Entry Point: return 1
    deactivate Registration Manager
    Entry Point -->> UI: https response
    deactivate Entry Point
    UI -->> User: return email already exist, would you like to use another email 
    deactivate UI
```