using System.Diagnostics;

namespace ParallelCalculations
{
    public class Program
    {
        static int _arraySize = 10000000;
        static int _numThreads = 4;

        // ToDo: сделать рефакторинг
        static void Main(string[] args)
        {
            if (args != null && args.Length == 1 && !int.TryParse(args[0], out _arraySize))
            {
                ConsoleHelper.WriteLineError("Invalid input argument.");
                return;
            }

            // генерируем коллекцию чисел
            IEnumerable<long> numbers = new long().Range(_arraySize);

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            ConsoleHelper.WriteLine($"Размер массива: [{_arraySize}]\r\nМетод вычеления: [обычный]");
            long result = numbers.Sum();
            stopwatch.Stop();
            ConsoleHelper.WriteLine($"Время выполнения: [{stopwatch.ElapsedMilliseconds}]\r\nРезультат = [{result}]\r\n");

            Console.ReadKey();
        }
    }
}