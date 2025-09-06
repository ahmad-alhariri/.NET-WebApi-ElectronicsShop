using ElectronicsShop.Domain.Common.Interfaces;

namespace ElectronicsShop.Domain.Common;

public abstract class BaseAuditableEntity: BaseEntity, IAuditableEntity
{
    public int? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    
    public int? DeletedBy { get; set; }
    public DateTime? DeletedDate { get; set; }
}