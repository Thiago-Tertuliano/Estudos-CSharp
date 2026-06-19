using TaskFlow.Domain.Enums.Tenant;
namespace TaskFlow.Domain.Entities;

public class Tenant 
{
    public Guid Id { get; protected set; }
    public string Name { get; protected set; } = string.Empty;
    public PlanType PlanType { get; private set; }
    public SubscriptionStatus SubscriptionStatus { get; private set; } = SubscriptionStatus.Active;
}
