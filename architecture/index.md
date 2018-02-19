# Ticket system architecture

* How are your ticket system build?
* Which components does your application consist of?

The ticket system is based on three main solutions: RestApi, BackOffice and TicketShop. BackOffice and TicketShop are
the client systems that are consuming the RestAPI. 

##RestApi Solution
The RestApi Solution consist of a .NET Core Web API application and three separate class libraries used by this application.

###DatabaseRepository
This class library contains the TicketDatabase class in which all methods handling the interaction with the TicketSystem database
are located. Methods here are taking car of things like:
- Getting data on all existing Venues, Tickets, TicketEvents, etc. from the database,
- Updating data on existing Venues, Tickets, etc.,
- Deleting data on existing Venues, Tickets, etc.,
- Inserting new data in the database tables for Tickets, Venues, etc.
The TicketDatabase class uses object types from the TicketSystemEngine Library, which contains the model classes with their 
respective properties, representing the objects one can identify through the tables and columns in the TicketSystem database.
The queries constructed within the different methods in TicketDatabase class are parameterized to enhance security.

###PaymentProvider
Class library that contains the logic for payment handling in the ticket system. It contains three classes:
one interface- IPaymentProvider, PaymentProvider and Payment. The method Pay in PaymentProvider class is used to simulate 
a real payment in the TicketSystem. A status for the Payment is randomly set. In the RestAPI in OrderController, more 
specifically in the POST-method "CreateOrder" we are calling the Pay method. If the PaymentStatus is set to something other 
than 0 (represents PaymentApproved) the order won't be created.

###TicketSystemEngine
Class library that contains all the model classes used to bind the database data to objects. Contains the following model classes:

####Order 
The table in the database that most closely resembles how we would define an order is TicketTransactions, therefore we used this
table to define order objects in the application. We added one property to the model that is not to be found as a column in the 
TicketTransactions table: TicketIDs. This was added since we saw the relevance in including this data in a customer order object. 
To be able to create objects that contain data both for the properties connected to the TicketTransactions table and data for
TicketIDs we needed to construct database methods that let us match rows in the TicketTransactions table with rows in the TicketsToTransactions 
table. This was accomplished through methods (in TicketDatabase class) like "FindTicketsByTransactionID", this method gets all tickets for a
specific TicketTransaction. This method is called within the method "FindOrdersSuchThat", where we loop through the list of TicketIDs 
that we get as a result from the method "FindTicketsByTransactionID". For example to return a list of all customer Orders (Order objects) that 
matches a string query the method "FindOrdersSuchThat" is called within the method "FindCustomerOrders(string query)". 

####Ticket
When it comes to Ticket objects in the application we needed to make a decision on how to handle the concept of Tickets. Are Tickets something that 
exist when an administrator creates a TicketEventDate? Or is a Ticket object created when a customer decides to buy a ticket for a specific TicketEventDate? 
We decided to go for the latter option, to treat Tickets as something that is created when a customer adds a Ticket to their cart. The table Tickets in the 
database contains the fields: TicketID and SeatID, which were translated into properties of the Ticket class. In addition we added properties: VenueName, 
EventName, TicketEventPrice, EventStartDateTime, this since one can argue that a customer would probobly want to see some more data on their selected Tickets 
than just TicketID and SeatID. For example in the TicketDatabase method "FindTicketByTicketID" we therefore did a query that joins the Tickets table with other 
tables that contain the data on the properties which's data can't be fetched from the Tickets table itself. In contrast when creating a Ticket we don't need 
to handle data for the extra properties, then we just insert a TicketID and a SeatID in the Tickets table. To prevent the case that two customers would get a 
Ticket with the same SeatID we did the following: constructed a method in TicketDatabase class called "FindOneAvailableSeatAtTicketEventDate" that checks for 
SeatIDs for the specified TicketEventDateID that are not already present in Tickets table, this method is then called within the "CreateTicket" method so a 
free SeatID is assigned to the new Ticket.

