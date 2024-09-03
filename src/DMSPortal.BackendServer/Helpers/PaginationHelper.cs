using System.ComponentModel;
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
                .Where(x =>
                    ((string)TypeDescriptor
                        .GetProperties(typeof(T))
                        .Find(filter.searchBy, true)?
                        .GetValue(x))
                    .Contains(filter.searchValue, StringComparison.CurrentCultureIgnoreCase))
                .ToList();
        }

        if (!string.IsNullOrEmpty(filter.orderBy))
        {
            items = filter.order switch
            {
                EPageOrder.ASC => items
                    .OrderBy(x =>
                        TypeDescriptor
                            .GetProperties(typeof(T))
                            .Find(filter.orderBy, true)?
                            .GetValue(x))
                    .ToList(),
                EPageOrder.DESC => items
                    .OrderByDescending(x =>
                        TypeDescriptor
                            .GetProperties(typeof(T))
                            .Find(filter.orderBy, true)?
                            .GetValue(x))
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