namespace TaskFlow.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; }
    public Guid TenantId { get; protected set; }
    public bool IsDeleted { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid CreatedBy { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public Guid? UpdatedBy { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public Guid? DeletedBy { get; private set; }

    protected BaseEntity() { }

    protected BaseEntity(Guid tenantId, Guid createdBy)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsDeleted(Guid deletedBy)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
    }
}