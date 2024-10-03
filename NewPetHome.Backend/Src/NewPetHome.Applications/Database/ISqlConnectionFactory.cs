using System.Data;

namespace NewPetHome.Applications.Database;

public interface ISqlConnectionFactory
{
    IDbConnection Create();
}