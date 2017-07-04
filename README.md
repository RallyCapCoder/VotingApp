# Welcome to the Voting App wiki!

Requirements  

1. Visual Studio 2015 or Visual Studio 2017  

2. SQL Server 2017 or SQL Server Express local-db

Setting up the application 

1. Clone the repository or download the zip file from master branch and save it to your local disk
2. Un-Zip the VotingBox.zip file and restore the database to SQL server
3. Open the VotingApp.sln in Visual Studio from where you saved the code from step 1
4. Build the solution and allow nuget to restore all the packages

Configuring the Connection Strings

There are two connection strings you need to setup in order to use the Voting App

1. Find the App.config in the VotingApp project and Set the DataSource and Intial Catalog to what you set up in sql server
2. Find the App.config in the VotingWeb project and Set the DataSource and Intial Catalog to what you set up in sql server

Start Debugging the App! You have finshed the configuration and setup!


#Voting App Information!
