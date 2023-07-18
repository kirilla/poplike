# poplike

## What is it? ##
* A simple voting platform, with items to vote or comment on.
* Items are grouped in categories.
* Users can see a list of all existing items.

## Live examples ##
* [poplike.se](https://poplike.se)
* [tyckomuppsala.se](https://tyckomuppsala.se)

## Getting started ##
* Find a web host with support for ASP.net Core and SQL Server
* Set up an email account
* Set up a SQL Server database and a user account for it.
* Add config files Web/Config/Database.json and Email.json
* Build the project with the "Local" config. (Similar to Debug.)
* Add and initial database migration and run it to create a local database
* Try to run the web application locally, with the "Local" build configuration.
* Build the project with the "Remote" config.
* Run the database migration to create the production database.
* Try to run the web application locally with the "Remote" build configuration, against the production database.
* Set up a publishing profile, e.g. by using publish settings from your web hotel.
* Publish
* Try to access the website.
* Tweak settings to turn on/off the user account creation features. (Sign up, Register account, Sign in, etc)
