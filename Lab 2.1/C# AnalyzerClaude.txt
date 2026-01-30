using System;
using System.IO;
using System.Linq;
using System.Globalization;

// 1. Data Model (Equivalent to a dictionary row structure)
public record SalesItem(string ProductName, decimal Price, int Quantity);

public class CsvProcessor
{
    // C# method equivalent to the Python 'calculate_total_sales' function
    public static decimal CalculateTotalSales(string filename)
    {
        // Use decimal for financial accuracy (Python's float is often imprecise)
        decimal total = 0.0m;

        try
        {
            // Read lines from the file, similar to Python's 'with open...'
            // We skip the header row (index 0) immediately.
            var dataLines = File.ReadLines(filename).Skip(1);

            // Process each data line using LINQ
            foreach (string line in dataLines)
            {
                // Equivalent to line.split(',')
                string[] columns = line.Split(',');

                if (columns.Length == 3)
                {
                    // Attempt to parse the values. Use InvariantCulture for consistent decimal parsing.
                    // The 'm' suffix on 0.0m ensures the variable is treated as a decimal.
                    if (decimal.TryParse(columns[1], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price) &&
                        int.TryParse(columns[2], out int quantity))
                    {
                        total += price * quantity;
                    }
                    else
                    {
                        Console.WriteLine($"Skipping malformed data in row: {line}");
                    }
                }
            }
        }
        catch (FileNotFoundException)
        {
            // Python equivalent of handling file open failure
            Console.WriteLine($"Error: The file '{filename}' was not found.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }

        return total;
    }

    // C# Main method equivalent to Python's 'if __name__ == "__main__":' block
    public static void Main(string[] args)
    {
        // Note: For this to run, a file named 'sales_data.csv' must exist
        // in the output directory (e.g., bin/Debug/net8.0/).
        string salesDataFile = "sales_data.csv";
        
        // Example: Create a dummy file if it doesn't exist for testing
        if (!File.Exists(salesDataFile))
        {
            // You should replace this with a real file in a live environment
            File.WriteAllText(salesDataFile, "product_name,price,quantity\nLaptop,1200.00,5\nMouse,25.50,10\n");
        }

        decimal totalSales = CalculateTotalSales(salesDataFile);
        
        // Print with currency formatting, equivalent to Python's f-string formatting
        Console.WriteLine($"Total sales from {salesDataFile}: {totalSales.ToString("C", CultureInfo.CurrentCulture)}");
    }
}
