using System.ComponentModel;
using DMSPortal.Models.Enums;

namespace DMSPortal.BackendServer.Models;

public class PaginationFilter
{
    private int _page = 1;

    private int _size = 10; 
    
    private bool _takeAll = true;
    
    private string _orderBy  = string.Empty;
    
    private EPageOrder _order = EPageOrder.ASC;
    
    private string _searchBy  = string.Empty;
    
    private string? _searchValue = string.Empty;
    
    [DefaultValue(1)]
    public int page
    {
        get => _page;
        set => _page = value < 1 ? 1 : value;
    }


    [DefaultValue(10)]
    public int size
    {
        get => _size;
        set => _size = value < 1 ? 1 : value;
    }

    [DefaultValue(true)]
    public bool takeAll
    {
        get => _takeAll;
        set => _takeAll = value;
    }
    
    public string? orderBy
    {
        get => _orderBy;
        set => _orderBy = value;
    }
    
    public EPageOrder order
    {
        get => _order;
        set => _order = value;
    }
    
    public string? searchBy
    {
        get => _searchBy;
        set => _searchBy = value;
    }
    
    [DefaultValue(null)]
    public string? searchValue
    {
        get => _searchValue;
        set => _searchValue = value;
    }
}