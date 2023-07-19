# poplike

## What is it? ##
* A simple voting website, with items to vote or comment on.
* Items are grouped in categories.
* Users can see a list of all existing items.
* You can link to items from e.g. Facebook, to push traffic to a certain vote.

## Live examples ##
* [poplike.se](https://poplike.se)
* [tyckomuppsala.se](https://tyckomuppsala.se) (in Swedish)

## Getting started ##
* Find a web host with support for ASP.net Core and SQL Server
* Set up an email account
* Set up a SQL Server database and a user account for it.
* Add config files Web/Config/Database.json and Email.json
* Build the project with the "Local" config. (Similar to Debug.)
* Set the Web project as the Startup Project, e.g. by right-clicking on the Web project file in Visual Studio and selection that option.
* Add an initial database migration (by running 'add-migration Init' in the package manager console, with 'Persistence' as the default project), then run it to create a local database ('update-database')
* Try to run the web application locally, with the "Local" build configuration.
* Build the project with the "Remote" config.
* Run the database migration to create the production database.
* Try to run the web application locally with the "Remote" build configuration, against the production database.
* Set up a publishing profile, e.g. by using publish settings from your web hotel. Use the 'Production' build config.
* Publish
* Try to access the website.
* Tweak settings to turn on/off the user account creation features. (Sign up, Register account, Sign in, etc)

## Account creation ##
* There are two ways to create an account: SignUp and RegisterAccount.
* In the SignUp operation, the user provides only the email address, after which an email invitation is mailed out. This ensures the signup is intentional, and that the users is actually in possession of the given address.
* In the RegisterAccount the user actually creates the account directly, in a single step, without any email.
* For the initial Admin account, it may be best to use the RegisterAccount method (see appsettings for how to enable it) to avoid potential issues with the email settings used for the mailout in the SignUp operation.
* The way to make the initial Admin account is to first create the user (by the RegisterAccount or SignUp operation) and then to manually flip the Admin bit on that particular User in the database.

  
