using System.Data;

namespace NewPetHome.Core.Abstraction;

public interface ISqlConnectionFactory
{
    IDbConnection Create();
}