using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RealStateManagementSystem.Domain.Dtos.RealState;
using RealStateManagementSystem.Domain.Entities;
using RealStateManagementSystem.Domain.Enums;
using RealStateManagementSystem.Domain.IServices;
using RealStateManagementSystem.Infastructure.Data;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateManagementSystem.Infastructure.Services
{
    public class RealStateService : BaseService<RealState, ApplicationDbContext>, IRealStateService
    {
        private readonly ApplicationDbContext _db;
        private readonly RealState _realState;
        private readonly Log _log;
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly ILogService _logService;
        private readonly IAppUserService _appUserService;

        public RealStateService(ApplicationDbContext db, RealState realState, Log log, IHttpContextAccessor httpAccessor, ILogService logService, IAppUserService appUserService) : base(db)
        {
            _db = db;
            _realState = realState;
            _log = log;
            _httpAccessor = httpAccessor;
            _logService = logService;
            _appUserService = appUserService;
        }

        public async Task<bool> AddRealStateAsync(CreateForRealStateDto createForRealStateDto, int id)
        {
            _realState.AppUserId = id;
            _realState.Latitude = createForRealStateDto.Latitude;
            _realState.Longitude = createForRealStateDto.Longitude;
            _realState.CreateDate = createForRealStateDto.CreateDate;
            _realState.Address = createForRealStateDto.Address;
            _realState.ProvinceId = createForRealStateDto.ProvinceId;
            _realState.DistrictId = createForRealStateDto.DistrictId;
            _realState.NeighbourhoodId = createForRealStateDto.NeighbourhoodId;
            _realState.Qualification = createForRealStateDto.Qualification;
            _realState.ParcelNo = createForRealStateDto.ParcelNo;
            _realState.IslandNo = createForRealStateDto.IslandNo;
            _realState.IsActive = createForRealStateDto.IsActive;
            await AddAsync(_realState);
            return true;
        }

        public async Task<bool> UpdateRealStateAsync(int id, UpdateForRealStateDto updateForRealStateDto)
        {
            var getRealState = await _db.RealStates.FindAsync(id);
            if (getRealState != null)
            {
                getRealState.ProvinceId = updateForRealStateDto.ProvinceId;
                getRealState.DistrictId = updateForRealStateDto.DistrictId;
                getRealState.NeighbourhoodId = updateForRealStateDto.NeighbourhoodId;
                getRealState.Qualification = 
                updateForRealStateDto.Qualification == 3 ? getRealState.Qualification = Qualification.Field
                :updateForRealStateDto.Qualification == 2 ? getRealState.Qualification = Qualification.Residential
                :updateForRealStateDto.Qualification == 1 ? getRealState.Qualification = Qualification.Land
                :getRealState.Qualification;
                getRealState.IslandNo = updateForRealStateDto.IslandNo;
                getRealState.ParcelNo = updateForRealStateDto.ParcelNo;
                getRealState.Address = updateForRealStateDto.Address;
                getRealState.UpdateDate = updateForRealStateDto.UpdateDate;
                getRealState.IsActive = updateForRealStateDto.IsActive;
                await UpdateAsync(getRealState);
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Province>> GetAllProvincesAsync()
        {
            return await _db.Provinces.ToListAsync();
        }

        public async Task<IEnumerable<District>> GetAllDistrictsAsync(int id)
        {
            return await _db.Districts.Include(x => x.Province)
            .Where(x => x.ProvinceId == id).ToListAsync();
        }

        public async Task<IEnumerable<Neighbourhood>> GetAllNeighbourhoodsAsync(int id)
        {
            return await _db.Neighbourhoods.Include(x => x.District)
            .Where(x => x.DistrictId == id).ToListAsync();
        }

        public async Task<Province> GetProvince(int id)
        {
            var province = await _db.Provinces.FindAsync(id);
            return province!;
        }

        public async Task<District> GetDistrict(int id)
        {
            var district = await _db.Districts.FindAsync(id);
            return district!;
        }

        public async Task<Neighbourhood> GetNeighbourhood(int id)
        {
            var neighbourhood = await _db.Neighbourhoods.FindAsync(id);
            return neighbourhood!;
        }

        public async Task<IEnumerable<RealState>> GetAllRealStatesAsync()
        {
            var realStatesList = await _db.RealStates.Include(x => x.Neighbourhood).ThenInclude(x => x.District)
                        .ThenInclude(x => x.Province).Where(x => x.IsActive)
                        .OrderByDescending(x => x.CreateDate).ToListAsync();
            return realStatesList;
        }

        public async Task<RealState> GetRealStateByIdAsync(int id)
        {
            var realState = await _db.RealStates.Include(x => x.Neighbourhood).ThenInclude(x => x.District)
            .ThenInclude(x => x.Province).Where(x => x.IsActive).OrderByDescending(x => x.CreateDate).FirstOrDefaultAsync(x => x.Id == id);
            return realState!;
        }

        public async Task<IEnumerable<RealState>> GetAllByValues(int skipValue, int takeValue)
        {

            return await _db.RealStates.Where(x => x.IsActive).OrderByDescending(x => x.CreateDate)
            .Skip(skipValue).Take(takeValue).ToListAsync();
        }

        public async Task<IEnumerable<RealState>> GetAllFilter(string filter)
        {
            return await _db.RealStates.Include(x => x.Neighbourhood).ThenInclude(x => x.District)
            .ThenInclude(x => x.Province).Where(r =>
                                    r.Neighbourhood.Name.ToLower().Contains(filter.Trim().ToLower()) ||
                                    r.District.Name.ToLower().Contains(filter.Trim().ToLower()) ||
                                    r.Province.Name.ToLower().Contains(filter.Trim().ToLower()) ||
                                    r.IslandNo.ToLower().Contains(filter.Trim().ToLower()) ||
                                    r.ParcelNo.ToLower().Contains(filter.Trim().ToLower()) ||
                                    r.Address!.ToLower().Contains(filter.Trim().ToLower()) && r.IsActive)
            .OrderByDescending(x => x.CreateDate).ToListAsync();
        }

        public async Task<IEnumerable<RealState>> GetAllSearchAndFilter(int skipValue, int takeValue, string filter)
        {
            ICollection<RealState> realStates;
            if (filter != "-1")
            {
                realStates = await _db.RealStates.Include(x => x.Neighbourhood).ThenInclude(x => x.District)
                .ThenInclude(x => x.Province).Where(r =>
                                    r.Neighbourhood.Name.ToLower().Contains(filter.Trim().ToLower()) ||
                                    r.District.Name.ToLower().Contains(filter.Trim().ToLower()) ||
                                    r.Province.Name.ToLower().Contains(filter.Trim().ToLower()) ||
                                    r.IslandNo.ToLower().Contains(filter.Trim().ToLower()) ||
                                    r.ParcelNo.ToLower().Contains(filter.Trim().ToLower()) ||
                                    r.Address!.ToLower().Contains(filter.Trim().ToLower()) && r.IsActive)
                .OrderByDescending(x => x.CreateDate).Skip(skipValue).Take(takeValue).ToListAsync();
            }
            else
            {
                realStates = await _db.RealStates.Include(x => x.Neighbourhood).ThenInclude(x => x.District)
                .ThenInclude(x => x.Province).Where(x => x.IsActive).OrderByDescending(x => x.CreateDate).ToListAsync();
            }

            if (realStates == null)
            {
                throw new System.NotImplementedException();
            }
            return realStates;
        }

        public async Task<int> GetCountForRealStatesAsync()
        {
            return await _db.RealStates.Where(x => x.IsActive).OrderByDescending(x => x.CreateDate).CountAsync();
        }
    }
}
