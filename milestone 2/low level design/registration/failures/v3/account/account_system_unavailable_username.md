# As a user I want to register to create an account but system fails to distribute system wide username 
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account 
    activate UI 
    UI ->> Entry Point: https request 
    activate Entry Point
    Entry Point ->> Registration Manager: isValid(email:string,pw:string): int
    activate Registration Manager
    Registration Manager ->> Registration Services: EmailExist(email:string):int
    activate Registration Services
    Registration Services ->> DataAccess: getEmail(email:string):int 
    activate DataAccess
    DataAccess ->> MySQL: insertCredentials(email:string,password:string): byte
    activate MySQL
    MySQL -->> DataAccess: return 1
    deactivate MySQL
    DataAccess -->> Registration Services: return 1
    deactivate DataAccess
    Registration Services -->> Registration Manager: return 1
    deactivate Registration Services
    Registration Manager -->> Entry Point: return 1
    Entry Point ->> Registration Manager: genUsername(usernamge:string): int
    Registration Manager -->> Entry Point: return 0
    deactivate Registration Manager
    Entry Point -->> UI: return https response 
    deactivate Entry Point
    UI -->> User: return unable to give system wide username
    deactivate UI
```