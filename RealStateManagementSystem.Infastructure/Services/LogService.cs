using Microsoft.EntityFrameworkCore;
using RealStateManagementSystem.Domain.Dtos.Log;
using RealStateManagementSystem.Domain.Entities;
using RealStateManagementSystem.Domain.IServices;
using RealStateManagementSystem.Infastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RealStateManagementSystem.Infastructure.Services
{
    public class LogService : BaseService<Log, ApplicationDbContext>, ILogService
    {
        private readonly ApplicationDbContext _db;
        private readonly Log _log;

        public LogService(ApplicationDbContext db, Log log) : base(db)
        {
            _db = db;
            _log = log;
        }

        public async Task<bool> AddLogAsync(CreateForLogDto createForLogDto)
        {
            _log.State = createForLogDto.State;
            _log.ProcessType = createForLogDto.ProcessType;
            _log.AppUserId = createForLogDto.AppUserId;
            _log.CreateDate = createForLogDto.CreateDate;
            _log.UserIp = createForLogDto.UserIp;
            _log.Description = createForLogDto.Description;
            _log.IsActive = createForLogDto.IsActive;
            return await AddAsync(_log);
        }

        public async Task<ICollection<Log>> GetAllLogsAsync(int skipValue, int takeValue)
        {
            List<Log> list = await _db.Logs.Include(x => x.AppUser)
            .OrderByDescending(x => x.CreateDate).Skip(skipValue).Take(takeValue).ToListAsync();
            if (list == null) { throw new System.NotImplementedException(); }
            return list;
        }

        public async Task<ICollection<Log>> GetAllLogsFilterAsync(string filter)
        {
            return await
            (from x in _db.Logs.Include(x => x.AppUser)
             where
             x.ProcessType!.ToLower().Contains(filter.ToLower()) ||
             x.CreateDate.HasValue.ToString().Contains(filter.ToLower()) ||
             x.AppUserId.ToString()!.ToLower().Contains(filter.ToLower()) ||
             x.State.ToLower().Contains(filter.ToLower()) ||
             x.Description!.ToLower().Contains(filter.ToLower()) ||
             x.UserIp.ToLower().Contains(filter.ToLower())
             select x
            ).Where(x => x.IsActive).OrderByDescending(x => x.CreateDate).ToListAsync();
        }

        public async Task<int> GetCountForLogAsync()
        {
            return await _db.Logs.Where(x => x.IsActive).OrderByDescending(x => x.CreateDate).CountAsync();
        }

        public async Task<int> GetCountForLogFilterAsync(string filter)
        {
            return await
            (from x in _db.Logs.Include(x => x.AppUser)
             where
             x.ProcessType!.ToLower().Contains(filter.ToLower()) ||
             x.CreateDate.HasValue.ToString().Contains(filter.ToLower()) ||
             x.AppUserId.ToString()!.ToLower().Contains(filter.ToLower()) ||
             x.State.ToLower().Contains(filter.ToLower()) ||
             x.Description!.ToLower().Contains(filter.ToLower()) ||
             x.UserIp.ToLower().Contains(filter.ToLower())
             select x
            ).Where(x => x.IsActive).OrderByDescending(x => x.CreateDate).CountAsync();
        }

        public async Task<ICollection<Log>> GetSearchAndFilterLogAsync(int skipValue, int takeValue, string filter)
        {
            List<Log> list;
            if (filter != "-1")
            {
                list = await _db.Logs.Include(x => x.AppUser)
                .Where(x => x.ProcessType!.ToLower().Contains(filter.ToLower()) ||
                            x.CreateDate.HasValue.ToString().Contains(filter.ToLower()) ||
                            x.AppUserId.ToString()!.ToLower().Contains(filter.ToLower()) ||
                            x.State.ToLower().Contains(filter.ToLower()) ||
                            x.Description!.ToLower().Contains(filter.ToLower()) ||
                            x.UserIp.ToLower().Contains(filter.ToLower())
                            && x.IsActive)
                .Skip(skipValue)
                .Take(takeValue)
                .OrderByDescending(x => x.CreateDate)
                .ToListAsync();
                return list;
            }
            else
            {
                list = await _db.Logs.Include(x => x.AppUser).Where(x => x.IsActive)
               .Skip(skipValue).Take(skipValue).OrderByDescending(x => x.CreateDate).ToListAsync();
            }

            if (list == null)
            {
                throw new System.NotImplementedException();
            }

            return list;
        }
    }
}
