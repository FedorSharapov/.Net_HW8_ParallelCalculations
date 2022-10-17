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


            ConsoleHelper.WriteLine($"Размер массива: [{_arraySize}]\r\nМетод вычеления: [параллельный с помощью Thread]");
            stopwatch.Restart();
            var chunkNums = numbers.Chunk(_arraySize/_numThreads).ToList();
            var countdownEvent = new CountdownEvent(chunkNums.Count);
            result = 0;

            object _locker = new object();

            foreach (var nums in chunkNums)
            {
                var action = new Action(() =>
                {
                    var res = nums.Sum();
                    lock(_locker)
                    {
                        result += res;
                    }
                    countdownEvent.Signal();
                });

                new Thread(start => action()).Start();
            }

            countdownEvent.Wait();
            stopwatch.Stop();
            countdownEvent.Dispose();
            ConsoleHelper.WriteLine($"Время выполнения: [{stopwatch.ElapsedMilliseconds}]\r\nРезультат = [{result}]\r\n");

            Console.ReadKey();
        }
    }
}