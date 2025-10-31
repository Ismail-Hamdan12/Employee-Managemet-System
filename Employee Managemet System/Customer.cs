using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Employee_Managemet_System
{
    public class Customer
    {
        public decimal balance;
         public int CustomerID;
         public string CustomerName;
         public decimal AccountBalance;

        public Customer(int id, string name, decimal balance)
        {
            CustomerID = id;
            CustomerName = name;
            AccountBalance = balance;
        }
        public void Deposit(decimal amount)
        {
            balance += amount;
        }
        
        public void Withdraw(decimal amount)
        {
            if (amount <= balance)
            {
                balance -= amount;
            }
            else
            {
                Console.WriteLine("Insufficient balance.");
            }
        }
        public void DisplayBalance()
        {
            Console.WriteLine($"Customer ID: {CustomerID}, Name: {CustomerName}, Balance: {balance}");
        }
        public int GetID() => CustomerID;
        public string GetName() => CustomerName;
        public decimal GetBalance() => balance;
    }

    public class Transaction 
    {
        public int TransactionID { get; set; } 
        public int CustomerID { get; set; }
        public int TransactionId { get; }
        public int CustomerId { get; }
        public string Type { get; set; } 
        public decimal Amount { get; set; } 
        public DateTime Date { get; set; } = DateTime.Now;

        public Transaction(int id, int custId, string type, decimal amount)
        {
            TransactionId = id;
            CustomerId = custId;
            Type = type;
            Amount = amount;
            Date = DateTime.Now;
        }

        public void DisplayTransaction()
        {
            Console.WriteLine($"{Date} | {Type} |{Amount}");
        }
    }

    public class Finance 
    {
        private List<Customer> customers = new List<Customer>();
        private List<Transaction> transactions = new List<Transaction>();

        private string connectionString = "server=localhost;user=root;password=root;database=testdb;";
        public void AddCustomer(Customer customer)
        {
            customers.Add(customer);

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO customers (CustomerID, CustomerName, AccountBalance) VALUES (@CustomerID, @CustomerName, @AccountBalance)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CustomerID", customer.GetID());
                cmd.Parameters.AddWithValue("@CustomerName", customer.GetName());
                cmd.Parameters.AddWithValue("@AccountBalance", customer.GetBalance());
                cmd.ExecuteNonQuery();
            }
        }
        public void RecordTransaction(Transaction t)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                // Check if CustomerId exists
                string checkCustomer = "SELECT COUNT(*) FROM customers WHERE CustomerId = @id";
                MySqlCommand checkCmd = new MySqlCommand(checkCustomer, conn);
                checkCmd.Parameters.AddWithValue("@id", t.CustomerId);
                long exists = (long)checkCmd.ExecuteScalar();

                if (exists == 0)
                {
                    Console.WriteLine("⚠️ Customer not found. Please add the customer first.");
                    return;
                }

                // Insert the transaction
                string insertTxn = "INSERT INTO transactions (TransactionId, CustomerId, Type, Amount, Date) VALUES (@tid, @cid, @type, @amt, @date)";
                MySqlCommand cmd = new MySqlCommand(insertTxn, conn);
                cmd.Parameters.AddWithValue("@tid", t.TransactionId);
                cmd.Parameters.AddWithValue("@cid", t.CustomerId);
                cmd.Parameters.AddWithValue("@type", t.Type);
                cmd.Parameters.AddWithValue("@amt", t.Amount);
                cmd.Parameters.AddWithValue("@date", t.Date);
                cmd.ExecuteNonQuery();

                // Update the customer balance based on Credit/Debit
                string updateQuery = "";

                if (t.Type == "Credit")
                {
                    updateQuery = "UPDATE customers SET AccountBalance = AccountBalance + @amt WHERE CustomerId = @cid";
                }
                else if (t.Type == "Debit")
                {
                    updateQuery = "UPDATE customers SET AccountBalance = Balance - @amt WHERE CustomerId = @cid";
                }

                if (updateQuery != "")
                {
                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@amt", t.Amount);
                    updateCmd.Parameters.AddWithValue("@cid", t.CustomerId);
                    updateCmd.ExecuteNonQuery();
                }

                Console.WriteLine("✅ Transaction recorded successfully!");
            }
        }
        public void DisplayAllCustomers()
        {
            foreach (var customer in customers)
            {
                customer.DisplayBalance();
            }
        }

    }

}
