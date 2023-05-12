using RealStateManagementSystem.Domain.Dtos.Log;
using RealStateManagementSystem.Domain.Dtos.RealState;
using RealStateManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateManagementSystem.Domain.IServices
{
    public interface IRealStateService : IBaseService<RealState>
    {
        Task<bool> AddRealStateAsync(CreateForRealStateDto createForRealStateDto, int id);
        Task<bool> UpdateRealStateAsync(int id,UpdateForRealStateDto updateForRealStateDto);
        Task<IEnumerable<RealState>> GetAllFilter(string filter);
        Task<IEnumerable<RealState>> GetAllSearchAndFilter(int skipValue, int takeValue, string filter);
        Task<IEnumerable<RealState>> GetAllByValues(int skipValue, int takeValue);
        Task<RealState> GetRealStateByIdAsync(int id);
        Task<IEnumerable<Province>> GetAllProvincesAsync();
        Task<IEnumerable<District>> GetAllDistrictsAsync(int id);
        Task<IEnumerable<Neighbourhood>> GetAllNeighbourhoodsAsync(int id);
        Task<Province> GetProvince(int id);
        Task<District> GetDistrict(int id);
        Task<Neighbourhood> GetNeighbourhood(int id);
        Task<IEnumerable<RealState>> GetAllRealStatesAsync();
        Task<int> GetCountForRealStatesAsync();
    }
}
