--TeamBigData.Utification.Users
CREATE TABLE dbo.UserProfiles
(
username varchar(50),
firstname varchar(20),
lastname varchar(50),
email varchar(50),
"address" varchar(50),
birthday Date,
CONSTRAINT UserProfiles_Primary_Key PRIMARY KEY(username),
CONSTRAINT UserProfiles_Foreign_Key_01 FOREIGN KEY (username) references dbo.Users(username)
);
