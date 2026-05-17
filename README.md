# Grocery Management System 🛒

A robust, multi-role desktop application built with C# and Windows Forms to streamline grocery store operations. This system connects Customers, Merchants, and Administrators through a centralized Microsoft SQL Server database, allowing for seamless inventory management and order processing.

## 🌟 Key Features

The application features a secure, role-based authentication system that directs users to their specific dashboards upon login.

### 👤 Customer Features
* *Interactive Shopping:* Browse available grocery products with real-time stock visibility.
* *Virtual Cart:* Add items to a shopping cart with dynamic total calculation.
* *Checkout System:* Enter delivery details and select payment methods (Cash on Delivery, Card, UPI).
* *Order Tracking:* View personal order history directly from the dashboard.

### 🏪 Merchant Features
* *Inventory Management:* Full CRUD (Create, Read, Update, Delete) operations for grocery products.
* *Click-to-Fill UI:* Automatically populate input fields by clicking on a product in the data grid.
* *Order Processing:* View incoming customer orders containing their specific products.
* *Status Updates:* Update order statuses (Pending, Processing, Shipped, Completed) or delete cancelled orders.

### 🛡️ Administrator Features
* *System Oversight:* A tabbed dashboard providing a bird's-eye view of the entire platform.
* *User Management:* View all registered accounts and their assigned roles.
* *Global Order History:* Monitor every order placed across the system.

## 💻 Technologies Used
* *Frontend:* C# Windows Forms (.NET Framework)
* *Backend:* C# Object-Oriented Programming
* *Database:* Microsoft SQL Server (SSMS)
* *Data Access:* ADO.NET (System.Data.SqlClient)

## 🚀 Setup and Installation

Follow these steps to run the project locally on your machine:

*1. Clone the repository:*
bash
git clone [https://github.com/YourUsername/Grocery-Management-System.git](https://github.com/YourUsername/Grocery-Management-System.git)


*2. Setup the Database:*
* Open Microsoft SQL Server Management Studio (SSMS).
* Connect to your local server (e.g., localhost).
* Create a new database named GroceryDB.
* Open the backupdb.sql file provided in this repository and execute the script to instantly create all tables and insert the seed data.

*3. Configure the Application:*
* Open the .slnx file in Visual Studio.
* Open the DatabaseConnection.cs file.
* Update the connection string to match your local SQL Server instance:
  @"Data Source=localhost;Initial Catalog=GroceryDB;Integrated Security=True;TrustServerCertificate=True"

*4. Build and Run:*
* Press Start in Visual Studio. 
* Use the following default credentials to test the roles:
  * *Admin:* admin / 1234
  * *Merchant:* merchant / 1234
  * *Customer:* customer / 1234

## 👨‍💻 Author
*Raihanul Momin Mozumdar*
https://github.com/notetoraihan-stack/Grocery-Management-System
