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
dateCreated DateTime default DateTime.Now(),
dateLastModified DateTime default DateTime.Now(),
userLastModified int
);