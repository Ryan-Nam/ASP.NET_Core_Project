# ASP.NET_Core_Project
This is made by Razor page, Showcasing authentication, search function based on DB using raw SQL query, statistics, booking system and different model classes. SQLite is injected as a database.

Tasks

- [x] 2 Two roles of administrators and customers

- [x] 3 Dynamic Navigation and layout
	- [x] not logged in links 
		- [x] Home
		- [x] Register
		- [x] Login
	- [x] customers links
		- [x] Home (/Index)
		- [x] My Details (/Customers/MyDetails)
		- [x] Book a Room
		- [x] Search Rooms
		- [x] My Bookings
		- [x] Logout (default)
	- [ ] admin links
		- [x] Home
		- [ ] Manage Bookings
		- [ ] Statistics
		- [x] Logout (default)

- [x] 4 Home Page
	- [x] Images
	- [x] Carosel Tags
	- [x] Columns/Links

- [x] 5 Models
	- [x] Models (Room, Customer, Booking)
	- [x] Add navigation property
	- [x] Define Primary key and foreign key
	- [x] Relationship(1 - m)
	- [x] Add data to Database
	- [x] Validation (Regular expression, data Type)

- [ ] 6  Customer Pages 
	- [x] Authorise pages (customers role into 6.1,2,3,4)
	- [x] 6.1 My Details (Customers/MyDetails)
		- [x] Configure first registered or not (not means existed customer update details)
		- [x] Input: Surname, Given Name, Postcode
		- [x] Success notification (Simple, text below)
		- [x] Create detail data into "/Customers/Index/"
		- [X] Redirect from register to this page
  	- [ ] 6.2 Search Rooms (Rooms)
		- [ ] Build form - Number of Beds (select tag helper) , Check in Date, check out date, submit (tag helpers) 
		- [ ] booking View Model
		- [ ] Raw SQL
		- [ ] Show Table
		- [ ] NOT REQUIRED (add booking link)
	- [ ] 6.3 Book a Room (Rooms)
		- [ ]  create booking form - Room id, check in, check out, submit
		- [ ] view model for this
		- [ ] Raw SQL
		- [ ] notification
	- [ ] 6.4 My Bookings
		- [ ] Table SurName, given name, roomID, Checkin date, checkout date, Cost (this customer only)
		- [ ] Toggle order Checkin and Cost

- [ ] 7 Admin Pages 
	- [ ] Authorise pages
	- [ ] 7.1 Mangage Bookings
		- [ ] create linnk
		- [ ] Table - RoomID, Customer Surname, Customer Given Name, Checking in Date, Checking out Date, and Cost
		- [ ] Links (delete detils)
		- [ ] create page 
			- [ ] room id
			- [ ] Customer drop down with Full name
			- [ ] check in
			- [ ] check out
			- [ ] cost (manual)
			- [ ] submit
			- [ ] CHeck if aviable Raw SQL
			- [ ] succes notification
			- [ ] redirect (return page maybe ?)
		- [ ] delete page
			- [ ] details
			- [ ] Delete confirmation
	- [ ] 7.2 Statistics
		- [ ] Table How many customers are located in each postcode. 'Postcode' and 'Number of Customers
		- [ ] Table How many bookings have been made for each room. 'Room ID' and 'Number of Bookings'
