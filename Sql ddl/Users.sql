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