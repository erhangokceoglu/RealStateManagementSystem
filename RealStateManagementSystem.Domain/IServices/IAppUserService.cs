using RealStateManagementSystem.Domain.Dtos.AppUser;
using RealStateManagementSystem.Domain.Entities;

namespace RealStateManagementSystem.Domain.IServices
{
    public interface IAppUserService : IBaseService<AppUser>
    {
        Task<AppUser> GetAppUserForProfileAsync(int id);
        Task<int> GetAppUserIdAsync(string email);
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role> GetRoleAsyncForAppUser(int id);
        Task<AppUser> LoginForAppUserAsync(LoginForAppUserDto loginForAppUserDto);
        Task<AppUser> ValidateForAppUserAsync(AppUser appUser);
        Task<bool> RegisterForAppUserAsync(CreateForAppUserDto createForAppUserDto);
        Task<bool> UpdateForAppUserAsync(int id, UpdateForAppUserDto updateForAppUserDto);
        Task<bool> ExistsAppUserWithEmailAsync(string email, string? mail);
        Task<bool> PasswordChangeForAppUserAsync(PasswordChangeForAppUserDto passwordChangeForAppUserDto);
        Task<bool> PasswordControlForAppUserAsync(int id, string password);
        Task<bool> AddAppUser(AppUser appUser);
        Task<bool> RegisterToken(Token token);
        Task<string> GetLastToken();
        Task<bool> DeleteAppUserAsync(int id);
        Task<bool> DeleteTokenAsync();
    }
}
