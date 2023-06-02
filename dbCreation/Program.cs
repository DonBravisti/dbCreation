﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace dbCreation
{
    class Program
    {
        static void Main(string[] args)
        {
			string connStr = "Database=; Data Source=localhost; User Id=root; Password=Aa123456;";
			MySqlConnection connection = new MySqlConnection(connStr);

			try
			{
				connection.Open();
				Console.WriteLine("opened...");

				string query = "CREATE SCHEMA IF NOT EXISTS `my_db`;" +
								"USE my_db";
				MySqlCommand command = new MySqlCommand(query, connection);
				command.ExecuteNonQuery();

				string queryCreateDocs = "CREATE TABLE IF NOT EXISTS documents (" +
					"id INT PRIMARY KEY AUTO_INCREMENT," +
					"title VARCHAR(255) NOT NULL)";
				command = new MySqlCommand(queryCreateDocs, connection);
				command.ExecuteNonQuery();

				string queryCreateDeps = "CREATE TABLE IF NOT EXISTS departments (" +
					"id INT PRIMARY KEY AUTO_INCREMENT," +
					"title VARCHAR(255) NOT NULL," +
					"document_id int," +
					"FOREIGN KEY(document_id) REFERENCES documents(id))";
				command = new MySqlCommand(queryCreateDeps, connection);
				command.ExecuteNonQuery();

				string queryCreateUsers = "CREATE TABLE IF NOT EXISTS users (" +
					"id INT PRIMARY KEY AUTO_INCREMENT," +
					"name VARCHAR(255) NOT NULL," +
					"department_id int,"+
					"FOREIGN KEY(department_id) REFERENCES departments(id))";
				command = new MySqlCommand(queryCreateUsers, connection);
				command.ExecuteNonQuery();



			}
			catch (MySqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			finally
			{
				connection.Close();
				Console.WriteLine("closed...");
			}
        }
    }
}