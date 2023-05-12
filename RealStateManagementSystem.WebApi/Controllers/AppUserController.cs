using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RealStateManagementSystem.Domain.Dtos.AppUser;
using RealStateManagementSystem.Domain.Entities;
using RealStateManagementSystem.Domain.HashingMethods;
using RealStateManagementSystem.Domain.IServices;
using RealStateManagementSystem.Domain.RequestParameters;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealStateManagementSystem.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        private readonly IAppUserService _appUserService;
        private readonly ILogService _logService;
        private readonly IConfiguration _configuration;
        private readonly Log _log;
        private readonly Token _token;

        public AppUserController(IAppUserService appUserService, ILogService logService, IConfiguration configuration, Log log, Token token)
        {
            _appUserService = appUserService;
            _logService = logService;
            _configuration = configuration;
            _log = log;
            _token = token;
        }

        [HttpPost]
        public async Task<ActionResult<AppUser>> Login([FromBody] LoginForAppUserDto loginForAppUserDto)
        {
            var appUser = await _appUserService.LoginForAppUserAsync(loginForAppUserDto);
            try
            {
                if (appUser != null)
                {
                    if (!Hashing.VerifyPassword(loginForAppUserDto.Password, appUser.PasswordHash, appUser.PasswordSalt))
                    {
                        _log.State = "Başarısız";
                        _log.AppUserId = appUser.Id;
                        _log.Description = "Kullanıcı Adı veya Parola hatalı girildi.";
                        _log.ProcessType = "Kullanıcı Giriş";
                        _log.CreateDate = appUser.CreateDate;
                        _log.IsActive = true;
                        _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? HttpContext.Connection.RemoteIpAddress?.ToString()!;
                        await _logService.AddAsync(_log);
                        return BadRequest(new { _log.Description });
                    }
                    _log.State = "Başarılı";
                    _log.AppUserId = appUser.Id;
                    _log.Description = "Kullanıcı Giriş Yaptı.";
                    _log.ProcessType = "Kullanıcı Giriş";
                    _log.CreateDate = appUser.CreateDate;
                    _log.IsActive = true;
                    _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? HttpContext.Connection.RemoteIpAddress?.ToString()!;
                    await _logService.AddAsync(_log);
                    var token = GetToken(appUser);
                    _token.AccesToken = token;
                    _token.IsActive = true;
                    await _appUserService.RegisterToken(_token);
                    return Ok(new { token });
                }
                else
                {
                    _log.State = "Başarısız";
                    _log.Description = "Kullanıcı Adı veya Parola hatalı girildi.";
                    _log.ProcessType = "Kullanıcı Giriş";
                    _log.CreateDate = DateTime.Now;
                    _log.IsActive = true;
                    _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? HttpContext.Connection.RemoteIpAddress?.ToString()!;
                    await _logService.AddAsync(_log);
                    return BadRequest(new { _log.Description });
                }
            }
            catch (Exception)
            {
                _log.State = "Başarısız";
                _log.Description = "Sistemde beklenmedik bir hata oluştu.";
                _log.ProcessType = "Kullanıcı Giriş";
                _log.CreateDate = DateTime.Now;
                _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                 HttpContext.Connection.RemoteIpAddress?.ToString()!;
                await _logService.AddAsync(_log);
                return BadRequest(new { _log.Description });
            }
        }

        private string GetToken(AppUser appUser)
        {
            try
            {
                var claims = new[]
                {
                   new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
                   new Claim(JwtRegisteredClaimNames.Email, appUser.Email)
                };
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:SigningKey"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: _configuration["JwtConfig:Issuer"],
                    audience: _configuration["JwtConfig:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(100),
                    notBefore: DateTime.UtcNow,
                    signingCredentials: credentials);
                var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                return token;
            }
            catch (Exception)
            {
                return null!;
            }
        }

        [HttpGet]
        private string ValidateToken(string token)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:SigningKey"]));
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                handler.ValidateToken(token, new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuer = false
                },
                out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                var claims = jwtToken.Claims.ToList();
                var activeAppUserId = claims.FirstOrDefault()!.ToString();
                return activeAppUserId;
            }
            catch (Exception)
            {
                return null!;
            }
        }

        private async Task<AppUser> GetActiveAppUser()
        {
            var activeAppUserId = ValidateToken(await _appUserService.GetLastToken()).Split(":")[2].Trim();
            var activeAppUser = await _appUserService.GetAppUserForProfileAsync(Convert.ToInt32(activeAppUserId));
            return activeAppUser;
        }

        [HttpPost]
        public async Task<ActionResult> Register([FromBody] CreateForAppUserDto createForAppUserDto)
        {
            var activeAppUser = await GetActiveAppUser();
            try
            {
                if (!ModelState.IsValid)
                {
                    _log.AppUserId = activeAppUser.Id;
                    _log.State = "Başarısız";
                    _log.Description = "Kullanıcı kayıt işlemi yapılırken beklenmedik bir hata oluştu.";
                    _log.ProcessType = "Kullanıcı Kayıt";
                    _log.CreateDate = DateTime.Now;
                    _log.IsActive = true;
                    _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                        HttpContext.Connection.RemoteIpAddress?.ToString()!;
                    await _logService.AddAsync(_log);
                    return BadRequest(new { _log.Description });
                }
                if (await _appUserService.ExistsAppUserWithEmailAsync(createForAppUserDto.Email, null))
                {
                    _log.AppUserId = activeAppUser.Id;
                    _log.State = "Başarısız";
                    _log.Description = "Sisteme daha önce kayıt yapılmış bir email adresi bulundu.";
                    _log.ProcessType = "Kullanıcı Kayıt";
                    _log.CreateDate = DateTime.Now;
                    _log.IsActive = true;
                    _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                        HttpContext.Connection.RemoteIpAddress?.ToString()!;
                    await _logService.AddAsync(_log);
                    return BadRequest(new { _log.Description });
                }
                await _appUserService.RegisterForAppUserAsync(createForAppUserDto);
                _log.AppUserId = activeAppUser.Id;
                _log.State = "Başarılı";
                _log.Description = "Kullanıcı ekleme başarılı bir şekilde gerçekleşti.";
                _log.ProcessType = "Kullanıcı Kayıt";
                _log.CreateDate = DateTime.Now;
                _log.IsActive = true;
                _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                    HttpContext.Connection.RemoteIpAddress?.ToString()!;
                await _logService.AddAsync(_log);
                return Ok(new { _log.Description });
            }
            catch (Exception)
            {
                _log.AppUserId = activeAppUser.Id;
                _log.State = "Başarısız";
                _log.Description = "Kullanıcı kayıt işlemi yapılırken beklenmedik bir hata oluştu.";
                _log.ProcessType = "Kullanıcı Kayıt";
                _log.CreateDate = DateTime.Now;
                _log.IsActive = true;
                _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                    HttpContext.Connection.RemoteIpAddress?.ToString()!;
                await _logService.AddAsync(_log);
                return BadRequest(new { _log.Description });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetAllUsers([FromQuery] Pagination pagination)
        {
            var activeAppUser = await GetActiveAppUser();
            var getAll = _appUserService.GetAllAsync().Result.Where(x => x.IsActive).ToList();
            try
            {
                if (getAll.Count == 0)
                {
                    _log.AppUserId = activeAppUser.Id;
                    _log.State = "Başarısız";
                    _log.Description = "Sistemde kayıtlı aktif kullanıcılar bulunamadı.";
                    _log.ProcessType = "Kullanıcı Listeleme";
                    _log.CreateDate = DateTime.Now;
                    _log.IsActive = true;
                    _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                        HttpContext.Connection.RemoteIpAddress?.ToString()!;
                    await _logService.AddAsync(_log);
                    return NotFound(new { _log.Description });
                }
                _log.AppUserId = activeAppUser.Id;
                _log.State = "Başarılı";
                _log.Description = "Sistemde kayıtlı aktif kullanıcılar listelendi.";
                _log.ProcessType = "Kullanıcı Listeleme";
                _log.CreateDate = DateTime.Now;
                _log.IsActive = true;
                _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                    HttpContext.Connection.RemoteIpAddress?.ToString()!;
                await _logService.AddAsync(_log);
                var totalCount = getAll.Where(x => x.Id != activeAppUser.Id).Count();
                var listAppUsers = getAll.Where(x => x.Id != activeAppUser.Id).Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Surname,
                    x.Email,
                    x.Address,
                    Role = _appUserService.GetRoleAsyncForAppUser(x.RoleId).Result.Name
                }).Skip(pagination.Page * pagination.Size).Take(pagination.Size).OrderByDescending(x => x.Id);
                return Ok(new
                {
                    totalCount,
                    listAppUsers
                });
            }
            catch (Exception)
            {
                _log.AppUserId = activeAppUser.Id;
                _log.State = "Başarısız";
                _log.Description = "Sistemde beklenmeyen bir hata oluştu.";
                _log.ProcessType = "Kullanıcı Listeleme";
                _log.CreateDate = DateTime.Now;
                _log.IsActive = true;
                _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                    HttpContext.Connection.RemoteIpAddress?.ToString()!;
                await _logService.AddAsync(_log);
                return BadRequest(new { _log.Description });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetAppUser(int id)
        {
            var activeAppUser = await GetActiveAppUser();
            var user = await _appUserService.GetByIdAsync(id);
            try
            {
                if (user == null)
                {
                    _log.AppUserId = activeAppUser.Id;
                    _log.State = "Başarısız";
                    _log.Description = "Girilen bilgilerde kullanıcı yoktur.";
                    _log.ProcessType = "Kullanıcı Listeleme";
                    _log.CreateDate = DateTime.Now;
                    _log.IsActive = true;
                    _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                        HttpContext.Connection.RemoteIpAddress?.ToString()!;
                    await _logService.AddAsync(_log);
                    return NotFound(new { _log.Description });
                }
                _log.AppUserId = activeAppUser.Id;
                _log.State = "Başarılı";
                _log.Description = "Sistemde seçilen kullanıcı getirildi.";
                _log.ProcessType = "Kullanıcı Getirme";
                _log.CreateDate = DateTime.Now;
                _log.IsActive = true;
                _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                    HttpContext.Connection.RemoteIpAddress?.ToString()!;
                await _logService.AddAsync(_log);
                return Ok(user);
            }
            catch (Exception)
            {
                _log.AppUserId = activeAppUser.Id;
                _log.AppUserId = user.Id;
                _log.State = "Başarısız";
                _log.Description = "Sistemde beklenmeyen bir hata oluştu.";
                _log.ProcessType = "Kullanıcı Getirme";
                _log.CreateDate = DateTime.Now;
                _log.IsActive = true;
                _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                    HttpContext.Connection.RemoteIpAddress?.ToString()!;
                await _logService.AddAsync(_log);
                return BadRequest(_log.Description);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateForAppUserDto updateForAppUserDto)
        {
            var activeAppUser = await GetActiveAppUser();
            var existingAppUser = await _appUserService.GetByIdAsync(id);
            try
            {
                if (existingAppUser == null)
                {
                    _log.AppUserId = activeAppUser.Id;
                    _log.State = "Başarısız";
                    _log.Description = "Kullanıcı sistemde bulunamadı.";
                    _log.ProcessType = "Kullanıcı Güncelleme";
                    _log.UpdateDate = DateTime.Now;
                    _log.IsActive = true;
                    _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                        HttpContext.Connection.RemoteIpAddress?.ToString()!;
                    await _logService.AddAsync(_log);
                    return NotFound(new { _log.Description });
                }
                if (!ModelState.IsValid)
                {
                    _log.AppUserId = activeAppUser.Id;
                    _log.State = "Başarısız";
                    _log.Description = "Kullanıcı güncelleme başarısız olmuştur.";
                    _log.ProcessType = "Kullanıcı Güncelleme";
                    _log.UpdateDate = DateTime.Now;
                    _log.IsActive = true;
                    _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                        HttpContext.Connection.RemoteIpAddress?.ToString()!;
                    await _logService.AddAsync(_log);
                    return BadRequest(new { _log.Description });
                }
                if (await _appUserService.UpdateForAppUserAsync(id, updateForAppUserDto))
                {
                    _log.AppUserId = activeAppUser.Id;
                    _log.State = "Başarılı";
                    _log.Description = "Sistemdeki kullanıcı başarılı şekilde güncellenmiştir.";
                    _log.ProcessType = "Kullanıcı Getirme";
                    _log.UpdateDate = DateTime.Now;
                    _log.IsActive = true;
                    _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                        HttpContext.Connection.RemoteIpAddress?.ToString()!;
                    await _logService.AddAsync(_log);
                    return Ok(new { _log.Description });
                }
                _log.AppUserId = activeAppUser.Id;
                _log.State = "Başarısız";
                _log.Description = "Kullanıcı güncelleme başarısız oldu. Sistemde aynı mail adresi vardır.";
                _log.ProcessType = "Kullanıcı Güncelleme";
                _log.UpdateDate = DateTime.Now;
                _log.IsActive = true;
                _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                    HttpContext.Connection.RemoteIpAddress?.ToString()!;
                await _logService.AddAsync(_log);
                return BadRequest(new { _log.Description });
            }
            catch (Exception)
            {
                _log.AppUserId = activeAppUser.Id;
                _log.State = "Başarısız";
                _log.Description = "Kullanıcı güncelleme başarısız oldu.";
                _log.ProcessType = "Kullanıcı Güncelleme";
                _log.UpdateDate = DateTime.Now;
                _log.IsActive = true;
                _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                    HttpContext.Connection.RemoteIpAddress?.ToString()!;
                await _logService.AddAsync(_log);
                return BadRequest(new { _log.Description });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var activeAppUser = await GetActiveAppUser();
            var existingAppUser = await _appUserService.GetByIdAsync(id);
            try
            {
                if (existingAppUser != null)
                {
                    _log.AppUserId = activeAppUser.Id;
                    _log.State = "Başarılı";
                    _log.Description = "Sistemdeki kullanıcı başarılı şekilde silinmiştir.";
                    _log.ProcessType = "Kullanıcı Silme";
                    _log.CreateDate = DateTime.Now;
                    _log.IsActive = true;
                    _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                        HttpContext.Connection.RemoteIpAddress?.ToString()!;
                    await _logService.AddAsync(_log);
                    await _appUserService.DeleteAppUserAsync(existingAppUser.Id);
                    return Ok(new { _log.Description });
                }
                _log.AppUserId = activeAppUser.Id;
                _log.State = "Başarısız";
                _log.Description = "Kullanıcı silinmesi başarısız olundu.";
                _log.ProcessType = "Kullanıcı Silme";
                _log.CreateDate = DateTime.Now;
                _log.IsActive = true;
                _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                    HttpContext.Connection.RemoteIpAddress?.ToString()!;
                await _logService.AddAsync(_log);
                return BadRequest(new { _log.Description });
            }
            catch (Exception)
            {
                _log.AppUserId = activeAppUser.Id;
                _log.State = "Başarısız";
                _log.Description = "Kullanıcı silinmesi başarısız olundu.";
                _log.ProcessType = "Kullanıcı Silme";
                _log.CreateDate = DateTime.Now;
                _log.IsActive = true;
                _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                    HttpContext.Connection.RemoteIpAddress?.ToString()!;
                await _logService.AddAsync(_log);
                return BadRequest(new { _log.Description });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            var roleList = await _appUserService.GetAllRolesAsync();
            return Ok(roleList);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var message = "";
            if (await _appUserService.DeleteTokenAsync())
            {
                message = "Son token başarılı şekilde silinmiştir.";
                return Ok(new { message });
            }
            message = "Son token silinmesi başarısız olundu.";
            return BadRequest(new { message });
        }
    }
}
