using PrismodPurchase.Src.Infraestructure.Persistence.Interfaces;

namespace PrismodPurchase.Src.Infraestructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    public UnitOfWork(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => await _dbContext.SaveChangesAsync(cancellationToken);
}
