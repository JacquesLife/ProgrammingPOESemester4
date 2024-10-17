## Prerequisites 

The application is built using ASP.NET with the target framework set to .NET 8.0. You will need to have the .NET 8.0 SDK installed on your machine to run the application. The Prog Web Application.Tests are also built using the .NET 8.0 SDK so there is no need to install a different version of the SDK. 

I have chosen to make use of an SQLite database for local storage of the user and claim data. It is a lightweight database that is easy to set up and use. The mydatabase.db file is included in the source code giving you full access to the database without having to set up a new one. It is located in the bin folder of Prog Web Application. If you would like to read the database file, you can use the SQLite Viewer to view the contents of the database. For the application to run effectively you do not need to have the SQLite or SQLite Viewer installed on your machine as I have included the database file in the source code.

Please ensure that you install the following software on your machine before running the application:

**Visual Studio**
- [Download .NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQLite/SQL Server Compact Toolbox for Visual Studio](https://marketplace.visualstudio.com/items?itemName=ErikEJ.SQLServerCompactSQLiteToolbox)

**Visual Studio Code**
- [Download .NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQLite](https://marketplace.visualstudio.com/items?itemName=alexcvzz.vscode-sqlite)
- [SQLite Viewer](https://marketplace.visualstudio.com/items?itemName=qwtel.sqlite-viewer)

Ensure that you follow the installation instructions provided and choose the installer that is appropriate for your operating system.

## How to use the Database Viewer in Visual Studio 

1. Open Visual Studio
2. Click on the View tab
3. Click on the SQL Server Compact/SQLite Toolbox option
4. Click on the Add SQL Server Compact/SQLite Connection option
5. Click on the Browse button
6. Navigate to the bin folder of the Prog Web Application project
7. Select the mydatabase.db file
8. Click on the Open button
9. Click on the OK button
10. You will now be able to view the contents of the database

## How to use the SQLite Viewer in Visual Studio Code

