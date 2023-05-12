using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RealStateManagementSystem.Domain.Dtos.AppUser;
using RealStateManagementSystem.Domain.Entities;
using RealStateManagementSystem.Domain.HashingMethods;
using RealStateManagementSystem.Domain.IServices;
using RealStateManagementSystem.Infastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateManagementSystem.Infastructure.Services
{
    public class AppUserService : BaseService<AppUser, ApplicationDbContext>, IAppUserService
    {
        private readonly ApplicationDbContext _db;
        private readonly AppUser _appUser;
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly ILogService _logService;

        public AppUserService(ApplicationDbContext db, AppUser appUser, IHttpContextAccessor httpAccessor, ILogService logService) : base(db)
        {
            _db = db;
            _appUser = appUser;
            _httpAccessor = httpAccessor;
            _logService = logService;
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _db.Roles.Where(x => x.IsActive).ToListAsync();
        }

        public async Task<Role> GetRoleAsyncForAppUser(int id)
        {
            var role = await _db.Roles.FindAsync(id);
            return role!;
        }

        public async Task<AppUser> GetAppUserForProfileAsync(int id)
        {
            var appUser = await _db.AppUsers.FindAsync(id);
            return appUser!;
        }

        public async Task<AppUser> LoginForAppUserAsync(LoginForAppUserDto loginForUserDto)
        {
            var loggedInUser = await _db.AppUsers.Where(x => x.IsActive!)
            .FirstOrDefaultAsync(x => x.Email == loginForUserDto.Email);
            return loggedInUser!;
        }

        public async Task<bool> PasswordChangeForAppUserAsync(PasswordChangeForAppUserDto passwordChangeForUserDto)
        {
            byte[] passwordHash, passwordSalt;
            var appUser = _db.AppUsers.Find(passwordChangeForUserDto.Id)!;
            if (passwordChangeForUserDto.NewPassword == passwordChangeForUserDto.RepeatNewPassword)
            {
                Hashing.CreatePasswordHash(passwordChangeForUserDto.NewPassword, out passwordHash, out passwordSalt);
                appUser.PasswordHash = passwordHash;
                appUser.PasswordSalt = passwordSalt;
                appUser.Password = passwordChangeForUserDto.NewPassword;
                appUser.UpdateDate = passwordChangeForUserDto.UpdateDate;
                await UpdateAsync(_appUser);
                return true;
            }
            return false;
        }

        public async Task<bool> PasswordControlForAppUserAsync(int id, string password)
        {
            password = Hashing.ControlPassword(password);
            var successNo = await _db.AppUsers
            .Where(x => x.Id == id && x.Password == password && x.IsActive).CountAsync();
            return (successNo > 0) ? true : false;
        }

        public async Task<bool> RegisterForAppUserAsync(CreateForAppUserDto createForAppUserDto)
        {
            byte[] passwordHash, passwordSalt;
            Hashing.CreatePasswordHash(createForAppUserDto.Password, out passwordHash, out passwordSalt);
            _appUser.Name = createForAppUserDto.Name;
            _appUser.Surname = createForAppUserDto.Surname;
            _appUser.Email = createForAppUserDto.Email;
            _appUser.Address = createForAppUserDto.Address;
            _appUser.IsActive = createForAppUserDto.IsActive;
            _appUser.Password = createForAppUserDto.Password;
            _appUser.RoleId = createForAppUserDto.RoleId;
            _appUser.CreateDate = createForAppUserDto.CreateDate;
            _appUser.PasswordHash = passwordHash;
            _appUser.PasswordSalt = passwordSalt;
            await AddAppUser(_appUser);
            return true;
        }

        public async Task<bool> UpdateForAppUserAsync(int id, UpdateForAppUserDto updateForAppUserDto)
        {
            byte[] passwordHash, passwordSalt;
            Hashing.CreatePasswordHash(updateForAppUserDto.Password, out passwordHash, out passwordSalt);
            var appUser = _db.AppUsers.Find(id)!;
            appUser.Name = updateForAppUserDto.Name;
            appUser.Surname = updateForAppUserDto.Surname;
            if (await ExistsAppUserWithEmailAsync(updateForAppUserDto.Email, appUser.Email))
            {
                return false;
            }
            appUser.Email = updateForAppUserDto.Email;
            appUser.Address = updateForAppUserDto.Address;
            appUser.IsActive = updateForAppUserDto.IsActive;
            appUser.Password = updateForAppUserDto.Password;
            appUser.RoleId = updateForAppUserDto.RoleId;
            appUser.UpdateDate = updateForAppUserDto.UpdateDate;
            appUser.PasswordHash = passwordHash;
            appUser.PasswordSalt = passwordSalt;
            await UpdateAsync(appUser);
            return true;
        }

        public async Task<AppUser> ValidateForAppUserAsync(AppUser appUser)
        {
            var loggedInAppUser = await _db.AppUsers.FirstOrDefaultAsync(x => x.Email == appUser.Email);
            return loggedInAppUser!;
        }

        public async Task<bool> ExistsAppUserWithEmailAsync(string email, string? mail)
        {
            if (mail == null)
            {
                return await _db.AppUsers.AnyAsync(x => x.Email == email) ? true : false;
            }
            else
            {
                return await _db.AppUsers.Where(x => x.Email != mail).AnyAsync(x => x.Email == email) ? true : false;
            }
        }

        public async Task<int> GetAppUserIdAsync(string email)
        {
            var appUser = await _db.AppUsers.FirstOrDefaultAsync(x => x.Email == email);
            return appUser!.Id;
        }

        public async Task<bool> AddAppUser(AppUser appUser)
        {
            await _db.AppUsers.AddAsync(appUser);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RegisterToken(Token token)
        {
            await _db.Tokens.AddAsync(token);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<string> GetLastToken()
        {
            var getLastToken = await _db.Tokens.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            return getLastToken!.AccesToken!;
        }

        public async Task<bool> DeleteAppUserAsync(int id)
        {
            var appUser = await _db.AppUsers.FindAsync(id);
            appUser!.IsActive = false;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTokenAsync()
        {
            var token = await GetLastToken();
            var lastToken = await _db.Tokens.FirstOrDefaultAsync(x => x.AccesToken == token);
            if (lastToken != null)
            {
                _db.Tokens.Remove(lastToken);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
