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