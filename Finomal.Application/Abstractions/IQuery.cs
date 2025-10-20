using Finomal.Domain.Abstractions;
using MediatR;

namespace Finomal.Application.Abstractions
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {
    }
}