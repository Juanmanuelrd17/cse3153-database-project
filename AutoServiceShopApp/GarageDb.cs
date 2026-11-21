using Microsoft.Data.SqlClient;

namespace AutoServiceShopApp
{
    public class GarageDb
    {
        // TrustServerCertificate=True is needed for local SQL Server with Microsoft.Data.SqlClient
        private string connectionString = "Server=localhost;Database=AutoServiceShopDB;Trusted_Connection=True;TrustServerCertificate=True;";

        // Test that the database exists
        public void CheckConnection()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
            }
        }

        // Display all customers
        public void GetCustomers()
        {
            string sql = "SELECT CustomerID, FirstName, LastName, Phone, Email FROM garage.Customers ORDER BY CustomerID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine("ID | Name | Phone | Email");
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["CustomerID"]} | {reader["FirstName"]} {reader["LastName"]} | {reader["Phone"]} | {reader["Email"]}");
                        }
                    }
                }
            }
        }

        // Insert a new customer
        public void AddCustomer(string firstName, string lastName, string phone, string email)
        {
            string sql = "INSERT INTO garage.Customers (FirstName, LastName, Phone, Email) VALUES (@firstName, @lastName, @phone, @email)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@firstName", firstName);
                    cmd.Parameters.AddWithValue("@lastName", lastName);
                    cmd.Parameters.AddWithValue("@phone", phone);
                    cmd.Parameters.AddWithValue("@email", email);

                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Customer added.");
                }
            }
        }

        // Update a customer's phone and email by ID
        public void UpdateCustomer(int customerId, string phone, string email)
        {
            string sql = "UPDATE garage.Customers SET Phone = @phone, Email = @email WHERE CustomerID = @id";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@phone", phone);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@id", customerId);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        Console.WriteLine("Customer updated.");
                    }
                    else
                    {
                        Console.WriteLine("Customer not found.");
                    }
                }
            }
        }

        // Delete a customer by ID
        public void DeleteCustomer(int customerId)
        {
            string sql = "DELETE FROM garage.Customers WHERE CustomerID = @id";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", customerId);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        Console.WriteLine("Customer deleted.");
                    }
                    else
                    {
                        Console.WriteLine("Customer not found.");
                    }
                }
            }
        }

        // Show total work orders and revenue for each mechanic
        public void ShowMechanicReport()
        {
            string sql = "SELECT m.FirstName + ' ' + m.LastName AS Mechanic, COUNT(w.WorkOrderID) AS TotalOrders, ISNULL(SUM(w.TotalCost), 0) AS TotalRevenue " +
                         "FROM garage.Mechanics m " +
                         "LEFT JOIN garage.WorkOrders w ON m.MechanicID = w.MechanicID " +
                         "GROUP BY m.MechanicID, m.FirstName, m.LastName " +
                         "ORDER BY TotalRevenue DESC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine("Mechanic | Total Orders | Total Revenue");
                        while (reader.Read())
                        {
                            decimal total = Convert.ToDecimal(reader["TotalRevenue"]);
                            Console.WriteLine($"{reader["Mechanic"]} | {reader["TotalOrders"]} | {total:C}");
                        }
                    }
                }
            }
        }
    }
}
