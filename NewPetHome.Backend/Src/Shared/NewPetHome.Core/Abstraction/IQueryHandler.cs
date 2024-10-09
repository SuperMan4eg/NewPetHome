using CSharpFunctionalExtensions;
using NewPetHome.SharedKernel;

namespace NewPetHome.Core.Abstraction;

public interface IQueryHandler<TResponse, in TQuery> where TQuery : IQuery
{
    Task<Result<TResponse, ErrorList>> Handle(
        TQuery query,
        CancellationToken cancellationToken = default);
}