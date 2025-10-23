using Finomal.Application.Abstractions;
using Finomal.Application.Users.Dtos;
using Finomal.Domain.Abstractions;
using Finomal.Domain.Users;
using Mapster;

namespace Finomal.Application.Users.GetUsers
{
    public class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, List<UserDto>>
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        public GetUsersQueryHandler(IUserService userService, IUserRepository userRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
        }
        public Task<Result<List<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            
            //var users = await _userRepository.GetAllUsersAsync();
            var response = new List<UserDto>();
            response.Add(new UserDto());
            return Task.FromResult<Result<List<UserDto>>>(response);
        }
    }

}
