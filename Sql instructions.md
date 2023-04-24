# Sql instructions
# Step 1 Create 4 Databases:
1. TeamBigData.Utification.Logs
2. TeamBigData.Utification.Users
2. TeamBigData.Utification.UserHash
2. TeamBigData.Utification.Features
# Step 2 Create a global AppUser Login (using CLI)
* This will be used for the logging database 
* Create a global AppUser\
```CREATE LOGIN AppUser WITH PASSWORD = 't', CHECK_POLICY=OFF```
# Step 3 Create a global admin/root user (using CLI) -> (**optional**, **skip** if using integrated security)
* Create a global root/admin user to alter any database\
```CREATE LOGIN root WITH PASSWORD = 'root', CHECK_POLICY=OFF```
# Step 4 populating the tables
* Open console for TeamBigData.Utification.Logs
```
CREATE TABLE Logs
(
LogID int IDENTITY,
CorrelationID int,
LogLevel varchar(10),
UserHash varchar(200),
[TimeStamp] datetime default getDate(),
[Event] varchar(70),
Category varchar(10),
[Message] varchar(100)
);
```

* Add the AppUser so that there is logging and cannot be immutable
```
CREATE USER AppUser FOR LOGIN AppUser;
ALTER ROLE [db_datareader] ADD MEMBER AppUser;
Grant INSERT to AppUser;
```

* Open console for TeamBigData.Utification.UserHash
```
CREATE TABLE dbo.UserHash
(
userHash varchar(200) NOT NULL,
"userID" int NOT NULL,
CONSTRAINT UserHash_Primary_Key PRIMARY KEY(userHash),
);
```

* Open console for TeamBigData.Utification.Users (make sure to **RUN** **Users** sql **FIRST** then **UserProfiles**)
```
CREATE TABLE dbo.Users
(
userID int NOT NULL Identity(1001,1),
username varchar(50),
"password" varchar(200),
"disabled" int DEFAULT 0,
salt varchar(50),
userHash varchar(200),
CONSTRAINT Users_Primary_Key PRIMARY KEY(userID),
CONSTRAINT Users_Candidate_Key_01 UNIQUE (username)
);
```

```
CREATE TABLE dbo.UserProfiles
(
userID int NOT NULL,
firstname varchar(20),
lastname varchar(50),
"address" varchar(50),
birthday Date,
"role" varchar(50),
CONSTRAINT UserProfiles_Primary_Key PRIMARY KEY(userID),
CONSTRAINT UserProfiles_Foreign_Key_01 FOREIGN KEY (userID) references dbo.Users(userID)
);
```

* Open console for TeamBigData.Utification.Features
```
-- auto-generated definition
create table dbo.Events
(
    title          char(30)      not null,
    description    char(150)     not null,
    eventID        int identity (200, 1)
        constraint Events_pk
            primary key,
    eventCreated   date          not null,
    count          int default 0 not null,
    userID         int           not null,
    disabled       int default 0 not null,
    lat            float         not null,
    lng            float         not null,
    showAttendance int default 0 not null
)
go

-- auto-generated definition
create table dbo.EventsJoined
(
    userID  int not null,
    eventID int not null
        constraint EventsJoined_Events_eventID_fk
            references dbo.Events
            on delete cascade,
    constraint EventsJoined_pk
        primary key (userID, eventID)
)
go


CREATE TABLE dbo.Pictures
(
userID int NOT NULL,
"disabled" int DEFAULT 0,
);

CREATE TABLE dbo.Pins
(
pinID int NOT NULL Identity,
userID int NOT NULL,
lat varchar(50),
lng varchar(50),
pinType int,
"description" varchar(MAX),
"disabled" int DEFAULT 0,
completed int DEFAULT 0,
"dateTime" varchar(50),
);

CREATE TABLE dbo."Services"
(
userID int NOT NULL,
"disabled" int DEFAULT 0,
);
```

# Step 5 (optional) instead of using Integrated Security and using the root/admin user created earlier
* In each corresponding database console
```
CREATE USER root FOR LOGIN root;
GRANT INSERT,SELECT,UPDATE,DELETE to root;
```
