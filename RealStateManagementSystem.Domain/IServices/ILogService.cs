using RealStateManagementSystem.Domain.Dtos.Log;
using RealStateManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RealStateManagementSystem.Domain.IServices
{
    public interface ILogService : IBaseService<Log>
    {
        Task<bool> AddLogAsync(CreateForLogDto createForLogDto);
        Task<ICollection<Log>> GetAllLogsAsync(int skipValue, int takeValue);
        Task<int> GetCountForLogAsync();
        Task<int> GetCountForLogFilterAsync(string filter);
        Task<ICollection<Log>> GetAllLogsFilterAsync(string filter);
        Task<ICollection<Log>> GetSearchAndFilterLogAsync(int skipValue, int takeValue, string filter);
    }
}
