namespace DMSPortal.Models.Common;

public class Metadata
{
    public int CurrentPage { get; private set; }

    public int TotalPages { get; private set; }

    public bool TakeAll { get; private set; }

    public int PageSize { get; private set; }

    public int PayloadSize { get; private set; }
    
    public int TotalCount { get; private set; }

    public bool HasPrevious => CurrentPage > 1;

    public bool HasNext => CurrentPage < TotalPages;

    public Metadata()
    {
        TotalCount = 0;
        PayloadSize = 0;
        PageSize = 10;
        CurrentPage = 1;
        TotalPages = 0;
        TakeAll = true;
    }

    public Metadata(int totalItems, int pageNumber, int pageSize, bool takeAll)
    {
        TotalCount = totalItems;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(TotalCount / (double)pageSize);
        TakeAll = takeAll;
        
        // Calculate PayloadSize mathematically
        if (takeAll)
        {
            PayloadSize = TotalCount;
        }
        else
        {
            int remainingItems = TotalCount - (pageNumber - 1) * pageSize;
            PayloadSize = Math.Min(pageSize, Math.Max(0, remainingItems));
        }
    }
}