using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Library;


public static class DbSetExtension
{

    public static async Task<int> RemoveMatchingAsync<T>(this DbSet<T> dbSet, Expression<Func<T, bool>> predicate) where T : class
    {
        ArgumentNullException.ThrowIfNull(predicate);
        if (dbSet != null)
        {
            var elementsToRemove = await dbSet
                    .Where(predicate)
                    .ToListAsync();
            dbSet.RemoveRange(elementsToRemove);
            return elementsToRemove.Count;
        }
        return 0;
    }

}