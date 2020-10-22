# ASP.NET_Core_Project
This is made by Razor page, Showcasing authentication, search function based on DB using raw SQL query, statistics, booking system and different model classes. SQLite is injected as a database.

Tasks

- [x] **2 Two roles of administrators and customers**

- [x] **3 Dynamic Navigation and layout**
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
	- [x] admin links
		- [x] Home
		- [x] Manage Bookings
		- [x] Statistics
		- [x] Logout (default)

- [x] **4 Home Page (/Pages/Index)**
	- [x] Images (4 images /wwwroot/images) 
	- [x] Carosel Tags (bootstrap)
	- [x] Columns/Links

- [x] **5 Models (draw model first)**
	- [x] Models (Room, Customer, Booking)
	- [x] Add navigation property (**important**)
	- [x] Define Primary key and foreign key
	- [x] Relationship(1 - m)
	- [x] Add data to Database
	- [x] Validation (Regular expression, data Type)

- [x] **6  Customer Pages ("customers" authorization)**
	- [x] Authorise pages (customers role into 6.1,2,3,4)
	- [x] 6.1 My Details (Customers/MyDetails)
		- [x] Configure first registered or not (not means existed customer update details)
		- [x] Input: Surname, Given Name, Postcode
		- [x] Success notification (Simple, text below)
		- [x] Create detail data into "/Customers/Index/"
		- [X] Redirect from register to this page
  	- [x] 6.2 Search Rooms ("Search a Room" function is in the Rooms Folder)
		- [x] Build form - Number of Beds (select tag helper) , Check in Date, check out date, submit (tag helpers) 
		- [x] Room Search Model class
		- [x] Raw SQL
	- [x] 6.3 Book a Room ("Book a room" function is in the Bookings Folder)
		- [x] create booking form - Room id, check in, check out, submit
		- [x] view model - "MakeBooking"
		- [x] Raw SQL
		- [x] validation, notification
	- [x] 6.4 My Bookings (Index/Bookings)
		- [x] Table SurName, given name, roomID, Checkin date, checkout date, Cost (this customer only)
		- [x] Toggle order Checkin and Cost (between ascending and descending)
		- [x] Delete "Edit/Delete" in content file, set authorization of "customers"

- [x] **7 Admin Pages (set "Administrators" authorization)**
	- [x] 7.1 Mangage Bookings (/BookingManagement/Bookings)
		- [x] (Content File work) Making a Table - RoomID, Customer Surname, Customer Given Name, Checking in Date, Checking out Date, and Cost
		- [x] Decorate "Edit/Delete" Button boxes (Implement Bootstrap)
		- [x] set delete detils (ID, Level, BedCount, Surname, GivenName, Check In, Out, Total Cost) 
		- [x] "create a new Booking" Link (Create/Bookings)
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
