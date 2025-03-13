namespace Streetcode.BLL.Util.Extensions
{
    internal static class IEnumerableExtensions
    {
        public static IEnumerable<T> Paginate<T>(this IEnumerable<T> query, int page, int pageSize)
            => query.Skip((page - 1) * pageSize).Take(pageSize);
    }
}