####TicketEvent
Class that defines objects of the type TicketEvent. This class represents a TicketEvent, containing properties that match each column in the TicketEvent table.
We added one column to the TicketEvent table: TicketEventPrice since we think that every Event should have a specific price.

####TicketEventDate
This class is connected to the TicketEvent class since you can't create a TicketEventDate object if you haven't first created a TicketEvent object, that since the
corresponding tables in the database for these classes have a relationship to eachother. A TicketEventDate object contains a TicketEventID, which means that the
ID assigned to this property of a TicketEventDate object needs to be found in the database. For example to list all TicketEventDates to a customer in the TicketShop
there is a method in the TicketDatabase class that selects all TicketEventDates from the database and returns it as a list of TicketEventDate objects.

####Venue
Class that defines objects of the type Venue, it matches the database table Venues and its columns exactly with it's properties: VenueID, VenueName, Address, City
and Country. This class is used to create Venue objects representing database data for example when someone wants to get a list of all existing Venues in the TicketSystem.
The database method for creating a new Venue inserts the Venue data in the database and also returns a Venue object, constructed from the parameters sent in to the method.


###RestAPI
The RestAPI project contains five controller classes that handles all the CRUD-functions: Create, Read, Update and Delete. It is these methods that are called
from within the applications: BackOffice and TicketShop to get the data needed and carry out the actions on the website that respective user (administrator and 
customer) should be able to do.

####EventController
This controller calls the TicketDatabase methods that handles TicketEvents. An example of a method in this controller is the POST-method. In the model class 
TicketEvent in TicketSystemEngine we have defined that the properties EventName and TicketEventPrice are required, the POST-method "CreateTicketEvent"
therefore checks if ModelState IsValid according to this criteria. If so, data on a TicketEvent is sent in to database table TicketEvents by calling the 
TicketDatabase method "EventAdd" and the POST-method returns a TicketEvent object.

####OrderController
Controller that calls TicketDatabase methods that works with data in database tables TicketTransactions, TicketsToTransactions and Tickets. For example GET-method
"Public IEnumerable<Order>FindCustomerOrders(string query)" calls the TicketDatabase method "FindCustomerOrders(query)" and returns a list of Order objects 
(TicketTransactions in database) based on the string value sent in to method, which can be either BuyerFirstName, BuyerLastName or BuyerEmailAddress. This method
is constructed for the use case when an administrator working in BackOffice wants to look up orders connected to a specific customer. 
Another example of a method in this class is the POST-method: "CreateOrder" that returns an int representing a TransactionID. Here we have use the built in input
validation to check if required properties when constructing an Order object has been given values when calling the method. The method also controls that one or more
TicketIDs are given. If the ModelState is valid and method got one or more TicketIDs it continues to Payment and if PaymentStatus is set to "PaymentApproved" method
tries to parse the string of TicketIDs as whole numbers and assigning them to the array TicketIDs. After that it tries to call the TicketDatabase method "AddCustomerOrder"
and return a TransactionID if successfull. If something goes wrong in these two last operations we return 0 and set Response StatusCode to 400. If PaymentStatus was set to 
something other than PaymentApproved we are returning the PaymentStatus we got and Response StatusCode is set to 403 (Forbidden). If instead something went wrong already 
in the if statement where we check that ModelState IsValid and that TicketID's are not an empty string we return 0 and set Response StatusCode to 400 (Bad Request).

####TicketController
This controller handels all the operations connected to Tickets. We didn't include an Update method in this controller since we didn't think this
was an essential use case. Since a Ticket only consists of a TicketID and a SeatID and we decided that every Ticket gets a random SeatID from available Seats
at the specific TicketEventDate there isn't much data that one could want to update on a Ticket. The POST-method in this controller (CreateTicket) takes a 
TicketEventDateID as it's parameter. The method then checks if this TicketEventDateID exists in the database, by calling the TicketDatabase method "FindTicketEventDateByID".
If this is not null, method calls TicketDatabase method "CreateTicket" and returns a Ticket object representing a Ticket connected to the specified TicketEventDateID.
If the TicketEventDateID wasn't found in the database we instead return null and Response StatusCode is set to 404 (not found). The "CreateTicket" method is used
when a customer in TicketShop adds a Ticket to her cart. 

