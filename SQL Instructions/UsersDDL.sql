CREATE TABLE dbo.Users
(
username varchar(50),
"password" varchar(200),
"disabled" int DEFAULT 0,
CONSTRAINT Users_Primary_Key PRIMARY KEY(username),
);
