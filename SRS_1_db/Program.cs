using SRS_1_db.DataBase;
using SRS_1_db.Helpers;
using SRS_1_db.Interfaces;


using (var DbContext = new DbContext())
{
    while (true)
    {
        Console.Clear();
        var ArraysHelper = new ArraysHelper();
        ITransactions transactions = null;
        string name1 = "", name2 = "";


        while (true)
        {
            Console.WriteLine("Выберете операцию:\n0 - отмена\n1 - вставка N-M (READ UNCOMMITTED, READ COMMITTED)\n2 - вставка 1-N (READ COMMITTED, REPEATABLE READ)\n3 - вставка 1-1 вставка 1-1 (REPEATABLE READ, SERIALIZABLE)");
            var val = Console.ReadLine();
            bool isBreak = false;
            switch (val)
            {
                case "1":
                    transactions = new ManyToMany(DbContext, ArraysHelper);
                    name1 = "READ UNCOMMITTED";
                    name2 = "READ COMMITTED";
                    isBreak = true;
                    break;
                case "2":
                    transactions = new OneToMany(DbContext, ArraysHelper);
                    name1 = "READ COMMITTED";
                    name2 = "REPEATABLE READ";
                    isBreak= true;
                    break;
                case "3":
                    transactions = new OneToOne(DbContext, ArraysHelper);
                    name1 = "REPEATABLE READ";
                    name2 = "SERIALIZABLE";
                    isBreak = true;
                    break;

                case "0":
                    return;
                default:
                    break;
            }
            if (isBreak) break;
        }
        Console.Clear();

        transactions.PrintTransactions();

        ThreadHelper.SendQueriesToDb(name1, transactions.TransactionA,
            transactions.TransactionB1, transactions.ReturnState);
        ArraysHelper.Compare();
        ArraysHelper.ClearArrays();

        ThreadHelper.SendQueriesToDb(name2, transactions.TransactionA,
            transactions.TransactionB2, transactions.ReturnState);
        ArraysHelper.Compare();
        ArraysHelper.ClearArrays();

        Console.WriteLine("Нажмите <Enter> для продолжения...");
        Console.ReadLine();
    }
}
