using Finomal.Domain.Abstractions;
using MediatR;

namespace Finomal.Application.Abstractions
{
    public interface ICommand : IRequest<Result>
    {
    }
    public interface ICommand<TResponse> : IRequest<Result<TResponse>>
    {
    }
}
