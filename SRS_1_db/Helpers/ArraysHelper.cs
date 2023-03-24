namespace SRS_1_db.Helpers
{
    public class ArraysHelper
    {

        readonly protected List<int> _firstArray;
        readonly protected List<int> _secondArray;
        public List<int> FirstArray { get { return _firstArray; } }
        public List<int> SecondArray { get { return _secondArray; } }
        public ArraysHelper()
        {
            _firstArray = new List<int>();
            _secondArray = new List<int>();
        }
        public void ClearArrays()
        {
            Console.WriteLine("Очищаю выборки...");
            _firstArray.Clear();
            _secondArray.Clear();
        }

        public void Compare()
        {
            Console.WriteLine("Сравниваю выборки...");
            int countMistake = 0;
            for (int i = 0; i < _firstArray.Count; i++)
            {
                if (_firstArray[i] != _secondArray[i])
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{_firstArray[i]}\t{_secondArray[i]}\tWrong");
                    Console.ForegroundColor = ConsoleColor.White;
                    countMistake++;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{_firstArray[i]}\t{_secondArray[i]}\tOK");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            if (countMistake == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Accepted");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
