using LorryTransport.Application.DTOs;

namespace LorryTransport.Application.Interfaces
{
    public interface IExpenseService
    {
        Task<List<ExpenseDto>> GetAllAsync();
        Task<ExpenseDto> CreateAsync(CreateExpenseDto dto);
        Task<bool> UpdateAsync(int id, CreateExpenseDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
