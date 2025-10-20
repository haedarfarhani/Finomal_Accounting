using Finomal.Application.Users;

namespace Finomal.Infrastructure.Data.Users
{
    public class UserService : IUserService
    {

        public UserService()
        {
 
        }

        public async Task AddRoleToUserAsync(string userId, string roleName)
        {
           
        }

        public async Task<bool> CurrentUserCanCreateArticleAsync()
        {
           return true;
        }

        public async Task<bool> CurrentUserCanEditArticleAsync(int articleId)
        {

            return true;
        }

        public async Task<string> GetCurrentUserIdAsync()
        {
            //var user = await GetCurrentUserAsync();

            return "user.Id";
        }

        public async Task<List<string>> GetUserRolesAsync(string userId)
        {

            return new List<string>();
        }

        public async Task<bool> IsCurrentUserInRoleAsync(string role)
        {
            //var user = await GetCurrentUserAsync();
            //var result = user is not null && await _userManager.IsInRoleAsync(user, role);
            return true;
        }

        public async Task RemoveRoleFromUserAsync(string userId, string roleName)
        {

        }

        //private async Task<User?> GetCurrentUserAsync()
        //{
        //    //var httpContext = _httpContextAccessor.HttpContext;
        //    //if (httpContext is null || httpContext.User is null)
        //    //{
        //    //    return null;
        //    //}
        //    return new User();
        //}
    }
}
