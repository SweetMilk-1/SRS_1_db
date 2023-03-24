using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRS_1_db.Interfaces
{
    internal interface ITransactions
    {
        void PrintTransactions();
        void TransactionA();
        void TransactionB1();
        void TransactionB2();
        void ReturnState();
    }
}
