using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.BackendServer.Infrastructure.Interfaces;
using DMSPortal.Models.DTOs;

namespace DMSPortal.BackendServer.Repositories.Contracts;

public interface IPermissionsRepository : IRepositoryBase<Permission>
{
}