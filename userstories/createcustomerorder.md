#Create customer order
The Ticketshop web application wants to be able to create a new customer order
when a customer presses the "confirm order" button in the web GUI. The Ticketshop
web application therefore wants to be able to use the RESTApis POST-method CreateOrder
which also handles the payment of an order and makes sure the order data gets in to the
database table TicketTransactions if paymentstatus was set to PaymentApproved.