####TicketEventDateController
The GET-methods  "GetAllTicketEventDates" and "FindTicketEventDates" are used to display existing TicketEventDates to customers in the TicketShop. POST-method 
"AddNewTicketEventDate" is used in BackOffice to be able to add TicketEventDates connected to TicketEvents. In model class TicketEventDate we have defined that
properties TicketEventID, VenueID, EventStartDateTime and NumberOfSeats requires values. This is checked in the POST-method, if ModelState is not valid or the
VenueID provided is less or equal to 0 we set the Response StatusCode to 400. If ModelState is valid we try calling TicketDatabase method "AddTicketEventDate"
to insert data into database table TicketEventDates. If something should be wrong in this, we catch either argumentexception or Sqlexception (depending on the
error) and set Response StatusCode to 400. 

####VenueController
This controller is primarily used for operations connected to the administration of Venues in BackOffice webapplication. For example the POST-method is used
when an administrator wants to add a new Venue. Also for the model class Venue in TicketSystemEngine we have defined properties as Required. In the POST-method
this is checked through ModelState IsValid, if so we insert data on a new Venue in database table Venues, otherwise Response StatusCode is set to 400. For the 
Delete-method "DeleteVenue" we have to check if the Venue that the user tries to delete actually exists in the database. This is accomplished by checking the 
result of calling the method "FindVenueByID". If the VenueID isn't found in the database Response StatusCode is set to 404, otherwise the Venue is deleted.


##BackOffice Solution
This solution contains one .Net Core Application project: BackOfficeWeb and two class libraries: RestApiClient and TicketSystemEngine. 

###RestApiClient
This classlibrary is used to call the CRUD-methods in the RESTApi solution. We have created invididual Api classes to divide
the operations in to different responsibility areas: EventApi, TicketApi, OrderAdministrationApi, TicketEventDateApi and 
VenueApi. In each of these classes the relevante CRUD-functions in RestAPI is called so we can handle the request sent from
the webbrowser and give back the correct (expected response). 

###TicketSystemEngine
Here we are including the TicketSystemEngine class library that was created in the RestAPI solution, to be able to refer back
to the same model when using the CRUD-functions as we did when creating them. 

###BackOfficeWeb

####Models

#####ApplicationUser 
This class is automatically generated when including the Microsoft.AspNetCore.Identity into the project. It is used for handling 
User logins in the application.

#####OverviewModel
Class used to represent the objects we are representing in BackOffice as lists, contains a list of Venue objects, list of TicketEvent
objects and list of TicketEventDates. 

#####VenueEventModel
Class that contains two properties: A list that is to contain Venue objects and a list that is to contain TicketEvents. This class
is similar to OverviewModel; it uses the models found in TicketSystemEngine and adapts them to how we want to show the data to users.

####Controllers

#####AccountController
This controller handles the login requests and requests for creating user accounts by users in the BackOffice webapplication. 

#####ManageController
Controller generated when including the Microsoft.AspNetCore Authentication, Authorization and Identity. This controller is
used for managing user accounts. For example if a user wants to change her password, logins and then clicks on "Password",
enters a new password and clicks on the button "Update password" the IActionResult method "ChangePassword" is called.

#####HomeController
This can be said to be the main controller of our BackofficeWeb application. The method "Index" checks if the user is authenticated correctly, if so returns the view
with a list of existing Venues. If not, the user is redirected to the Login view again. To be able to show the different menu pages and let users interact with them the 
following actionresult methods were created: Events, Order, DateAdd, EditEvent, EventAdd, VenueAdd.

