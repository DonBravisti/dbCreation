using System;
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
			string connectionString = "Database=; Data Source=localhost; User Id=root; Password=Aa123456;";
			MySqlConnection connection = new MySqlConnection(connectionString);
            Console.WriteLine("hey");
            Console.WriteLine("hey2");
            try
			{
				connection.Open();
				Console.WriteLine("opened...");

				string queryCreateDB = "CREATE SCHEMA IF NOT EXISTS `my_db`;" +
									   "USE my_db";
				MySqlCommand command = new MySqlCommand(queryCreateDB, connection);
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

				string queryInsert = @"INSERT INTO `my_db`.`documents` (`title`) VALUES ('doc1');
									   INSERT INTO `my_db`.`documents` (`title`) VALUES ('doc2');

									   INSERT INTO `my_db`.`departments` (`title`, `document_id`) VALUES ('dep1', '1');
									   INSERT INTO `my_db`.`departments` (`title`, `document_id`) VALUES ('dep2', '2');

									   INSERT INTO `my_db`.`users` (`name`, `department_id`) VALUES ('ted', '1');
									   INSERT INTO `my_db`.`users` (`name`, `department_id`) VALUES ('barney', '2');";
				command = new MySqlCommand(queryInsert, connection);
				command.ExecuteNonQuery();

				string queryUpdate = @"UPDATE `my_db`.`documents` SET `title` = 'docdo' WHERE (`id` = '1');";
                command = new MySqlCommand(queryUpdate, connection);
                command.ExecuteNonQuery();

				string querySelect = @"SELECT
									   u.id as ""id пользователя"",
									   u.name as ""Имя"",
									   dep.title as ""Отдел"",
									   doc.title as ""Расписание""
									   FROM my_db.users as u
									   join my_db.departments as dep on dep.id = u.department_id
									   join my_db.documents as doc on doc.id = dep.document_id";
                command = new MySqlCommand(querySelect, connection);
                MySqlDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					object id = reader.GetValue(0);
                    object name = reader.GetValue(1);
                    object dep = reader.GetValue(2);
                    object doc = reader.GetValue(3);

					string result = string.Format("{0} {1} {2} {3}", id, name, dep, doc);
                    Console.WriteLine(result);
                }
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
