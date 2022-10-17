namespace ParallelCalculations
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<long> Range(this long source, long length)
        {
            for (long i = source; i < length; i++)
                yield return i;
        }
    }
}
