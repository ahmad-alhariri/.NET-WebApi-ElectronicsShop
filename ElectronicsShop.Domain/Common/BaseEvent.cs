using MediatR;

namespace ElectronicsShop.Domain.Common;

public abstract class BaseEvent : INotification
{
    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}