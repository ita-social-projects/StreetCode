using Microsoft.EntityFrameworkCore;

namespace Streetcode.DAL.Helpers
{
    public class PaginationResponse<T>
    {
        private PaginationResponse(IEnumerable<T> items, ushort count, ushort pageNumber, ushort pageSize)
        {
            TotalItems = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (ushort)Math.Ceiling(count / (double)pageSize);
            Entities = items;
        }

        public ushort TotalItems { get; private set; }
        public ushort CurrentPage { get; private set; }
        public ushort TotalPages { get; private set; }
        public ushort PageSize { get; private set; }

        public IEnumerable<T> Entities { get; set; }

        public static async Task<PaginationResponse<T>> Create(IQueryable<T> source, ushort pageNumber, ushort pageSize)
        {
            ushort count = (ushort)(source?.Count() ?? 0);
            var items = await (source?
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize) ?? Enumerable.Empty<T>().AsQueryable())
                .ToListAsync();
            return new PaginationResponse<T>(items, count, pageNumber, pageSize);
        }
    }
}
