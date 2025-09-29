using System.Threading;
using System.Threading.Tasks;
using ElectronicsShop.Application.Interfaces.Services;
using MediatR;

namespace ElectronicsShop.Application.Features.Authentication.Events
{
    public class UserSignedInEventHandler : INotificationHandler<UserSignedInEvent>
    {
        private readonly ICartMigrationService _cartMigrationService;

        public UserSignedInEventHandler(ICartMigrationService cartMigrationService)
        {
            _cartMigrationService = cartMigrationService;
        }

        public async Task Handle(UserSignedInEvent notification, CancellationToken cancellationToken)
        {
            await _cartMigrationService.MigrateAnonymousCartToUserAsync(notification.AnonymousId, notification.UserId, cancellationToken);
        }
    }
}

