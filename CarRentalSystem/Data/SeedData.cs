using CarRentalSystem.Models;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace CarRentalSystem.Data;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        CreateDatabaseAndTablesAndDataManually(connectionString);
    }

    private static void CreateDatabaseAndTablesAndDataManually(string connectionString)
    {
        try
        {
            var builder = new MySqlConnectionStringBuilder(connectionString);
            var databaseName = builder.Database;
            builder.Database = ""; 

            using (var connection = new MySqlConnection(builder.ConnectionString))
            {
                connection.Open();
                
                // 1. Create Database
                using (var cmd = new MySqlCommand($"CREATE DATABASE IF NOT EXISTS `{databaseName}` CHARACTER SET utf8;", connection))
                {
                    cmd.ExecuteNonQuery();
                }

                connection.ChangeDatabase(databaseName);

                // 2. Create Tables
                string createUsers = @"CREATE TABLE IF NOT EXISTS Users (
                    Id INT AUTO_INCREMENT PRIMARY KEY,
                    Name VARCHAR(255) NOT NULL,
                    Email VARCHAR(255) NOT NULL,
                    Password VARCHAR(255) NOT NULL,
                    Role VARCHAR(50) NOT NULL
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8;";

                string createCars = @"CREATE TABLE IF NOT EXISTS Cars (
                    Id INT AUTO_INCREMENT PRIMARY KEY,
                    Brand VARCHAR(255) NOT NULL,
                    Model VARCHAR(255) NOT NULL,
                    PricePerDay DECIMAL(18,2) NOT NULL,
                    IsAvailable TINYINT(1) NOT NULL DEFAULT 1,
                    ImageUrl TEXT
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8;";

                string createBookings = @"CREATE TABLE IF NOT EXISTS Bookings (
                    Id INT AUTO_INCREMENT PRIMARY KEY,
                    UserId INT NOT NULL,
                    CarId INT NOT NULL,
                    StartDate DATETIME NOT NULL,
                    EndDate DATETIME NOT NULL,
                    TotalAmount DECIMAL(18,2) NOT NULL,
                    Status VARCHAR(50) NOT NULL
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8;";

                string createPayments = @"CREATE TABLE IF NOT EXISTS Payments (
                    Id INT AUTO_INCREMENT PRIMARY KEY,
                    BookingId INT NOT NULL,
                    Amount DECIMAL(18,2) NOT NULL,
                    PaymentDate DATETIME NOT NULL,
                    Status VARCHAR(50) NOT NULL
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8;";

                foreach (var sql in new[] { createUsers, createCars, createBookings, createPayments })
                {
                    using (var cmd = new MySqlCommand(sql, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                // 3. Seed Data (Only if empty)
                using (var checkCmd = new MySqlCommand("SELECT COUNT(*) FROM Users", connection))
                {
                    if (Convert.ToInt32(checkCmd.ExecuteScalar()) == 0)
                    {
                        Console.WriteLine("[SeedData] Seeding initial data...");
                        // Seed Admin
                        using (var cmd = new MySqlCommand("INSERT INTO Users (Name, Email, Password, Role) VALUES ('Admin User', 'admin@carrental.com', 'admin', 'Admin')", connection))
                        {
                            cmd.ExecuteNonQuery();
                        }

                        // Seed Cars with various categories
                        string[] carSeeds = new[] {
                            "INSERT INTO Cars (Brand, Model, PricePerDay, IsAvailable, ImageUrl) VALUES ('Tesla', 'Model 3', 150.00, 1, 'https://images.unsplash.com/photo-1560958089-b8a1929cea89')",
                            "INSERT INTO Cars (Brand, Model, PricePerDay, IsAvailable, ImageUrl) VALUES ('BMW', 'X5', 120.00, 1, 'https://images.unsplash.com/photo-1555215695-3004980ad54e')",
                            "INSERT INTO Cars (Brand, Model, PricePerDay, IsAvailable, ImageUrl) VALUES ('Toyota', 'Camry', 60.00, 1, 'https://images.unsplash.com/photo-1621007947382-bb3c3994e3fb')",
                            "INSERT INTO Cars (Brand, Model, PricePerDay, IsAvailable, ImageUrl) VALUES ('Audi', 'A4', 95.00, 1, 'https://images.unsplash.com/photo-1541348263662-e0c8de42d1ee')",
                            "INSERT INTO Cars (Brand, Model, PricePerDay, IsAvailable, ImageUrl) VALUES ('Mercedes', 'S-Class', 250.00, 1, 'https://images.unsplash.com/photo-1618843479313-40f8afb4b4d8')",
                            "INSERT INTO Cars (Brand, Model, PricePerDay, IsAvailable, ImageUrl) VALUES ('Ford', 'Mustang', 180.00, 1, 'https://images.unsplash.com/photo-1584345604481-03bd1a35074b')",
                            "INSERT INTO Cars (Brand, Model, PricePerDay, IsAvailable, ImageUrl) VALUES ('Lamborghini', 'Urus', 500.00, 1, 'https://images.unsplash.com/photo-1571607388263-1044f9ea01dd')"
                        };

                        foreach (var carSql in carSeeds)
                        {
                            using (var cmd = new MySqlCommand(carSql, connection))
                            {
                                cmd.ExecuteNonQuery();
                            }
                        }
                        Console.WriteLine("[SeedData] Data seeded successfully with 7 premium cars.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[SeedData] Manual setup failed: {ex.Message}");
        }
    }
}
