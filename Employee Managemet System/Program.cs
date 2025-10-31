using System;
using System.ComponentModel;
using MySql.Data.MySqlClient;
using System.Data;
using Employee_Managemet_System;
namespace EmployeeManagementSystem
{

    class Programs
    {

        public static void Main(string[] args)
        {
            

            Finance f = new Finance();
            Console.Write("Enter number of customers: ");
            int count = int.Parse(Console.ReadLine());

            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"\nCustomer {i + 1}:");
                Console.Write("ID: ");
                int id = int.Parse(Console.ReadLine());
                Console.Write("Name: ");
                string name = Console.ReadLine();
                Console.Write("Initial Balance: ");
                decimal balance = decimal.Parse(Console.ReadLine());

                //Customer c = new Customer(id, name, balance);
                //f.AddCustomer(c;)

                Console.WriteLine("\nEnter Transaction Details:");
                Console.Write("Transaction ID: ");
                int tid = int.Parse(Console.ReadLine());
                Console.Write("Customer ID: ");
                int cid = int.Parse(Console.ReadLine());
                Console.Write("Type (Credit/Debit): ");
                string type = Console.ReadLine();
                Console.Write("Amount: ");
                decimal amt = decimal.Parse(Console.ReadLine());

                Transaction txn = new Transaction(tid, cid, type, amt);
                f.RecordTransaction(txn);

            }
           
        }
    }
}
