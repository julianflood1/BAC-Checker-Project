# _BAC Checker_

#### _Project created for Epicodus Group Project, C-Sharp/CSS - Week Five. June 22, 2017_

#### By _**David Wilson, Alyssa Moody, Maria Del Castillo, Julian Flood, Jenna Cooper**_

## Description

_A lighthearted app to check your Blood Alcohol Content. Two members of this team are in the C# course, and three are in CSS/Design. This app will combine the knowledge of both groups, and allow us to practice communicating with collaborators with different areas of expertise._

## Program Specifications

| Description  | Input Example | Output Example |
| ------------- | ------------- | ------------- |
| 1. The program allows patrons to enter their information and the program will calculate their bmi.  | Name, Gender, Weight, Height | --  |
| 2. The program allows patrons to order a drink.  | --   | --  |

## Wishlist Specifications

(C-SHARP)
1. CHANGE ADD* to orders table method to account for all connected tables
2. Make BAC decrease over time.
3. Add "I drank water" button/order food button to decrease BAC (can be included in #1)
4. Add total of customer tab
5. Fix bartender, show patronBAC thingamajig. GRRRRRRR (see css #1)
6. Make instances more accurate (turn to decimal) (maybe make formula more accurate too)
7. Remove success page (can probably make dictionary just like the Get["/whatever/whatever"])

(CSS)

1. Individual bartender page (bartender.cshtml) - list of their patrons WITH METERS MAYBE ZOMG (that part is c#... dammit)
2. Style links differently than regular text. Underline or different color?

NOTES 6/21
Nav bar is super nice, but only shows on a few pages. Should show on all but home page, maybe even home page as well. Not sure what's wrong.
Pages missing from:
1.  Success (gonna try to change success page routing)
2.  bartender.cshtml
3.  bar_menu.cshtml
4.  patron_add.cshtml
5.  patron.cshtml
6.  drinks_add.cshtml
7.  food_add.cshtml
Dropdown lists on patron page could use some styling.
List of patrons on patrons.cshtml is all mashed together.
When clicking on link to recently created patron it leads to a broken page due to null values (probably the error we were having with the 4join table)
Wow I'm tired and I cant think of anything else.

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

_NA_

## Support and contact details

_If you run into any issues or have questions, ideas or concerns, please contact Alyssa Moody at alyssanicholemoody@gmail.com_

## Technologies Used

_**Languages:** HTML, CSS, C#, SQL._

_**Frameworks:** Nancy, .Net._

_**Testing:** xUnit._

### License

*MIT license Agreement*

Copyright (c) 2017 **_David Wilson, Alyssa Moody, Maria Del Castillo, Julian Flood, Jenna Cooper_**
