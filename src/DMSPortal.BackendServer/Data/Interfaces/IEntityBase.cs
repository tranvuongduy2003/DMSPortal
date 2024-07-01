namespace DMSPortal.BackendServer.Data.Interfaces;

public interface IEntityBase<TKey>
{
    TKey Id { get; set; }
}