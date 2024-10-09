using System.Data;
using Microsoft.Extensions.Configuration;
using NewPetHome.Core.Abstraction;
using Npgsql;

namespace NewPetHome.Volunteers.Infrastructure;

public class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly IConfiguration _configuration;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDbConnection Create() =>
        new NpgsqlConnection(_configuration.GetConnectionString("Database"));
}