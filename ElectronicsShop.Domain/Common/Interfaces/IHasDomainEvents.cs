using System.ComponentModel.DataAnnotations.Schema;

namespace ElectronicsShop.Domain.Common.Interfaces;

public interface IHasDomainEvents
{
    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents { get; }
    public void ClearDomainEvents();
}