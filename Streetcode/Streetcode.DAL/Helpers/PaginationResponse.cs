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

        public static PaginationResponse<T> Create(IQueryable<T> source, ushort? pageNumber = null, ushort? pageSize = null)
        {
            ushort count = (ushort)(source?.Count() ?? 0);

            if (pageNumber is null && pageSize is null)
            {
                return new PaginationResponse<T>(source?.AsEnumerable() ?? Enumerable.Empty<T>(), count, 1, count);
            }

            if (pageNumber == 0)
            {
                return new PaginationResponse<T>(Enumerable.Empty<T>(), count, 0, 0);
            }

            var items = source?
                .Skip((pageNumber!.Value - 1) * pageSize!.Value)
                .Take(pageSize!.Value)
                .AsEnumerable() ?? Enumerable.Empty<T>();

            return new PaginationResponse<T>(items, count, pageNumber!.Value, pageSize!.Value);
        }
    }
}
