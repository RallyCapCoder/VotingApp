# Welcome to the Voting App Wiki

**Requirements**  

1. Visual Studio 2015 or Visual Studio 2017.  

2. SQL Server 2017 or SQL Server Express local-db.

**Setting up the application**  

1. Clone the repository from master branch and save it to your local disk.
2. Un-Zip the VotingBox.zip file and restore the database to SQL server.
3. Open the VotingApp.sln in Visual Studio from where the code was saved from Step One.
4. Build the solution and allow nuget to restore all the packages.

**Configuring the Connection Strings**

There are two connection strings to set up in order to use the Voting App.

1. Find the App.config in the VotingApp project and Set the DataSource and Intial Catalog to what is set up in the local sql server.
2. Find the App.config in the VotingWeb project and Set the DataSource and Intial Catalog to what is set up in the local sql server.

Start Debugging the App! Configuration and Setup are complete!


# Using the App

**Registering A New User**  

_In order to use the App, other than the Home Page and Voting Information Page, you must be registered as a user._

1. Click the Register Button in the top right corner of the page.
2. Fill out the Form.  
    The password will need to have at least:
     * One Capital Letter
     * One Special Character and One Number
3. Click the Register Button.

**Registering A New Admin User**  
1. Repeat Steps for Registering A New User.
2. Once Registered, go into the database and find AspNetRoles. If you did not use the backup provided execute the following;   `INSERT INTO AspNetRoles(Id,Name)VALUES(NEWID(),'CanSeeElectionResults') `
3. Find your AspNetUserId in the AspNetUserTable by searching for your email address. Add a row in AspNetUserRoles with your UserId and the RoleId that you created/found in Step Two.

**Rules for Voting and Ballot Information**  
_You must be a registered voter to able to use this functionality._ 
_Once you vote, you cannot vote again on the same account. No Double Voting!_

Ranking Candidates (For President and Vice-President)
* Integers must be used in this sections inputs.  Text is invalid.
* Ranking input is for both President and Vice-President
* Write-in candidates are considered a valid rankable candidate.
  * The first input is for the Presidential candidate write-in.
  * The second input is for the Vice-Presidential candidate write-in.
  * The third input is for giving the candidates a rank.  
  
Multi Vote Candidates (State Representatives)
* Abilty to choose multiple candidates (up to two) by selecting each appropriate checkbox. 
* Ability to write-in one of the multiple candidates.
  * First input is for the name of the State Representative write-in.

Single Vote Ballot Issue/Candidate  
  * Ability to pick either Yes or No for these ballot items.
  * No write-in allowed for these items.

**Viewing the Election Results**  
_Must be logged in as an Admin, to use this functionality._  
1. Log in as an Admin.
2. Click the Election Results tab.
3. Results will be listed in the following order: 
  * President and Vice-President
  * Supreme Court Judge
  * State Representative
  * Ballot Issue
