
namespace CactusDepot.Shared.Models.Seeds
{
    public static class PageHelper
    {
        public static PagedResult<T> GetPaged<T>(this IQueryable<T> query, int page, int pageSize) where T : CactusSeed
        {
            PagedResult<T> result = new()
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = query.Count()
            };


            double pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            int skip = (page - 1) * pageSize;
            result.Results = (IList<CactusSeed>)query.Skip(skip).Take(pageSize).ToList();

            return result;
        }
    }
}
