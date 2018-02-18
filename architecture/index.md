# Ticket system architecture

* How are your ticket system build?
* Which components does your application consist of?

The ticket system is based on three main solutions: RestApi, BackOffice and TicketShop. BackOffice and TicketShop are
the client systems that are consuming the RestAPI. 

##RestApi
The RestApi Solution consist of a .NET Core Web API application and three separate class libraries used by this application.

###DatabaseRepository
This class library contains the TicketDatabase class in which all methods handling the interaction with the TicketSystem database
are located. Methods here are taking care of: 
- Getting data on all existing Venues, Tickets, TicketEvents, etc. from the database,
- Updating data on existing Venues, Tickets, etc.,
- Deleting data on existing Venues, Tickets, etc.,
- Inserting new data in the database tables for Tickets, Venues, etc.
The TicketDatabase class uses object types from the TicketSystemEngine Library which contains the model classes with their 
respective properties, representing the objects one can identify through the tables and columns in the TicketSystem database.
The queries constructed within the different methods in TicketDatabase class are parameterized to enhance security.

###PaymentProvider
This class library contains the logic for payment handling in the ticket system. It contains three classes:
one interface- IPaymentProvider, PaymentProvider and Payment. The method Pay in PaymentProvider class is used to be able
to simulate a realy payment in the TicketSystem. A status for the Payment is randomly set.
In the RestAPI in OrderController, more specifically in the POST-method "CreateOrder" we are calling the Pay method. If we get
another status than 0 (represents PaymentApproved) the order won't be created.

 





# Context diagram

<img src="images/context.png" />

# Container diagram

<img src="images/container.png" />

# Database

<img src="images/database_diagram.png" />