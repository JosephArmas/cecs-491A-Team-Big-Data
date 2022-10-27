# As a user I cannot register to create an account with an existing username 
```mermaid
sequenceDiagram
    actor User
    User ->> UI: create an account
    activate UI
    UI ->> Entry Point: https request
    activate Entry Point
    Entry Point ->> Resgistration Manager: isValid(email:string,pw:string):int
    activate Resgistration Manager
    Resgistration Manager ->> Registration Services: EmailExist(email:string):int
    activate Registration Services
    Registration Services ->> DataAccess: getEmail(e:string):int 
    activate DataAccess
    DataAccess ->> MySQL: searchEmail(email:string):byte
    activate MySQL
    MySQL -->> DataAccess: return 1
    DataAccess -->> Registration Services: return 1
    Registration Services -->> Resgistration Manager: return 1
    Resgistration Manager -->> Entry Point: return 1
    Entry Point ->> Resgistration Manager: genUsername(userName:string): int
    Resgistration Manager ->> Registration Services: UserExist(userName:string):int
    Registration Services ->> DataAccess: getUsername(userName:string): int
    DataAccess ->> MySQL: searchUser(userName:string):byte
    MySQL -->> DataAccess: return 1
    deactivate MySQL
    DataAccess -->> Registration Services: return 1
    deactivate DataAccess
    Registration Services -->> Resgistration Manager: return 1
    deactivate Registration Services
    Resgistration Manager -->> Entry Point: return 1
    deactivate Resgistration Manager
    Entry Point -->> UI: https response
    deactivate Entry Point
    UI -->> User: return user already exist 
    deactivate UI
```