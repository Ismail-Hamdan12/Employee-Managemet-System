# Employee Managemet System
# 💰 C# Finance Management System (CRUD + MySQL)

A simple console-based finance management project built using **C# (.NET)** and **MySQL**.  
Demonstrates basic CRUD operations, OOP concepts, and ERP-style financial transactions.

---

## 🚀 Features
- Create, Update, and Manage Customers
- Record Credit/Debit Transactions
- Auto-Update Customer Balances
- View Transaction History
- Connected to MySQL Database

---

## 🛠️ Technologies
- C# (.NET Console App)
- MySQL (Localhost)
- ADO.NET / MySql.Data library

---

## 🧱 Database Schema

```sql
CREATE TABLE customers (
  CustomerId INT AUTO_INCREMENT PRIMARY KEY,
  Name VARCHAR(100),
  AccountBalance DECIMAL(10,2)
);

CREATE TABLE transactions (
  TransactionId INT AUTO_INCREMENT PRIMARY KEY,
  CustomerId INT,
  Type ENUM('Credit','Debit'),
  Amount DECIMAL(10,2),
  Date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (CustomerId) REFERENCES customers(CustomerId)
);