####Views
In views we have three separate folders: Account, Manage and Home. The Account and Manage views are handling the views connected to login, management of user accounts and the like.
The Home folder contains ten separate view classes that each handles separate aspects of what to show to the user according to a specific request-response scenario.
For example: VenueAdd view handles what to show to the user and how to handle the data that the user put in when adding a venue. In this view an Ajax script is used to send the values 
entered by the user in th web GUI to the RESTApi method AddVenue (the latter also inserts the data in to the database table Venues). In the other views we are similarly using Ajax scripts
to communicate with the RestAPI and it's methods. For some views we are handling some of the requests through the HomeController instead, for example in "Event" view when we want to show 
a list of existing TicketEvents, that is handled through the HomeController method "Events", to fill the list of TicketEvents we are then looping through a list of existing TicketEvent objects
that we got through the EventApi. The Delete-action for TicketEvents in Event view is handled through an Ajax script though, in the Ajax script we are calling the Delete-method in the RESTApi 
directly, to remove the TicketEvent from the database.


##TicketShop Solution
This solution contains one ASP .NET Core web application called: TicketShop and two classlibraries: TicketSystemEngine and RESTApiClient. 

###RestApiClient
This classlibrary is used to call the CRUD-methods in the RESTApi solution. We have created invididual Api classes to divide
the operations in to different responsibility areas: EventApi, TicketApi, OrderAdministrationApi, TicketEventDateApi and 
VenueApi. In each of these classes the relevante CRUD-functions in RestAPI is called so we can handle the request sent from
the webbrowser and give back the correct (expected response). 

###TicketSystemEngine
Here we are including the TicketSystemEngine class library that was created in the RestAPI solution, to be able to refer back
to the same model when using the CRUD-functions as we did when creating them. 

###TicketShopWeb

####Models

#####ApplicationUser 
This class is automatically generated when including the Microsoft.AspNetCore.Identity into the project. It is used for handling 
User logins in the application.

#####CustomerModel
Class that contains four properties: each of them are a list that is to contain- TicketEvent objects, TicketEventDate objects, Venue objects and Ticket objects. This class
uses the models found in TicketSystemEngine and adapts them to how we want to show the data to users.

#####CartModel
Thi class is used to represent a cart in the TicketShop, it only contains one propert: A list that is to contain Ticket objects.

#####CustomerSession
At the moment this model class is not in use. The purpose with the class is to handle CustomerSession objects: representing one customers interaction with the TicketShop. 

####Controllers

#####AccountController
This controller handles the login requests and requests for creating user accounts by users in the BackOffice webapplication. 

#####ManageController
Controller generated when including the Microsoft.AspNetCore Authentication, Authorization and Identity. This controller is
used for managing user accounts. For example if a user wants to change her password, logins and then clicks on "Password",
enters a new password and clicks on the button "Update password" the IActionResult method "ChangePassword" is called.

#####HomeController
This can be said to be the main controller class of the TicketShopWeb application. For example the "Index" IActionResult method returns a view that shows all available
TicketEventDates to the user when the user has successfully logged in. The actionresult method "Cart" are handling the showing of the view of the cart, where the Tickets
the customer has decided to buy should be shown. 

####Views
In views we have three separate folders: Account, Manage and Home. The Account and Manage views are handling the views connected to login, management of user accounts and the like.
The Home folder contains ten separate view classes that each handles separate aspects of what to show to the user according to a specific request-response scenario. For example:
In "Index" view we are handling what to show to the user on the first page after user has successfully logged in and starts interacting with the webpage. To get data on Venues,
TicketEvents and TicketEventDates that each holds the data connected to Events that we want to show the user, the IActionResult method "Index" in HomeController constructs
lists of TicketEvents, TicketEventDates and Venues by calling the RestAPI GET-methods on an eventapi, venuapi and dateapi object. Each list are then used in the "Index" view to show
the result to the user (List of Events that user can buy tickets for). To let the user add a ticket to their cart an Ajax script is used. Likewise an Ajax script is used to create a 
Ticket in the database when user clicks on the "Add to cart" button, in this script the RestApi's POST-method for Tickets are directly called and data on a Ticket is put in to the 
database.



# Context diagram

<img src="images/context.png" />

# Container diagram

<img src="images/container.png" />

# Database

<img src="images/database_diagram.png" />