using System.ComponentModel;
using DMSPortal.Models.Enums;

namespace DMSPortal.Models.Models;

public class PaginationFilter
{
    private int _page = 1;

    [DefaultValue(1)]
    public int page
    {
        get => _page;
        set => _page = value < 1 ? 1 : value;
    }


    private int _size = 10;

    [DefaultValue(10)]
    public int size
    {
        get => _size;
        set => _size = value < 1 ? 1 : value;
    }

    private bool _takeAll = true;

    [DefaultValue(true)]
    public bool takeAll
    {
        get => _takeAll;
        set => _takeAll = value;
    }

    private EPageOrder _order = EPageOrder.ASC;

    public EPageOrder order
    {
        get => _order;
        set => _order = value;
    }

    public string? _search = null;

    [DefaultValue(null)]
    public string? search
    {
        get => _search;
        set => _search = value;
    }
}