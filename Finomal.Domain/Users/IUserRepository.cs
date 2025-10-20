namespace Finomal.Domain.Users
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(string userId);
        Task<List<User>> GetUsersByIdsAsync(IEnumerable<string> userIds);
        Task<List<User>> GetAllUsersAsync();
    }
}