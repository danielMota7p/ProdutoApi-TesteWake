using System.Collections.Generic;

namespace ProductApplication.DTOs
{
    public class PagedResult<T>
    {
        public IReadOnlyCollection<T> Items { get; init; } = new List<T>();
        public int Page { get; init; }
        public int PageSize { get; init; }
        public int TotalItems { get; init; }
        public int TotalPages => PageSize <= 0 ? 0 : (int)System.Math.Ceiling((double)TotalItems / PageSize);
    }
}
