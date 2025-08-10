namespace Persistence.Repositories.Queries;

public class BaseQueryRepository<TContext>
{
    internal readonly TContext _context;

    public BaseQueryRepository(TContext context)
    {
        _context = context;
    }
}