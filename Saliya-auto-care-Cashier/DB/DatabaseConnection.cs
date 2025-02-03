using System;
using System.Data.SqlClient;

public class DatabaseConnectionMS
{
    private static DatabaseConnectionMS instance;  // Singleton instance
    private static readonly object locks = new object();
    private SqlConnection connection;

    //private string connectionString = "Data Source=RAVEEN_LENOVO;Initial Catalog=webEditorData;User ID=saliyaAdmin001;Password=saliya007#"; //MS SQL serve

    private string connectionString = "Server=YOUR_SERVER;Database=YOUR_DATABASE;User Id=YOUR_USERNAME;Password=YOUR_PASSWORD;";

    // Private constructor prevents direct instantiation
    private DatabaseConnectionMS()
    {
        connection = new SqlConnection(connectionString);
    }

    // Public static method to get the instance
    public static DatabaseConnectionMS Instance
    {
        get
        {
            if (instance == null)
            {
                lock (locks)  // Thread-safe singleton
                {
                    if (instance == null)
                    {
                        instance = new DatabaseConnectionMS();
                    }
                }
            }
            return instance;
        }
    }

    // Method to open the connection
    public SqlConnection GetConnection()
    {
        if (connection.State == System.Data.ConnectionState.Closed)
        {
            connection.Open();
        }
        return connection;
    }

    // Method to close the connection
    public void CloseConnection()
    {
        if (connection.State == System.Data.ConnectionState.Open)
        {
            connection.Close();
        }
    }
}
