using DMSPortal.BackendServer.Models;
using DMSPortal.Models.Enums;

namespace DMSPortal.BackendServer.Helpers;

public static class PaginationHelper<T>
{
    public static Pagination<T> Paginate(PaginationFilter filter, List<T> items)
    {
        var metadata = new Metadata(items.Count, filter.page, filter.size, filter.takeAll);
        
        if (!string.IsNullOrEmpty(filter.searchValue) && !string.IsNullOrEmpty(filter.searchBy))
        {
            items = items
                .Where(x => x.GetType().GetProperty(filter.searchBy).ToString()
                    .Equals(filter.searchValue, StringComparison.CurrentCultureIgnoreCase))
                .ToList();
        }

        if (!string.IsNullOrEmpty(filter.orderBy))
        {
            items = filter.order switch
            {
                EPageOrder.ASC => items
                    .OrderBy(c => c.GetType().GetProperty(filter.orderBy))
                    .ToList(),
                EPageOrder.DESC => items
                    .OrderByDescending(c => c.GetType().GetProperty(filter.orderBy))
                    .ToList(),
                _ => items
            };
        }
        
        if (filter.takeAll == false)
        {
            items = items
                .Skip((filter.page - 1) * filter.size)
                .Take(filter.size)
                .ToList();
        }
        

        return new Pagination<T>
        {
            Items = items,
            Metadata = metadata
        };
    }
}