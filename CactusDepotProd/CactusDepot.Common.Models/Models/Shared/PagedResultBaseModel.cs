using System;
using System.Collections.Generic;
using CactusDepot.Shared.Models.Seeds;

namespace CactusDepot.Shared.Models
{
    public abstract class PagedResultBaseModel
    {
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }

        public int FirstRowOnPage => (CurrentPage - 1) * PageSize + 1;

        public int LastRowOnPage => Math.Min(CurrentPage * PageSize, RowCount);
    }

    public class PagedResult<T> : PagedResultBaseModel where T : CactusSeed
    {
        public System.Collections.Generic.IList<CactusSeed> Results { get; set; }

        public PagedResult()
        {
            Results = new List<CactusSeed>();
        }
    }

}



