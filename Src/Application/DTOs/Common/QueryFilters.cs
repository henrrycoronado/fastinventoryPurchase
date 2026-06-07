namespace PrismodPurchase.Src.Application.DTOs.Common;

public class PurchaseOrderQueryFilters
{
    public short? Status { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public bool SortDescending { get; set; } = true;
}

public class PagedResultDto<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
}
