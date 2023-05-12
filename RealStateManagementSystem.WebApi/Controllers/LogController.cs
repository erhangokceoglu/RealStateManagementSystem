using Microsoft.AspNetCore.Mvc;
using RealStateManagementSystem.Domain.Entities;
using RealStateManagementSystem.Domain.IServices;
using RealStateManagementSystem.Domain.RequestParameters;

namespace RealStateManagementSystem.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Log>>> GetLogs([FromQuery] Pagination pagination)
        {
            var getAll = await _logService.GetAllAsync();
            var totalCount = await _logService.GetCountForLogAsync();
            try
            {
                if (totalCount == 0)
                {
                    var message = "Loglar bulunamadı";
                    return NotFound(new { message });
                }
                var listLogs = getAll.Select(x => new
                {
                    x.Id,
                    x.State,
                    x.Description,
                    x.ProcessType,
                    x.UserIp,
                    x.AppUserId,
                    x.CreateDate
                }).Skip(pagination.Page * pagination.Size).Take(pagination.Size).OrderByDescending(x=>x.Id);
                return Ok(new
                {
                    totalCount,
                    listLogs
                });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
