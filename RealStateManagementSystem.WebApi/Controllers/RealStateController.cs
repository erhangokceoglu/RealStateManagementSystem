using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RealStateManagementSystem.Domain.Dtos.RealState;
using RealStateManagementSystem.Domain.Entities;
using RealStateManagementSystem.Domain.Enums;
using RealStateManagementSystem.Domain.IServices;
using RealStateManagementSystem.Domain.RequestParameters;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace RealStateManagementSystem.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RealStateController : ControllerBase
    {
        private readonly IAppUserService _appUserService;
        private readonly ILogService _logService;
        private readonly IRealStateService _realStateService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly Log _log;

        public RealStateController(IAppUserService appUserService, ILogService logService, IRealStateService realStateService, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, Log log)
        {
            _appUserService = appUserService;
            _logService = logService;
            _realStateService = realStateService;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _log = log;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var activeRealState = await _realStateService.GetByIdAsync(id);
            if (activeRealState == null)
            {
                var message = "Sistemde tasinmaz bulunamadı";
                return NotFound(new { message });
            }
            return Ok(activeRealState);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Pagination pagination)
        {
            var activeAppUser = await GetActiveAppUser();
            var realStates = await _realStateService.GetAllRealStatesAsync();
            var totalCount = await _realStateService.GetCountForRealStatesAsync();
            if (totalCount == 0)
            {
                _log.AppUserId = activeAppUser.Id;
                _log.State = "Başarısız";
                _log.Description = "Sistemde kayıtlı aktif taşınmazlar bulunamadı.";
                _log.ProcessType = "Taşınmaz Listeleme";
                _log.CreateDate = DateTime.Now;
                _log.IsActive = true;
                _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                    HttpContext.Connection.RemoteIpAddress?.ToString()!;
                await _logService.AddAsync(_log);
                return Ok(new
                {
                    totalCount,
                    realStates
                });

            }
            var listRealStates = realStates.Select(x => new
            {
                x.Id,
                Province = _realStateService.GetProvince(x.ProvinceId).Result.Name,
                District = _realStateService.GetDistrict(x.DistrictId).Result.Name,
                Neighbourhood = _realStateService.GetNeighbourhood(x.NeighbourhoodId).Result.Name,
                x.IslandNo,
                x.ParcelNo,
                Qualification =
                x.Qualification == Qualification.Land ? "Arsa" :
                x.Qualification == Qualification.Residential ? "Mesken" :
                x.Qualification == Qualification.Field ? "Tarla" : "Bilinmiyor",
                x.Address
            }).Skip(pagination.Page * pagination.Size).Take(pagination.Size).OrderByDescending(x => x.Id);
            return Ok(new
            {
                totalCount,
                listRealStates
            });
        }

        [HttpGet("{filter}")]
        public async Task<IActionResult> GetAllByFilter(string filter)
        {
            var realStates = _realStateService.GetAllFilter(filter);
            if (realStates.Result.ToList().Count == 0)
            {
                var allList = await _realStateService.GetAllRealStatesAsync();
                return Ok(allList);
            }
            return Ok(realStates);
        }

        [HttpGet]
        [Route("{skipValue}/{takeValue}")]
        public async Task<IActionResult> GetAllByValues(int skipValue, int takeValue)
        {
            var realStates = _realStateService.GetAllByValues(skipValue, takeValue);
            if (realStates.Result.ToList().Count == 0)
            {
                var allList = await _realStateService.GetAllRealStatesAsync();
                return Ok(allList);
            }
            return Ok(realStates);
        }

        [HttpGet]
        [Route("{skipValue}/{takeValue}/{filter}")]
        public async Task<IActionResult> GetAllSearchAndFilter(int skipValue, int takeValue, string filter)
        {
            var realStates = _realStateService.GetAllSearchAndFilter(skipValue, takeValue, filter);
            if (realStates.Result.ToList().Count == 0)
            {
                var allList = await _realStateService.GetAllRealStatesAsync();
                return Ok(allList);
            }
            return Ok(realStates);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateForRealStateDto createForRealStateDto)
        {
            var activeAppUser = await GetActiveAppUser();
            try
            {
                if (!ModelState.IsValid)
                {
                    _log.AppUserId = activeAppUser.Id;
                    _log.State = "Başarısız";
                    _log.ProcessType = "Taşınmaz Kayıt";
                    _log.Description = $" Sisteme {createForRealStateDto.IslandNo}-{createForRealStateDto.ParcelNo}'li taşınmaz beklenmedik bir" +
                     $"hatadan dolayı eklenememiştir.";
                    _log.CreateDate = createForRealStateDto.CreateDate;
                    _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                        HttpContext.Connection.RemoteIpAddress?.ToString()!;
                    _log.IsActive = true;
                    await _logService.AddAsync(_log);
                    return BadRequest(new { _log.Description });
                }
                var existRealState = await _realStateService.ExistsAsync(x => x.IslandNo == createForRealStateDto.IslandNo && x.ParcelNo == createForRealStateDto.ParcelNo);
                if (existRealState)
                {
                    _log.AppUserId = activeAppUser.Id;
                    _log.State = "Başarısız";
                    _log.ProcessType = "Taşınmaz Kayıt";
                    _log.Description = $" Sistemde {createForRealStateDto.IslandNo}-{createForRealStateDto.ParcelNo}'li tasinmaz mevcuttur." +
                     $"Başka taşınmaz giriniz.";
                    _log.CreateDate = createForRealStateDto.CreateDate;
                    _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                        HttpContext.Connection.RemoteIpAddress?.ToString()!;
                    _log.IsActive = true;
                    await _logService.AddAsync(_log);
                    return BadRequest(new { _log.Description });
                }
                else
                {
                    await _realStateService.AddRealStateAsync(createForRealStateDto, activeAppUser.Id);
                    _log.AppUserId = activeAppUser.Id;
                    _log.State = "Başarılı";
                    _log.ProcessType = "Taşınmaz Kayıt";
                    _log.Description = $" Sisteme {createForRealStateDto.IslandNo}-{createForRealStateDto.ParcelNo}'li taşınmaz başarılı " +
                     $"şekilde eklenmiştir.";
                    _log.CreateDate = createForRealStateDto.CreateDate;
                    _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                        HttpContext.Connection.RemoteIpAddress?.ToString()!;
                    _log.IsActive = true;
                    await _logService.AddAsync(_log);
                    return Ok(new { _log.Description });
                }
            }
            catch (Exception)
            {
                _log.AppUserId = activeAppUser.Id;
                _log.State = "Başarısız";
                _log.ProcessType = "Taşınmaz Kayıt";
                _log.Description = $" Sisteme {createForRealStateDto.IslandNo}-{createForRealStateDto.ParcelNo}'li taşınmaz beklenmedik bir" +
                 $"hatadan dolayı eklenememiştir.";
                _log.CreateDate = createForRealStateDto.CreateDate;
                _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                HttpContext.Connection.RemoteIpAddress?.ToString()!;
                _log.IsActive = true;
                await _logService.AddAsync(_log);
                return BadRequest(new { _log.Description });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateForRealStateDto updateForRealStateDto)
        {
            var activeAppUser = await GetActiveAppUser();
            var existingRealState = await _realStateService.GetByIdAsync(id);
            try
            {
                if (existingRealState == null)
                {
                    _log.AppUserId = activeAppUser.Id;
                    _log.State = "Başarısız";
                    _log.Description = "Taşınmaz sistemde bulunamadı.";
                    _log.ProcessType = "Taşınmaz Güncelleme";
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
                    _log.Description = "Taşınmaz güncelleme başarısız olmuştur.";
                    _log.ProcessType = "Taşınmaz Güncelleme";
                    _log.UpdateDate = DateTime.Now;
                    _log.IsActive = true;
                    _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                        HttpContext.Connection.RemoteIpAddress?.ToString()!;
                    await _logService.AddAsync(_log);
                    return BadRequest(new { _log.Description });
                }

                if (await _realStateService.UpdateRealStateAsync(id, updateForRealStateDto))
                {
                    _log.AppUserId = activeAppUser.Id;
                    _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                     HttpContext.Connection.RemoteIpAddress?.ToString()!;
                    _log.UpdateDate = DateTime.Now;
                    _log.State = "Başarılı";
                    _log.ProcessType = "Taşınmaz Güncelleme";
                    _log.Description = $"Sistemdeki {existingRealState.IslandNo}-{existingRealState.ParcelNo} taşınmaz başarılı " +
                     $"şekilde güncellenmiştir";
                    _log.IsActive = true;
                    await _logService.AddAsync(_log);
                    return Ok(new { _log.Description });
                }
                else
                {
                    _log.AppUserId = activeAppUser.Id;
                    _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                     HttpContext.Connection.RemoteIpAddress?.ToString()!;
                    _log.UpdateDate = DateTime.Now;
                    _log.State = "Başarısız";
                    _log.ProcessType = "Taşınmaz Güncelleme";
                    _log.Description = $"Sistemdeki {existingRealState.IslandNo}-{existingRealState.ParcelNo}'li taşınmaz güncellenememiştir.";
                    _log.IsActive = true;
                    await _logService.AddAsync(_log);
                    return BadRequest(new { _log.Description });
                }
            }
            catch (Exception)
            {
                _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                 HttpContext.Connection.RemoteIpAddress?.ToString()!;
                _log.UpdateDate = DateTime.Now;
                _log.State = "Başarısız";
                _log.ProcessType = "Taşınmaz Güncelleme";
                _log.Description = $"Sistemdeki {existingRealState.IslandNo} - {existingRealState.ParcelNo}'li taşınmaz beklenmedik bir" +
                 $"hatadan dolayı güncelenemiştir.";
                _log.IsActive = true;
                await _logService.AddAsync(_log);
                return BadRequest(new { _log.Description });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var realState = await _realStateService.GetByIdAsync(id);
            var deleteRealState = await _realStateService.DeleteAsync(id);
            try
            {
                if (deleteRealState)
                {
                    _log.AppUserId = realState.AppUserId;
                    _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                     HttpContext.Connection.RemoteIpAddress?.ToString()!;
                    _log.CreateDate = DateTime.Now;
                    _log.State = "Başarılı";
                    _log.ProcessType = "Taşınmaz Silme";
                    _log.Description = $"Sistemdeki {realState.IslandNo}-{realState.ParcelNo}'li taşınmaz başarılı bir şekilde " +
                     $"silinmiştir.";
                    _log.IsActive = true;
                    await _logService.AddAsync(_log);
                    return Ok(new {_log.Description});
                }
                _log.AppUserId = realState.AppUserId;
                _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                 HttpContext.Connection.RemoteIpAddress?.ToString()!;
                _log.CreateDate = DateTime.Now;
                _log.State = "Başarısız";
                _log.ProcessType = "Taşınmaz Silme";
                _log.Description = $"Sistemdeki {realState.IslandNo}-{realState.ParcelNo}'li taşınmaz silinmemiştir.";
                _log.IsActive = true;
                await _logService.AddAsync(_log);
                return Ok(new { _log.Description });
            }
            catch (Exception)
            {
                _log.UserIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                 HttpContext.Connection.RemoteIpAddress?.ToString()!;
                _log.CreateDate = realState.CreateDate;
                _log.State = "Başarısız";
                _log.ProcessType = "Taşınmaz Silme";
                _log.Description = $"Sistemdeki {realState.IslandNo}-{realState.ParcelNo}'li taşınmaz beklenmedik bir " +
                 $"hatadan dolayı silinememiştir.";
                _log.IsActive = true;
                await _logService.AddAsync(_log);
                return BadRequest(new { _log.Description });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Province>>> GetAllProvinces()
        {
            var getList = await _realStateService.GetAllProvincesAsync();
            return Ok(getList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<District>>> GetAllDistricts(int id)
        {
            var getList = await _realStateService.GetAllDistrictsAsync(id);
            return Ok(getList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Neighbourhood>>> GetAllNeighbourhoods(int id)
        {
            var getList = await _realStateService.GetAllNeighbourhoodsAsync(id);
            return Ok(getList);
        }

        private async Task<AppUser> GetActiveAppUser()
        {
            var activeAppUserId = ValidateToken(await _appUserService.GetLastToken()).Split(":")[2].Trim();
            var activeAppUser = await _appUserService.GetAppUserForProfileAsync(Convert.ToInt32(activeAppUserId));
            return activeAppUser;
        }

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
    }
}
