using MediatR;


namespace ElectronicsShop.Application.Features.Authentication.Events
{
    public class UserSignedInEvent : INotification
    {
        public Guid UserId { get; }
        public Guid AnonymousId { get; }

        public UserSignedInEvent(Guid userId, Guid anonymousId)
        {
            UserId = userId;
            AnonymousId = anonymousId;
        }
    }
}

