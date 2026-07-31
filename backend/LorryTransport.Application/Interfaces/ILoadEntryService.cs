using LorryTransport.Application.DTOs;

namespace LorryTransport.Application.Interfaces
{
    public interface ILoadEntryService
    {
        Task<List<LoadEntryDto>> GetAllAsync();
        Task<LoadEntryDto?> GetByIdAsync(int id);
        Task<LoadEntryDto> CreateAsync(CreateLoadEntryDto dto);
        Task<bool> UpdateAsync(int id, CreateLoadEntryDto dto);
        Task<bool> DeleteAsync(int id);
        Task<DashboardDto> GetDashboardAsync();
    }
}
