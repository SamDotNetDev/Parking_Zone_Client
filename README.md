ABOUT PROJECT ParkingZone

There are 2 parts of the application: admin portal and client portal.
Entities:
These entities exist within the scope of the application: client, parking zone, parking slot, slot category and reservation.
Client is the user of the system who reserves a slot in the application. Each client has a name and phone number.
Parking zone is the branch in the parking management company. There can be multiple parking zones across cities. There are the name, address and date of establishment for parking zones.
Parking slot is the place in the parking zone that can be booked. Each parking slot should at least have a number (set manually) and IsAvailableForBooking properties. Parking slots can be of different slot categories. Each slot category has a different fee per hour. Several parking slots can have the same categories.
Reservation is the action of booking a parking slot for some period by a vehicle owner. Each reservation record should have a parking slot identifier, booking period start and duration (in hours), and vehicle plate numbers.


Used technologies:
.NET 8 (6), ASP.NET Core MVC
Entityframework
MSSQL
Javascript, jQuery, ajax
Github, GitFlow
Github actions
CI
