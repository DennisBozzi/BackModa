using Microsoft.EntityFrameworkCore;
using System.Linq;

public class PaginationHelper<T>
{
    public List<T> Data { get; set; }
    public int Count { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int PageTotal { get; set; }

    public void Formater()
    {
        Count = Data.Count();
        PageTotal = (int)Math.Ceiling(Count / (double)PageSize);
        Data = Data.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();
    }
}