# AspNetCore_MVC_Project
Food Ordering System
This Food Ordering System is a web application developed for ordering food. It allows users to browse a variety of food categories, select items, add them to a cart, place orders, and make payments. This system also includes user registration, login, and remember me functionality. Administrators can manage the food items and view orders.

# Features
## User Features
User Registration: Users can register by providing their information, and the system validates the input.

User Login: Registered users can log in with their credentials.

Remember Me: The "Remember Me" functionality allows users to stay logged in even after closing the browser.

Browse Menu: Users can explore a variety of food categories, including South Indian, Chinese, Punjabi, Gujarati, Cold Drinks, and Fast Food.

Add to Cart: Users can add food items to their cart, specifying the quantity (with validation to ensure it's greater than 0).

Cart Management: Users can view and manage the items in their cart, including deleting items.

Place Order: Users can place orders, and the system calculates the total cost based on the items and quantities.

Order Confirmation: Users are prompted to provide their name, address, and mobile number when placing an order. They can choose between online payment and cash on delivery.

Online Payment: Integration with online payment systems for secure payments.

Email Confirmation: After placing an order, users receive a confirmation email with detailed bill information.

## Administrator Features
Admin Login: Administrators can log in to the system to access administrative features.

Item Management: Administrators can view, edit, and delete food items from the system.

# Application Architecture
The Food Ordering System follows a three-tier architecture, separating the presentation layer (frontend), application logic (backend), and data storage (database). This design ensures modularity, maintainability, and scalability for future enhancements.

# How to Run
Clone this repository to your local machine.

Open the solution in Microsoft Visual Studio.

Migrate the database

Create the Admin using 'CreateAdminOneTimeController'


# Technology Stack
Frontend: HTML, CSS, Bootstrap, JavaScript
Backend: C#, ASP.NET
Database: SQL (Microsoft SQL Server)


# User Roles
Customer (User): Users can browse, order, and pay for food items.

Administrator: Administrators can manage food items and view orders.

# System Overview
The Food Ordering System simplifies food ordering and delivery. Users can register, browse, order, and pay for their favorite food items. Administrators can manage the system's food items.

