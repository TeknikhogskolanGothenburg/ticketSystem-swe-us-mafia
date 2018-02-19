# Ticket system architecture

* How are your ticket system build?
* Which components does your application consist of?

The ticket system is based on three main solutions: RestApi, BackOffice and TicketShop. BackOffice and TicketShop are
the client systems that are consuming the RestAPI. 

##RestApi
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
TicketTransactions table: TicketIDs. This was added since we saw the relevance in including this information in a customer order object. 
To be able to create objects that contain data both on the properties connected to the TicketTransactions table and data on the property 
TicketIDs we needed to construct database methods that let us match rows in the TicketTransactions table with rows in the TicketsToTransactions 
table. This was accomplished through methods (in TicketDatabase class) like "FindTicketsByTransactionID", this method gets all tickets for a
specific TicketTransaction. This method is called within the method "FindOrdersSuchThat", where we loop through the list of TicketIDs 
that we get as a result from the method "FindTicketsByTransactionID". For example to return a list of all customer Orders (Order objects) that 
matches a string query the method "FindOrdersSuchThat" is called within the method "FindCustomerOrders(string query)". 

####Ticket
When handling Ticket objects in the application we needed to make a decision on how to handle the concept of Tickets. Are Tickets something that 
exist when an administrator creates a TicketEventDate? Or is a Ticket object created when a customer decides to buy a ticket for a specific TicketEventDate? 
We decided to go for the latter option, to treat Tickets as something that is created when a customer adds a Ticket to their cart (decides they
want to buy a ticket for a TicketEventDate). The table Tickets in the database contains the fields: TicketID and SeatID, which were translated into 
properties of the Ticket class. In addition we added properties: VenueName, EventName, TicketEventPrice, EventStartDateTime, this since one can
argue that a customer would probobly want to see some more data on their selected Tickets than just TicketID and SeatID. For example in the TicketDatabase
method "FindTicketByTicketID" we therefore did a query that joins the Tickets table with other tables that contain the data on the properties which's data
can't be fetched from the Tickets table itself. In contrast when creating a Ticket we don't need to handle data for the extra properties, then we just want 
to insert a TicketID and a SeatID in the Tickets table. To prevent the case that two customers would get a Ticket with the same SeatID we did the following: 
constructed a method in TicketDatabase class called "FindOneAvailableSeatAtTicketEventDate" that checks for SeatIDs for the specified TicketEventDateID that 
are not already present in Tickets table, this method is then called within the "CreateTicket" method so that we can assign a free SeatID to the new Ticket.



 





# Context diagram

<img src="images/context.png" />

# Container diagram

<img src="images/container.png" />

# Database

<img src="images/database_diagram.png" />