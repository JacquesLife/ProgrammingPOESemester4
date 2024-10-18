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
2. Open the Prog Web Application solution file
3. Click on the View tab
4. Click on the SQL Server Compact/SQLite Toolbox option
5. Click on the Add SQL Server Compact/SQLite Connection option
6. Click on the Browse button
7. Navigate to the bin folder of the Prog Web Application project
8. Select the mydatabase.db file
9. Click on the Open button
10. Click on the OK button
11. You will now be able to view the contents of the database

## How to use the SQLite Viewer in Visual Studio Code

1. Open Visual Studio Code
2. Navigate to Prog Web Application folder
3. Open Prog Web Application Folder 
4. press `Ctrl + Shift + P` to open the command palette or `Cmd + Shift + P` on Mac
5. Type `SQLite: Open Database` and press Enter
6. Choose the mydatabase.db file and press Enter
7. Navigate to the Explorer tab
8. Click on the SQLite Explorer option to view the contents of the database

## How to run the application

1. Open Visual Studio
2. Open the Prog Web Application solution file
3. Right-click on the Prog Web Application project
4. Click on the Set as StartUp Project option
5. Click on the Start button to run the application
6. The application will open in your default browser
7. You can now interact with the application

## How to run the tests

1. Open Visual Studio
2. Open the Prog Web Application solution file
3. Right-click on the Prog Web Application.Tests project
4. Click on the Run Tests option
5. The tests will run and you will see the results in the Test Explorer

## How to run the application using Visual Studio Code

1. Open Visual Studio Code
2. Navigate to Prog Web Application folder
3. Open Prog Web Application Folder
4. Press `Ctrl + F5` to run the application or `fn + F5` on Mac
5. The application will open in your default browser
6. You can now interact with the application

## How to run the tests using Visual Studio Code

1. Open Visual Studio Code
2. Navigate to Prog Web Application folder
3. Open Prog Web Application Folder
4. Navigate to the Prog Web Application.Tests folder by right-clicking on the folder and selecting the Open in integrated Terminal option 
5. Type `dotnet test` in the terminal and press Enter
6. The tests will run and you will see the results in the terminal

# References

Hassan, Z.U. (2024). Action Result In ASP.NET MVC. [online] C-sharpcorner.com. Available at: https://www.c-sharpcorner.com/article/action-result-in-asp-net-mvc/.

Keep it simple, stupid. (2023). Unit testing in C# .NET with MSTest & Moq. [online] YouTube. Available at: https://www.youtube.com/watch?v=7UFjv_l0nfo.

Khan, A. (2024). Working with SQL Lite Database in Asp.NET Core Web API. [online] C-sharpcorner.com. Available at: https://www.c-sharpcorner.com/article/working-with-sql-lite-database-in-asp-net-core-web-api/.

Rick-Anderson (2022). Model validation in ASP.NET Core MVC. [online] learn.microsoft.com. Available at: https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-6.0.

ncarandini (2023). Unit testing C# with MSTest and .NET - .NET. [online] learn.microsoft.com. Available at: https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest.

tdykstra (2023). Handle errors in ASP.NET Core. [online] learn.microsoft.com. Available at: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-8.0.

tdykstra (2024). Dependency injection in ASP.NET Core. [online] Microsoft.com. Available at: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-6.0.

TutorialBrain (2022). How to Run SQLITE in Visual Studio Code. [online] YouTube. Available at: https://www.youtube.com/watch?v=JrAiefGNUq8.

