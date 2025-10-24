# E-commerce Application (ASP.NET Core MVC & MySQL)

## Project Title
**EcommerceApp: Minimal Product Catalog and Server-Side Cart**

## Description
This project is a minimal implementation of an e-commerce platform built using **ASP.NET Core 8 MVC** and **Entity Framework Core**. It demonstrates server-side logic for maintaining a persistent shopping cart using a **MySQL** database.

### Key Features:
* **Persistent Shopping Cart:** Cart items are stored in the database (`CartItems` table), providing persistence across sessions (though authentication is not implemented).
* **MySQL Integration:** The application is configured to connect to an external MySQL database via the Pomelo MySQL Provider.
* **Product Seeding:** Products are automatically added to the database via EF Core Migrations upon setup.
* **CRUD Operations (Cart):** Supports adding items to the cart and clearing the entire cart.

## Installation Steps (Prerequisites)

To run this project, you need the following installed on your machine:

1.  **.NET 8 SDK:** Download and install the latest .NET 8 SDK.
2.  **Visual Studio or VS Code:** A suitable IDE for C# development.
3.  **MySQL Server:** A running instance of MySQL (LocalHost or Remote) or a suitable MySQL/MariaDB compatible database.
4.  **NuGet Packages:** The project relies on the following key packages:
    * `Microsoft.EntityFrameworkCore.Tools`
    * `Pomelo.EntityFrameworkCore.MySql` (Used to connect to MySQL)

## Configuration and How to Run the Project

Follow these steps to set up the database connection and run the application:

### Step 1: Configure Database Connection
1.  Open **`appsettings.json`** (or `appsettings.Development.json`).
2.  Ensure the `ConnectionStrings:DefaultConnection` entry is set correctly for your local MySQL instance:

    ```json
    "ConnectionStrings": {
      "DefaultConnection": "server=localhost;port=3306;database=EcommerceShopDB;user=root;password=yourpassword"
    }
    ```
    **(Replace `yourpassword` with the password for the `root` user or a dedicated database user.)**

### Step 2: Install EF Core Tools (If necessary)
If you encounter a `dotnet ef command not found` error, install the global tool:
```bash
dotnet tool install --global dotnet-ef
```
### Step 3: Run Database Migrations
Navigate to the root directory of the EcommerceApp project in your terminal and run the migrations
```bash
# 1. Add/Re-add the migration (ensures MySQL provider is used)
dotnet ef migrations add InitialCreate --output-dir Data/Migrations

# 2. Apply the migration to your MySQL database
dotnet ef database update
```
### Step 4: Run the Application
Start the application from your IDE (Visual Studio) or directly from the terminal:
```bash

dotnet run
```
The application should open in your browser, typically at https://localhost:7257 or an assigned port.

