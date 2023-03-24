using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SRS_1_db.Helpers
{
    public delegate void ClearDelegate();
    public static class ThreadHelper
    {
        private static int CountOperation = 100;

        /// <summary>
        /// Создает 2 потока и выполняет операции
        /// </summary>
        /// <param name="name">Подпись</param>
        /// <param name="firstOperation">первая операция</param>
        /// <param name="secondOperation">вторая операция</param>
        /// <param name="clear">вернуть результат</param>
        public static void SendQueriesToDb(string name, ThreadStart firstOperation,
            ThreadStart secondOperation, ClearDelegate clear)
        {
            Console.WriteLine($"Запускаю 2 потока ({name})...");

            for (int i = 0; i < CountOperation; i++)
            {
                Thread thread1 = new Thread(firstOperation);
                Thread thread2 = new Thread(secondOperation);

                thread1.Start();
                thread2.Start();

                thread1.Join();
                thread2.Join();

                clear();
            }

            Console.WriteLine("Выполнение закончилось!");
        }
    }
}
