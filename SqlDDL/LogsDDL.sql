CREATE TABLE Logs       
(
LogID int identity,
[DateTime] varchar(50),
LogLevel varchar(10),
Opr varchar(70),
Category varchar(10),
[Message] varchar(100)
)
