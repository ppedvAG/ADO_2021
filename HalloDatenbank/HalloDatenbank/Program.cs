using System;
using System.Data.SqlClient;

namespace HalloDatenbank
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hallo Datenbank");

            //string conString = "Server=.;Database=Northwind;Trusted_Connection=true";
            string conString = "Server=(localdb)\\mssqllocaldb;Database=NORTHWND;Trusted_Connection=true";

            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                Console.WriteLine("Datenbankverbindung wurde hergestellt");

                ShowEmployeeCount(con);
                ShowAllEmployees(con);

                Console.WriteLine("Suche: ");
                string suche = Console.ReadLine();

                var cmd = con.CreateCommand();
                //BÖSE wegen SQL Injections: ;CREATE DATABASE HACKED;---
                //cmd.CommandText = "SELECT * FROM Employees WHERE FirstName LIKE '" + suche + "%'"; 

                cmd.CommandText = "SELECT * FROM Employees WHERE FirstName LIKE @search";
                cmd.Parameters.AddWithValue("@search", suche+"%");

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var fName = reader.GetString(reader.GetOrdinal("FirstName"));
                    var lName = reader.GetString(reader.GetOrdinal("LastName"));
                    var bdate = reader.GetDateTime(reader.GetOrdinal("BirthDate"));
                    Console.WriteLine($"{fName} {lName} {bdate:d}");
                }


            } // con.Dispose(); // --> con.Close();


            Console.WriteLine("Ende");
            Console.ReadLine();
        }

        private static void ShowEmployeeCount(SqlConnection con)
        {
            SqlCommand countCmd = new SqlCommand();
            countCmd.Connection = con;
            countCmd.CommandText = "SELECT COUNT(*) FROM Employees";
            object countResultAsObj = countCmd.ExecuteScalar();
            int countAsInt = (int)countResultAsObj;

            //Console.WriteLine("Es sind " + countAsInt.ToString() + " Employees in der Datenbank");
            //Console.WriteLine(string.Format("Es sind {0} Employees in der Datenbank", countAsInt));
            Console.WriteLine($"Es sind {countAsInt} Employees in der Datenbank"); //string interpolation
        }

        private static void ShowAllEmployees(SqlConnection con)
        {
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * FROM Employees";
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string lastName = reader.GetString(1);
                    string firstName = (string)reader["FirstName"];
                    DateTime birthDate = reader.GetDateTime(reader.GetOrdinal("BirthDate"));
                    Console.WriteLine($"{id} {firstName} {lastName} {birthDate:d}");
                }
            } //.. -> reader.Closer();
        }
    }
}
