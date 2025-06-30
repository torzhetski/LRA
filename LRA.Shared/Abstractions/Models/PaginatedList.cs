public class PaginatedList<T>
{

    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }

    public List<T> Items { get; set; } = new();

    public PaginatedList() { }

    public PaginatedList(IQueryable<T> source, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = source.Count();
        TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

        Items.AddRange(source.Skip(PageNumber * PageSize).Take(PageSize));
    }
}
