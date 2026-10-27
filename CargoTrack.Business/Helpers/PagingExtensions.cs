using CargoTrack.DTO.DTOs.Common;
using Microsoft.EntityFrameworkCore;

namespace CargoTrack.Business.Helpers
{
    public static class PagingExtensions
    {
        private const int DefaultPageSize = 10;
        private const int MaxPageSize = 100;

        public static async Task<PagedResultDto<T>> ToPagedResultAsync<T>(this IQueryable<T> query, int page, int pageSize)
        {
            page = Math.Max(page, 1);
            pageSize = pageSize is < 1 or > MaxPageSize ? DefaultPageSize : pageSize;

            var totalCount = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResultDto<T>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}