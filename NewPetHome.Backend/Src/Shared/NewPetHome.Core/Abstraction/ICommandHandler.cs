using CSharpFunctionalExtensions;
using NewPetHome.SharedKernel;

namespace NewPetHome.Core.Abstraction;

public interface ICommandHandler<TResponse, in TCommand> where TCommand : ICommand
{
    Task<Result<TResponse, ErrorList>> Handle(
        TCommand command,
        CancellationToken cancellationToken = default);
}

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    Task<UnitResult<ErrorList>> Handle(
        TCommand command,
        CancellationToken cancellationToken = default);
}