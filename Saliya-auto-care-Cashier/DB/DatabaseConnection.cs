using System;
using System.Data.SqlClient;
using System.Diagnostics;

public class DatabaseConnectionMS
{
    private SqlConnection connection;   

    //private string connectionString = "Data Source=RAVEEN_LENOVO;Initial Catalog=webEditorData;User ID=saliyaAdmin001;Password=saliya007#"; //MS SQL serve


    public string connectionString = "Server=SACHITHA\\SQLEXPRESS;Database=webEditorData;Integrated Security=True;";

    // Constructor to initialize the connection
    public DatabaseConnectionMS()
    {
        connection = new SqlConnection(connectionString);
    }

    // Method to open the connection
    public SqlConnection GetConnection()
    {
        try
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            return connection;
        }
        catch (Exception ex)
        {
            // Log exception details (You can use any logging framework or Console here)
            Debug.WriteLine($"Error opening connection: {ex.Message}");
            throw new InvalidOperationException("Could not establish a connection to the database.", ex);
        }
    }

    // Method to close the connection
    public void CloseConnection()
    {
        try
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }   
        }
        catch (Exception ex)
        {
            // Log exception details (You can use any logging framework or Console here)
            Debug.WriteLine($"Error closing connection: {ex.Message}");
        }
    }

    // Method to check if the database connection is working
    public bool TestConnection()
    {
        try
        {
            GetConnection(); // Try opening the connection
            CloseConnection(); // Close the connection after successful check
            return true;  // Connection is successful
        }
        catch (Exception)
        {
            return false;  // Connection failed
        }
    }
}
