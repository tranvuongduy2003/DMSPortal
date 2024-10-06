namespace DMSPortal.BackendServer.Abstractions.Entity;

public interface IIdentityEntityBase<TKey> : IEntityBase
{
    TKey Id { get; set; }
}