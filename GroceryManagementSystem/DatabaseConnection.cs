using System;
using System.Data;
using System.Data.SqlClient;

namespace GroceryManagementSystem
{
    public class DatabaseConnection
    {
        // Replace with your actual connection string from Server Explorer.
        private string connectionString = @"Data Source=localhost;Initial Catalog=GroceryDB;Integrated Security=True;TrustServerCertificate=True";

        public DatabaseConnection()
        {
        }

        public DatabaseConnection(string connString)
        {
            connectionString = connString;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        /// <summary>
        /// Executes a SELECT query and returns a DataTable.
        /// </summary>
        public DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    try
                    {
                        conn.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle or log exception
                        throw new Exception("Error executing query.", ex);
                    }
                }
            }
            return dataTable;
        }

        /// <summary>
        /// Executes an INSERT, UPDATE, or DELETE query and returns the number of rows affected.
        /// </summary>
        public int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    try
                    {
                        conn.Open();
                        rowsAffected = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // Handle or log exception
                        throw new Exception("Error executing non-query.", ex);
                    }
                }
            }
            return rowsAffected;
        }
    }
}
