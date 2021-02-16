using System;
using System.Collections.Generic;
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
                IEnumerable<Employee> emps = GetAllEmployees(con);
                ShowEmployees(emps);

                foreach (var em in emps)
                {
                    var cmd = con.CreateCommand();
                    cmd.CommandText = "UPDATE Employees SET BirthDate=@newBDate WHERE EmployeeID=@id";
                    cmd.Parameters.AddWithValue("@id", em.Id);
                    cmd.Parameters.AddWithValue("@newBDate", em.BirthDate.AddYears(1));
                    int affectedRows = cmd.ExecuteNonQuery();

                    if (affectedRows == 0)
                        Console.WriteLine($"{em.FirstName} wurde nicht jünger gemacht");
                    else if (affectedRows == 1)
                        Console.WriteLine($"{em.FirstName} wurde jünger gemacht");
                    else
                        Console.WriteLine("PANIK!!!");

                }

                ShowEmployees(GetAllEmployees(con));


                //SearchEmployee(con);

            } // con.Dispose(); // --> con.Close();


            Console.WriteLine("Ende");
            Console.ReadLine();
        }

        private static void ShowEmployees(IEnumerable<Employee> emps)
        {
            foreach (var em in emps)
            {
                Console.WriteLine($"{em.Id} {em.FirstName} {em.LastName} {em.BirthDate:d}");
            }
        }

        private static void SearchEmployee(SqlConnection con)
        {
            Console.WriteLine("Suche: ");
            string suche = Console.ReadLine();

            var cmd = con.CreateCommand();
            //BÖSE wegen SQL Injections: ';CREATE DATABASE HACKED;--
            //cmd.CommandText = "SELECT * FROM Employees WHERE FirstName LIKE '" + suche + "%'"; 

            cmd.CommandText = "SELECT * FROM Employees WHERE FirstName LIKE @search";
            cmd.Parameters.AddWithValue("@search", suche + "%");

            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var fName = reader.GetString(reader.GetOrdinal("FirstName"));
                var lName = reader.GetString(reader.GetOrdinal("LastName"));
                var bdate = reader.GetDateTime(reader.GetOrdinal("BirthDate"));
                Console.WriteLine($"{fName} {lName} {bdate:d}");
            }
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

        private static IEnumerable<Employee> GetAllEmployees(SqlConnection con)
        {
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * FROM Employees";
            var result = new List<Employee>();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var employee = new Employee();
                    employee.Id = reader.GetInt32(0);
                    employee.LastName = reader.GetString(1);
                    employee.FirstName = (string)reader["FirstName"];
                    employee.BirthDate = reader.GetDateTime(reader.GetOrdinal("BirthDate"));
                    result.Add(employee);
                }
            } //.. -> reader.Closer();
            return result;
        }
    }

    class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
