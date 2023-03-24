using SRS_1_db.Helpers;
using SRS_1_db.Interfaces;

namespace SRS_1_db.DataBase
{
    /// <summary>
    /// Грязное чтение READ UNCOMMITTED, READ COMMITED
    /// </summary>
    public class ManyToMany:ITransactions
    {
        private readonly DbContext _dbContext;
        private readonly ArraysHelper _arrayHelper;
        public ManyToMany(DbContext dbContext, ArraysHelper arrayHandler)
        {
            _dbContext = dbContext;
            _arrayHelper = arrayHandler;
        }
        public async void TransactionA()
        {
            Random random = new Random();

            bool isCommited = random.Next(0, 2) == 0;
            var command = _dbContext.FirstConnection.CreateCommand();
            command.CommandText = $@"
START TRANSACTION;
UPDATE users_musics SET grade = 5 WHERE user_id = 1 AND music_id = 34;
";
            command.ExecuteNonQuery();
            Thread.Sleep(20);
            command.CommandText = $@"
ROLLBACK;
SELECT grade FROM users_musics WHERE user_id = 1 AND music_id = 34;
";
            var first = (int)command.ExecuteScalar();
            _arrayHelper.FirstArray.Add(first);
        }
        public void TransactionB1()
        {
            var command = _dbContext.SecondConnection.CreateCommand();
            command.CommandText = $@"
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
START TRANSACTION;
SELECT grade FROM users_musics WHERE user_id = 1 AND music_id = 34;
COMMIT;
";
            int last = (int)(command.ExecuteScalar() ?? -1);
            _arrayHelper.SecondArray.Add(last);
        }
        public void TransactionB2()
        {
            var command = _dbContext.SecondConnection.CreateCommand();
            command.CommandText = $@"
SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
START TRANSACTION;
SELECT grade FROM users_musics WHERE user_id = 1 AND music_id = 34;
COMMIT;
";
            int last = (int)(command.ExecuteScalar() ?? -1);
            _arrayHelper.SecondArray.Add(last);
        }
        public void ReturnState()
        {
        }

        public void PrintTransactions()
        {
            string s = @"
1)
START TRANSACTION;
UPDATE users_musics SET grade = 5 WHERE user_id = 1 AND music_id = 34;
/*Спим*/
ROLLBACK;
SELECT grade FROM users_musics WHERE user_id = 1 AND music_id = 34;

2)
SET TRANSACTION ISOLATION LEVEL <READ UNCOMMITED | READ COMMITED>;
START TRANSACTION;
SELECT grade FROM users_musics WHERE user_id = 1 AND music_id = 34;
COMMIT;
";
            Console.WriteLine(s);
        }
    }
}