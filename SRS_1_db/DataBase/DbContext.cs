using MySql.Data.MySqlClient;

namespace SRS_1_db.DataBase
{
    public class DbContext : IDisposable
    {
        readonly protected MySqlConnection _firstConnection;
        readonly protected MySqlConnection _secondConnection;
        readonly protected MySqlConnection _rootConnection;       
        public MySqlConnection FirstConnection { get { return _firstConnection; } }
        public MySqlConnection SecondConnection { get { return _secondConnection; } }
        public DbContext()
        {
            Console.WriteLine("Подготавливаю базу данных...");

            _firstConnection = new MySqlConnection(getConnectionString("user_1", "1"));
            _secondConnection = new MySqlConnection(getConnectionString("user_2", "1"));
            _rootConnection = new MySqlConnection(getConnectionString("root", ""));

            _firstConnection.Open();
            _secondConnection.Open();
            _rootConnection.Open();

            Console.WriteLine("Создано 3 подключения.");

            ReturnDataBaseState();
            Console.WriteLine("База данных подготовлена!");
        }
        private void ReturnDataBaseState()
        {
            var command = _firstConnection.CreateCommand();
            command.CommandText = @"
DROP DATABASE IF EXISTS `music`;
CREATE DATABASE `music`;
USE `music`;
";
            using (var sr = new StreamReader("MusicDamp.sql"))
            {
                string damp = sr.ReadToEnd();
                command.CommandText += damp;
            }
            command.ExecuteNonQuery();
        }
        private string getConnectionString(string user, string password) => $"Server=localhost;Database=music;user={user};password={password}";
        public void Dispose()
        {
            _firstConnection.Close();
            _secondConnection.Close();
            _rootConnection.Close();
            Console.WriteLine("Подключения закрыты!");
        }
    }
}
