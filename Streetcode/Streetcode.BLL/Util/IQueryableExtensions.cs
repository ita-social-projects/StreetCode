namespace Streetcode.BLL.Util
{
    internal static class IQueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int page, int pageSize)
            => query.Skip((page - 1) * pageSize).Take(pageSize);
    }
}
