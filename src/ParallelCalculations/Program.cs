using System.Diagnostics;

namespace ParallelCalculations
{
    public class Program
    {
        /// <summary>
        /// Сравнение параллельного вычесления суммы с обычным последоватеьным
        /// </summary>
        static void Main()
        {
            var arraySizes = new int[] { 100000, 1000000, 10000000};
                        
            foreach (var size in arraySizes)
            {
                var array = GenerateArray(size);

                Test(array.MySum, size, "обычный");
                Test(array.MyParallelSum, size, "параллельный (Thread)");
                Test(array.AsParallel().Sum, size, "параллельный (LINQ)");
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Генерация массива случайных чисел
        /// </summary>
        /// <param name="size">разер массива</param>
        /// <returns>массив случайных чисел</returns>
        public static int[] GenerateArray(int size)
        {
            var array = new int[size];
            var range = int.MaxValue / size;

            Random random = new Random();
            for (int i = 0; i < size; i++)
                array[i] = random.Next(-range, range);
           
            return array;
        }

        /// <summary>
        /// Тестирование быстродействия выполнения функции
        /// </summary>
        /// <param name="func">функция для теста</param>
        private static void Test(Func<int> func, int arraySize, string name)
        {
            long sum = 0;
            var stopwatch = new Stopwatch();

            ConsoleHelper.WriteLine($"Размер массива: [{arraySize}]");
            ConsoleHelper.WriteLine($"Метод выполнения: [{name}]");
            for (int j = 1; j < 11; j++)
            {
                stopwatch.Restart();

                var result = func();

                stopwatch.Stop();
                sum += stopwatch.ElapsedMilliseconds;
                ConsoleHelper.WriteLine($"{j}.Результат: [{result}]. Время выполнения: [{stopwatch.ElapsedMilliseconds}]");
            }

            double meanVal = ((sum / 10.0));
            ConsoleHelper.WriteLine($"Среднее время выполнения: [{meanVal:F1}] мс\r\n",ConsoleColor.Green);
        }
    }
}