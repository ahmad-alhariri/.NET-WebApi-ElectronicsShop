using ElectronicsShop.Domain.Common.Results;

namespace ElectronicsShop.Application.Interfaces.Services;

public interface ICartMigrationService
{
    Task<Result<Success>> MigrateAnonymousCartToUserAsync(Guid anonymousId, Guid userId ,CancellationToken cancellationToken);
}