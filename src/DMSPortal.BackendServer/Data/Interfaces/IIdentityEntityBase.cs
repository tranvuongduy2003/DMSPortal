namespace DMSPortal.BackendServer.Data.Interfaces;

public interface IIdentityEntityBase<TKey> : IEntityBase
{
    TKey Id { get; set; }
}