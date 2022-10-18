namespace ParallelCalculations
{
    public static class MyExtensions
    {
        /// <summary>
        /// Вычесление суммы элементов массива
        /// </summary>
        /// <param name="array">массив чисел</param>
        /// <returns>сумма чисел</returns>
        public static int MySum(this int[] array)
        {
            int result = 0;
            for (int i = 0; i < array.Length; i++)
                result += array[i];

            return result;
        }

        /// <summary>
        /// Параллельное вычесление суммы элементов массива с использованием Thread
        /// </summary>
        /// <param name="array">массив чисел</param>
        /// <returns>сумма чисел</returns>
        public static int MyParallelSum(this int[] array)
        {
            var arrayChunks = array.Chunk(array.Length / Environment.ProcessorCount).ToArray();
            var count = arrayChunks.Count();
            var results = new int[count];
            var countdownEvent = new CountdownEvent(count);

            for (int i = 0; i < count; i++)
            {
                var j = i;
                var action = new Action(() =>
                {
                    results[j] = arrayChunks[j].MySum();
                    countdownEvent.Signal();
                });

                new Thread(start => action()).Start();
            }

            countdownEvent.Wait();
            countdownEvent.Dispose();

            return results.MySum();
        }
    }
}
