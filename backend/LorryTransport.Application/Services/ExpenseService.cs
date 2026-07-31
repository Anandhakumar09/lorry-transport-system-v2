using LorryTransport.Application.DTOs;
using LorryTransport.Application.Interfaces;
using LorryTransport.Domain.Entities;

namespace LorryTransport.Application.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IGenericRepository<Expense> _repository;

        public ExpenseService(IGenericRepository<Expense> repository)
        {
            _repository = repository;
        }

        public async Task<List<ExpenseDto>> GetAllAsync()
        {
            var list = await _repository.GetAllAsync();
            return list.OrderByDescending(e => e.Date).Select(e => new ExpenseDto
            {
                Id = e.Id,
                Date = e.Date,
                ExpenseType = e.ExpenseType,
                Amount = e.Amount,
                Remarks = e.Remarks
            }).ToList();
        }

        public async Task<ExpenseDto> CreateAsync(CreateExpenseDto dto)
        {
            var expense = new Expense
            {
                Date = dto.Date,
                ExpenseType = dto.ExpenseType,
                Amount = dto.Amount,
                Remarks = dto.Remarks
            };

            await _repository.AddAsync(expense);
            await _repository.SaveChangesAsync();

            return new ExpenseDto
            {
                Id = expense.Id,
                Date = expense.Date,
                ExpenseType = expense.ExpenseType,
                Amount = expense.Amount,
                Remarks = expense.Remarks
            };
        }

        public async Task<bool> UpdateAsync(int id, CreateExpenseDto dto)
        {
            var expense = await _repository.GetByIdAsync(id);
            if (expense == null) return false;

            expense.Date = dto.Date;
            expense.ExpenseType = dto.ExpenseType;
            expense.Amount = dto.Amount;
            expense.Remarks = dto.Remarks;

            _repository.Update(expense);
            return await _repository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var expense = await _repository.GetByIdAsync(id);
            if (expense == null) return false;

            _repository.Delete(expense);
            return await _repository.SaveChangesAsync();
        }
    }
}
