CREATE TABLE dbo."Events"
(
userID int NOT NULL,
"disabled" int DEFAULT 0,
);

CREATE TABLE dbo.Pictures
(
userID int NOT NULL,
"disabled" int DEFAULT 0,
);

CREATE TABLE dbo.Pins
(
userID int NOT NULL,
"disabled" int DEFAULT 0,
);

CREATE TABLE dbo."Services"
(
userID int NOT NULL,
"disabled" int DEFAULT 0,
);