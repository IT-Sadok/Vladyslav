namespace Domain.Extensions;

public static class ListExtensions
{
    public static IEnumerable<List<T>> Batch<T>(this List<T> source, int size)
    {
        for (var i = 0; i < source.Count; i += size)
        {
            yield return source.GetRange(i, Math.Min(size, source.Count - i));
        }
    }
}
