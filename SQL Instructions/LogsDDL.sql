--TeamBigData.Utification.Logs
CREATE TABLE Logs       
(
LogID int IDENTITY,
CorrelationID int,
LogLevel varchar(10),
<<<<<<<< HEAD:SQL Instructions/LogsDDL.txt
UserHash varchar(200),
========
[User] varchar(100),
>>>>>>>> integration:SQL Instructions/LogsDDL.sql
[TimeStamp] datetime default getDate(),
[Event] varchar(70),
Category varchar(10),
[Message] varchar(100)
)