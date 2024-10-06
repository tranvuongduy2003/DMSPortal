using DMSPortal.BackendServer.Data.Entities;

namespace DMSPortal.BackendServer.Abstractions.Repository.Contracts;

public interface ICommandsRepository : IRepositoryBase<Command, string>
{
}