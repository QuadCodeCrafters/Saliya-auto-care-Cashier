using System;
using System.Data.SqlClient;
using System.Diagnostics;

public class DatabaseConnectionMS
{
    private readonly SqlConnection connection;

    //public string connectionString = "your MS SQL connectionString";

    public string connectionString = "your MYSQL WORKBENCH connectionString";

    public DatabaseConnectionMS()
    {
        connection = new SqlConnection(connectionString);
    }

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
            Debug.WriteLine($"Error opening connection: {ex.Message}");
            throw new InvalidOperationException("Could not establish a connection to the database.", ex);
        }
    }

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
            Debug.WriteLine($"Error closing connection: {ex.Message}");
        }
    }

    public bool TestConnection()
    {
        try
        {
            GetConnection();
            CloseConnection();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
