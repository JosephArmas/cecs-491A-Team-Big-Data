CREATE TABLE dbo.Users
(
username varchar(100),
"password" varchar(100),
email varchar(100),
CONSTRAINT Users_Primary_Key PRIMARY KEY(email),
CONSTRAINT Users_Candidate_Key_01 UNIQUE (username)
);