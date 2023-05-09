Instructions for Microsoft SQL Server Manager Studio

Step 0. Install Microsoft SQL Server and Microsoft SQL Server Manager
https://www.microsoft.com/en-us/sql-server/sql-server-downloads

STEP 1. Create 2 SQL Databases	
	Right Click Databases folder and create new database
	First is named TeamBigData.Utification.Logs
	Second is named TeamBigData.Utification.Users
	Third is named TeamBigData.Utification.UserHash
	Fourth is named TeamBigData.Utification.Features
Step 2. Creaing an AppUser Login
	Right Click the Security Tab on the bottom of your SQL Server
	(It should be global, not in any 1 database)
	Create a new Login
	Name it AppUser
	Set it to SQL Server Authentication
	Set Password to t
	Click ok on the bottom
Step 3. Creating an AppUser User in the Logs Database
	Open The Logs Database
	Right Security and Create new User
	Type AppUser into User name and Login name
	Click Memberships on the left and make them a db_datareader
Step 4. Creating the Tables
	Execute the LogsDDL into the Logs Database
	*Very important to execute Users then UserProfiles, it has a foreign key constraint
	Execute the UsersDDL into the Users Database
	Execute the UserProfilesDDL into the Users Database
	Execute the FeaturesDDL into the Features Database
	Execute the UserHashDDL into the UserHash Database
Step 5. Execute the Stored Procedures for the Reputation Feature
	Execute the Reputation DDL Features(Stored Procedures) in the Features Database
	Execute the Reputation DDL Users(Stored Procedures) in the Users Database
Step 6. Granting Insert Permission on Logs
	Enter in the SQL query GRANT INSERT on dbo.Logs to AppUser

*No manual data has to be inputed for testing, but you have to run Registration Tests before Authentication Tests for it to populate the database