using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace VideoGames.BLL.Dtos.Pagination
{
    public class PaginationResponseDto<T>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int Total { get; set; } = 0;
        public int PageCount { get; set; } = 1;
        public List<T> Items { get; set; } = [];
    }
}