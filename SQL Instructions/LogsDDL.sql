--TeamBigData.Utification.Logs
CREATE TABLE Logs       
(
LogID int identity,
CorrelationID int,
LogLevel varchar(10),
[User] varchar(100),
[TimeStamp] datetime default getDate(),
[Event] varchar(70),
Category varchar(10),
[Message] varchar(100)
)