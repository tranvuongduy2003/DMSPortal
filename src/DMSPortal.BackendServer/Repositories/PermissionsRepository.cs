using System.Data;
using Dapper;
using DMSPortal.BackendServer.Data;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.BackendServer.Infrastructure.RepositoryBase;
using DMSPortal.BackendServer.Repositories.Contracts;
using DMSPortal.Models.DTOs;
using Microsoft.Data.SqlClient;

namespace DMSPortal.BackendServer.Repositories;

public class PermissionsRepository : RepositoryBase<Permission>, IPermissionsRepository
{
	public PermissionsRepository(ApplicationDbContext dbContext) : base(
        dbContext)
	{
	}
}