# _BAC Checker_

#### _Project created for Epicodus Group Project, C-Sharp/CSS - Week Five. June 22, 2017_

#### By _**David Wilson, Alyssa Moody, Maria Del Castillo, Julian Flood, Jenna Cooper**_

## Description

_A lighthearted app to check your Blood Alcohol Content. Two members of this team are in the C# course, and three are in CSS/Design. This app will combine the knowledge of both groups, and allow us to practice communicating with collaborators with different areas of expertise._

## Program Specifications

| Description  | Input Example | Output Example |
| ------------- | ------------- | ------------- |
| The program allows patrons to enter their information and view their meter.  | Name | "Chet Manly"  |
| The program allows patrons to order drinks and food.  | PBR  | PBR |
| The program increases patron BAC based on drinks ordered.  | .00  | .03123 |
| The program allows bartenders to be added to the database.  | Name | Horace |
| The program allows patrons to be added to bartenders.  | Horace | Horace - David |
| The program allows users to add drinks and food to the menu.  | Burger | Burger |
| The program allows patrons to view a full bar menu with all drinks and food.  | Burgers, Fries, etc  | Burgers, Fries, etc |
| The program allows patrons to view a total tab.  | 2 items ordered  | $8 |

## Wishlist Specifications

(C-SHARP)
1. Make BAC decrease over time/when food is ordered.
2. Fix bartender, show patronBAC thingamajig. GRRRRRRR

## Setup/Installation Requirements

_Runs on the .Net Framework._

_Install Visual Studio 2015. https://go.microsoft.com/fwlink/?LinkId=532606 ._

_Install ASP.Net 5. This will give you access to the .NET Framework. https://go.microsoft.com/fwlink/?LinkId=627627 ._

_Restart PowerShell. While located in your machine's Home directory, enter the command > dnvm upgrade._

_Requires Nancy Web Framework located at: http://nancyfx.org/. You can also do this via Windows PowerShell with the command > **Install-Package Nancy**_

_**From GitHub: Download or clone project repository onto desktop from GitHub.**_

_In your preferred database management system (I use SSMS), open the bac_checker.sql file from the project folder. Run the execute command on the file. If this does not work, run the following command in SQLCMD:

CREATE DATABASE bac_checker; GO USE bac_checker; GO CREATE TABLE patrons (id INT IDENTITY(1,1), name VARCHAR(100), gender VARCHAR(25), weight INT, height INT, bmi INT); GO CREATE TABLE drinks (id INT IDENTITY(1,1), name VARCHAR(50), drink_type VARCHAR(50), abv DECIMAL(3,1), cost DECIMAL(4,2)); GO CREATE TABLE orders (id INT IDENTITY(1,1), patrons_id INT, drinks_id INT); GO

_To create test database, in your preferred database management system (I use SSMS) open the bac_checker_test.sql file from the project folder. Run the execute command on the file. If this does not work, back up and restore the database as a test database in your preferred database management system.

_In PowerShell, cd into the project folder. Enter the command > **dnu restore**_

_Enter the command > **dnx kestrel**_

_In your preferred browser, navigate to http://localhost:5004/ and you should see the application._

## Known Bugs

_When refreshing a page after submitting a form, it will prompt user to re-submit data. If done this will duplicate data._

## Support and contact details

_If you run into any issues or have questions, ideas or concerns, please contact Alyssa Moody at alyssanicholemoody@gmail.com or David Wilson at davidtheadmiral@gmail.com_

## Technologies Used

_**Languages:** HTML, CSS, C#, SQL._

_**Frameworks:** Nancy, .Net._

_**Testing:** xUnit._

### License

*MIT license Agreement*

Copyright (c) 2017 **_David Wilson, Alyssa Moody, Maria Del Castillo, Julian Flood, Jenna Cooper_**
