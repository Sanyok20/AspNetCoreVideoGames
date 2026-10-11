using Microsoft.EntityFrameworkCore;
using VideoGames.BLL.Dtos.Pagination;
using VideoGames.DAL.Repositories;

namespace VideoGames.BLL.Tools
{
    public class PaginateCollection
    {
        public async Task<PaginateDto<T>> PaginateAsync<T>(IQueryable<T> items, PaginationRequestDto dto, CancellationToken ct = default)
        {
            int pageSize = dto.PageSize < 1 ? 20 : dto.PageSize;

            int total = await items.CountAsync(ct);
            int pages = (int)Math.Ceiling((double)total / dto.PageSize);

            int page = dto.Page < 1 || dto.Page > pages ? 1 : dto.Page;

            items = items
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return new PaginateDto<T>
            {
                Items = items,
                Page = page,
                PageCount = pages,
                Total = total
            };
        }
    }
}