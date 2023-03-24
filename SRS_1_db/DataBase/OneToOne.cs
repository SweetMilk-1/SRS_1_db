using SRS_1_db.Helpers;
using SRS_1_db.Interfaces;

namespace SRS_1_db.DataBase
{
    /// <summary>
    /// ФАНТОМНОЕ ЧТЕНИЕ (REPEATABLE READ, SERIALIZABLE)
    /// </summary>
    public class OneToOne:ITransactions
    {
        private readonly DbContext _dbContext;
        private readonly ArraysHelper _arrayHelper;
        public OneToOne(DbContext dbContext, ArraysHelper arrayHandler)
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
INSERT INTO video_clips(id, file_path, release_date, direct_by) VALUES (
    40,
    'videos/dreamin.mp4',
    '2022-11-04',
    'Andrey03Rus'
);
UPDATE musics SET has_video_clip = 1 WHERE id = 40;
";
            command.ExecuteNonQuery();
            Thread.Sleep(50);
            command.CommandText=$@"
{(isCommited?"COMMIT;":"ROLLBACK;")};
";
            command.ExecuteNonQuery();
            _arrayHelper.FirstArray.Add(isCommited?1:0);
        }
        public void TransactionB1()
        {
            var command = _dbContext.SecondConnection.CreateCommand();
            Thread.Sleep(20);
            command.CommandText = $@"
SET TRANSACTION ISOLATION LEVEL REPEATABLE READ;
START TRANSACTION;
SELECT has_video_clip FROM musics WHERE name LIKE '%Dreamin%';
COMMIT;
";
            int first = (int)(command.ExecuteScalar() ?? -1);
            _arrayHelper.SecondArray.Add(first);
        }
        public void TransactionB2()
        {
            var command = _dbContext.SecondConnection.CreateCommand();
            Thread.Sleep(20);
            command.CommandText = $@"
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
START TRANSACTION;
SELECT has_video_clip FROM musics WHERE name LIKE '%Dreamin%';
COMMIT;
";
            int first = (int)(command.ExecuteScalar() ?? -1);
            _arrayHelper.SecondArray.Add(first);
        }
        public void ReturnState()
        {
            var command = _dbContext.FirstConnection.CreateCommand();
            command.CommandText = @"
DELETE FROM video_clips WHERE direct_by LIKE '%Andrey03Rus%';
UPDATE musics SET has_video_clip = 0 WHERE id = 40;
";
            command.ExecuteNonQuery();
            return;
        }

        public void PrintTransactions()
        {
            string s = @"
1)
START TRANSACTION;
INSERT INTO video_clips(id, file_path, release_date, direct_by) VALUES (
    40,
    'videos/dreamin.mp4',
    '2022-11-04',
    'Andrey03Rus'
);
UPDATE musics SET has_video_clip = 1 WHERE id = 40;
/*Спим*/
COMMIT | ROLLBACK;

2)
SET TRANSACTION ISOLATION LEVEL <REAPEATABLE READ | SERIALIZABLE>;
START TRANSACTION;
SELECT has_video_clip FROM musics WHERE name LIKE '%Dreamin%';
COMMIT;
";
            Console.WriteLine(s);
        }
    }
}



