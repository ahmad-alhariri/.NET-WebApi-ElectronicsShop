using ElectronicsShop.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronicsShop.Application.Common.Extensions;

public static class IQueryableExtensions
{
    /// <summary>
    /// Creates a paginated list from an IQueryable source.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source.</typeparam>
    /// <param name="source">The IQueryable source to paginate.</param>
    /// <param name="pageNumber">The current page number (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A PagedList containing the items for the specified page and pagination metadata.</returns>
    public static async Task<PaginatedList<T>> ToPagedListAsync<T>(
        this IQueryable<T> source,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        // Ensure page number and size are valid
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 10 : pageSize;

        // The CountAsync call is executed as a separate, efficient query
        var totalCount = await source.CountAsync(cancellationToken);

        // The Skip and Take operations are translated directly into SQL OFFSET and FETCH clauses
        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<T>(items, totalCount, pageNumber, pageSize);
    }
}