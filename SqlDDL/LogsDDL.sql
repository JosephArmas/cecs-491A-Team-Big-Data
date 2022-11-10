CREATE TABLE Logs       
(
LogID int identity,
CorrelationID int,
LogLevel varchar(10),
[User] varchar(100),
[DateTime] varchar(50),
[Event] varchar(70),
Category varchar(10),
[Message] varchar(100)
)
