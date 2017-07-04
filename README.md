# Welcome to the Voting App wiki

**Requirements**  

1. Visual Studio 2015 or Visual Studio 2017  

2. SQL Server 2017 or SQL Server Express local-db

**Setting up the application**  

1. Clone the repository or download the zip file from master branch and save it to your local disk
2. Un-Zip the VotingBox.zip file and restore the database to SQL server
3. Open the VotingApp.sln in Visual Studio from where you saved the code from step 1
4. Build the solution and allow nuget to restore all the packages

**Configuring the Connection Strings**

There are two connection strings you need to setup in order to use the Voting App

1. Find the App.config in the VotingApp project and Set the DataSource and Intial Catalog to what you set up in sql server
2. Find the App.config in the VotingWeb project and Set the DataSource and Intial Catalog to what you set up in sql server

Start Debugging the App! You have finshed the configuration and setup!


# Using the App

**Registering A New User**  

_In order to use the App besides the Home page and Voting Information page, you need to be registered as a user._

1. Click the Register Button in the top right corner of the page
2. Fill out the Form, your password will need to have at least
   * One Capital Letter
   * One Special Character and One Number
3. Click the Register Button

**Registering A New Admin User**  
1. Repeat Steps for Registering A New User
2. Once Registered, go into your database and find AspNetRoles, you should see one row in there if you used the backup provided if not execute `INSERT INTO AspNetRoles(Id,Name)VALUES(NEWID(),'CanSeeElectionResults') `
3. Find your AspNetUserId in the AspNetUserTable by searching for your email then Add a row in AspNetUserRoles with your UserId and the RoleId that you created/found in step 2

**Rules for Voting and Ballot Information**  
_You must be a registered voter to able to use this functionality_ 
* For the ranking the candidates section, you must use integers page will not allow for text
* You can Write in a rankable candidate
* Vote in one oval sections only allow one of the check boxes to be selected, you cant double vote!
* Vote for two section allows for a write in but only allows two choices including the write in

**Seeing the Election Results**  
_Must be logged in as an admin to use this functionality _
1. Log in as an Admin 
2. Click Election Results tab
