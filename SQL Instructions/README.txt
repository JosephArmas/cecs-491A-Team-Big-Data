Instructions for Microsoft SQL Server Manager Studio

STEP 1. Create 2 SQL Databases	
	Right Click Databases folder and create new database
	First is named TeamBigData.Utification.Logs
	Second is named TeamBigData.Utification.Users
Step 2. Creaing an AppUser Login
	Right Click the Security Tab on the bottom of your SQL Server
	(It should be global, not in any 1 database)
	Create a new Login
	Name it AppUser
	Set it to SQL Server Authentication
	Set Password to t
	Click ok
Step 3. Creating an AppUser User in the Logs Database
	Open The Logs Database
	Right Security and Create new User
	Type AppUser into User name and Login name
	Click Memberships on the left and make them a db_reader
	Enter in the SQL query GRANT INSERT on dbo.Logs to AppUser
Step 4. Creating the Tables
	Execute the LogsDDL into the Logs Database
	Execute the UsersDDL into the Users Database
	Execute the UserProfilesDDL into the Users Database
	*Very important to execute Users then UserProfiles, it has a foreign key constraint

*No manual data has to be inputed for testing, but you have to run Registration Tests before Authentication Tests for it to populate the database