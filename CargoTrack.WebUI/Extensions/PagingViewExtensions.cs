using CargoTrack.DTO.DTOs.Common;
using CargoTrack.WebUI.Models;

namespace CargoTrack.WebUI.Extensions
{
    public static class PagingViewExtensions
    {
        public static PaginationViewModel ToPagination<T>(this PagedResultDto<T> result) => new()
        {
            Page = result.Page,
            TotalPages = result.TotalPages,
            TotalCount = result.TotalCount
        };
    }
}
