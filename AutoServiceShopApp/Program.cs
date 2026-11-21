using Microsoft.Data.SqlClient;

namespace AutoServiceShopApp
{
    class Program
    {
        static void Main(string[] args)
        {
            GarageDb db = new GarageDb();

            // Make sure the database exists before showing the menu
            try
            {
                db.CheckConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not connect to AutoServiceShopDB: " + ex.Message);
                return;
            }

            // Main menu loop
            bool running = true;
            while (running)
            {
                Console.WriteLine();
                Console.WriteLine("Auto Service Shop Menu");
                Console.WriteLine("1. View Customers");
                Console.WriteLine("2. Add Customer");
                Console.WriteLine("3. Update Customer");
                Console.WriteLine("4. Delete Customer");
                Console.WriteLine("5. Mechanic Work Order Report");
                Console.WriteLine("6. Exit");
                Console.Write("Select an option: ");

                string choice = Console.ReadLine() ?? "";
                Console.WriteLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            db.GetCustomers();
                            break;

                        case "2":
                            Console.Write("First name: ");
                            string first = Console.ReadLine() ?? "";
                            Console.Write("Last name: ");
                            string last = Console.ReadLine() ?? "";
                            Console.Write("Phone: ");
                            string phone = Console.ReadLine() ?? "";
                            Console.Write("Email: ");
                            string email = Console.ReadLine() ?? "";
                            db.AddCustomer(first, last, phone, email);
                            break;

                        case "3":
                            db.GetCustomers();
                            Console.Write("Customer ID to update: ");
                            if (!int.TryParse(Console.ReadLine(), out int updateId))
                            {
                                Console.WriteLine("Invalid ID.");
                                break;
                            }
                            Console.Write("New phone: ");
                            string newPhone = Console.ReadLine() ?? "";
                            Console.Write("New email: ");
                            string newEmail = Console.ReadLine() ?? "";
                            db.UpdateCustomer(updateId, newPhone, newEmail);
                            break;

                        case "4":
                            db.GetCustomers();
                            Console.Write("Customer ID to delete: ");
                            if (!int.TryParse(Console.ReadLine(), out int deleteId))
                            {
                                Console.WriteLine("Invalid ID.");
                                break;
                            }
                            db.DeleteCustomer(deleteId);
                            break;

                        case "5":
                            db.ShowMechanicReport();
                            break;

                        case "6":
                            running = false;
                            break;

                        default:
                            Console.WriteLine("Invalid option.");
                            break;
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Database error: " + ex.Message);
                }
            }
        }
    }
}
