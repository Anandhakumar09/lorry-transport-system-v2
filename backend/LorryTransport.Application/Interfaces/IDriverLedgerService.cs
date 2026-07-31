using LorryTransport.Application.DTOs;

namespace LorryTransport.Application.Interfaces
{
    public interface IDriverLedgerService
    {
        Task<List<DriverLedgerDto>> GetAllLedgersAsync();
        Task<DriverLedgerDto?> GetLedgerByDriverIdAsync(int driverId);
    }
}
