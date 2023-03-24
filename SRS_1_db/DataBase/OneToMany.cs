using SRS_1_db.Helpers;
using SRS_1_db.Interfaces;

namespace SRS_1_db.DataBase
{
    /// <summary>
    /// Неповторяющееся чтение чтение (READ COMMITTED, REPEATABLE READ)
    /// </summary>
    public class OneToMany:ITransactions
    {
        private readonly DbContext _dbContext;
        private readonly ArraysHelper _arrayHelper;
        public OneToMany(DbContext dbContext, ArraysHelper arrayHandler)
        {
            _dbContext = dbContext;
            _arrayHelper = arrayHandler;
        }
        public void TransactionA()
        {
            Random random = new Random();
            var command = _dbContext.FirstConnection.CreateCommand();
            command.CommandText = $@"
START TRANSACTION ;
INSERT INTO authors(name, description) VALUES (
    'Markul',
    'Хип-хоп-исполнитель, певец, автор песен и бывший член творческого объединения «Green Park Gang».'
);
SELECT LAST_INSERT_ID();
INSERT INTO musics(name, file_path, genre_id, author_id, release_date) VALUES (
    'Стрелы',
    'musics/стрелы.mp3',
    21,
    LAST_INSERT_ID(),
    '2022-11-03'
);
";
            command.ExecuteNonQuery();
            Thread.Sleep(20);
            command.CommandText = @$"
COMMIT;
";
            command.ExecuteNonQuery();
            _arrayHelper.FirstArray.Add(1);
        }
        public void TransactionB1()
        {
            Thread.Sleep(10);
            var command = _dbContext.SecondConnection.CreateCommand();
            command.CommandText = $@"
SET TRANSACTION ISOLATION LEVEL READ COMMITTED; 
START TRANSACTION;
SELECT id FROM authors WHERE name LIKE '%Markul%'";
            int first = (int)(command.ExecuteScalar() ?? -1);
            Thread.Sleep(20);
            command.CommandText = @$"
SELECT id FROM authors WHERE name LIKE '%Markul%';
COMMIT;
";
            int last = (int)(command.ExecuteScalar() ?? -1);
            _arrayHelper.SecondArray.Add(last == first ? 1 : 0);
        }
        public void TransactionB2()
        {
            Random rand = new();

            Thread.Sleep(rand.Next(0, 6));
            var command = _dbContext.SecondConnection.CreateCommand();
            command.CommandText = $@"
SET TRANSACTION ISOLATION LEVEL REPEATABLE READ; 
START TRANSACTION;
SELECT id FROM authors WHERE name LIKE '%Markul%'";
            int first = (int)(command.ExecuteScalar() ?? -1);
            Thread.Sleep(20);
            command.CommandText = @$"
SELECT id FROM authors WHERE name LIKE '%Markul%';
COMMIT;
";
            int last = (int)(command.ExecuteScalar() ?? -1);
            _arrayHelper.SecondArray.Add(last == first ? 1 : 0);
        }

        public void PrintTransactions()
        {
            string s = @"
1)
START TRANSACTION;
INSERT INTO authors(name, description) VALUES (
    'Markul',
    'Хип-хоп-исполнитель, певец, автор песен и бывший член творческого объединения «Green Park Gang».'
);
SELECT LAST_INSERT_ID();
INSERT INTO musics(name, file_path, genre_id, author_id, release_date) VALUES (
    'Стрелы',
    'musics/стрелы.mp3',
    21,
    LAST_INSERT_ID(),
    '2022-11-03'
);
/*Спим*/
COMMIT;

2)
/*Спим*/
SET TRANSACTION ISOLATION LEVEL <READ COMMITTED | REPEATABLE READ>; 
START TRANSACTION;
SELECT id FROM authors WHERE name LIKE '%Markul%';
/*Спим*/
SELECT id FROM authors WHERE name LIKE '%Markul%';
COMMIT;
";

            Console.WriteLine(s);
        }

        public void ReturnState()
        {
            var command = _dbContext.SecondConnection.CreateCommand();
            command.CommandText = $@"
DELETE FROM musics WHERE name LIKE '%Стрелы%';
DELETE FROM authors WHERE name LIKE '%Markul%';
";
            command.ExecuteNonQuery();
        }
    }
}