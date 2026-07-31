using LorryTransport.Application.DTOs;
using LorryTransport.Application.Interfaces;
using LorryTransport.Domain.Entities;

namespace LorryTransport.Application.Services
{
    // This class holds ALL the business rules for a Load Entry (trip).
    // The auto-calculations (Freight, Expense Total, Profit, Driver Balance) happen HERE
    // so the same logic is used no matter where it's called from (API, background job, etc).
    public class LoadEntryService : ILoadEntryService
    {
        private readonly IGenericRepository<LoadEntry> _repository;

        public LoadEntryService(IGenericRepository<LoadEntry> repository)
        {
            _repository = repository;
        }

        // Central calculation method - single source of truth for the formulas.
        private void ApplyCalculations(LoadEntry entry)
        {
            // Freight Amount = Rate Per Ton x Total Tons
            entry.FreightAmount = entry.RatePerTon * entry.TotalTons;

            // Expense Total = Loading + Diesel + Driver Salary + Cleaner Salary + Commission + Other + Advance
            entry.ExpenseTotal = entry.LoadingCharge
                                 + entry.DieselAmount
                                 + entry.DriverSalary
                                 + entry.CleanerSalary
                                 + entry.Commission
                                 + entry.OtherExpenses
                                 + entry.AdvanceAmount;

            // Profit = Freight Amount - Expense Total
            entry.Profit = entry.FreightAmount - entry.ExpenseTotal;

            // Driver Balance = Advance + Driver Salary
            entry.DriverBalance = entry.AdvanceAmount + entry.DriverSalary;
        }

        public async Task<List<LoadEntryDto>> GetAllAsync()
        {
            var entries = await _repository.GetAllAsync();
            return entries.Select(MapToDto).OrderByDescending(e => e.Date).ToList();
        }

        public async Task<LoadEntryDto?> GetByIdAsync(int id)
        {
            var entry = await _repository.GetByIdAsync(id);
            return entry == null ? null : MapToDto(entry);
        }

        public async Task<LoadEntryDto> CreateAsync(CreateLoadEntryDto dto)
        {
            var entry = new LoadEntry
            {
                Date = dto.Date,
                FromLocation = dto.FromLocation,
                ToLocation = dto.ToLocation,
                CustomerId = dto.CustomerId,
                MaterialName = dto.MaterialName,
                VehicleId = dto.VehicleId,
                DriverId = dto.DriverId,
                AdvanceAmount = dto.AdvanceAmount,
                LoadingCharge = dto.LoadingCharge,
                RatePerTon = dto.RatePerTon,
                TotalTons = dto.TotalTons,
                DieselAmount = dto.DieselAmount,
                DriverSalary = dto.DriverSalary,
                CleanerSalary = dto.CleanerSalary,
                Commission = dto.Commission,
                OtherExpenses = dto.OtherExpenses,
                Notes = dto.Notes
            };

            ApplyCalculations(entry);

            await _repository.AddAsync(entry);
            await _repository.SaveChangesAsync();

            return MapToDto(entry);
        }

        public async Task<bool> UpdateAsync(int id, CreateLoadEntryDto dto)
        {
            var entry = await _repository.GetByIdAsync(id);
            if (entry == null) return false;

            entry.Date = dto.Date;
            entry.FromLocation = dto.FromLocation;
            entry.ToLocation = dto.ToLocation;
            entry.CustomerId = dto.CustomerId;
            entry.MaterialName = dto.MaterialName;
            entry.VehicleId = dto.VehicleId;
            entry.DriverId = dto.DriverId;
            entry.AdvanceAmount = dto.AdvanceAmount;
            entry.LoadingCharge = dto.LoadingCharge;
            entry.RatePerTon = dto.RatePerTon;
            entry.TotalTons = dto.TotalTons;
            entry.DieselAmount = dto.DieselAmount;
            entry.DriverSalary = dto.DriverSalary;
            entry.CleanerSalary = dto.CleanerSalary;
            entry.Commission = dto.Commission;
            entry.OtherExpenses = dto.OtherExpenses;
            entry.Notes = dto.Notes;

            ApplyCalculations(entry);

            _repository.Update(entry);
            return await _repository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entry = await _repository.GetByIdAsync(id);
            if (entry == null) return false;

            _repository.Delete(entry);
            return await _repository.SaveChangesAsync();
        }

        public async Task<DashboardDto> GetDashboardAsync()
        {
            var all = await _repository.GetAllAsync();
            var today = DateTime.Today;

            var todaysTrips = all.Where(e => e.Date.Date == today).ToList();
            var thisMonthTrips = all.Where(e => e.Date.Month == today.Month && e.Date.Year == today.Year).ToList();

            return new DashboardDto
            {
                TodaysTrips = todaysTrips.Count,
                ThisMonthTrips = thisMonthTrips.Count,
                TotalIncome = all.Sum(e => e.FreightAmount),
                TotalExpense = all.Sum(e => e.ExpenseTotal),
                TotalProfit = all.Sum(e => e.Profit),
                TotalDieselExpense = all.Sum(e => e.DieselAmount),
                TotalDriverSalary = all.Sum(e => e.DriverSalary),
                TotalCleanerSalary = all.Sum(e => e.CleanerSalary),
                TotalCommission = all.Sum(e => e.Commission),
                PendingDriverBalance = all.Sum(e => e.DriverBalance)
            };
        }

        private static LoadEntryDto MapToDto(LoadEntry e) => new()
        {
            Id = e.Id,
            Date = e.Date,
            FromLocation = e.FromLocation,
            ToLocation = e.ToLocation,
            CustomerId = e.CustomerId,
            MaterialName = e.MaterialName,
            VehicleId = e.VehicleId,
            DriverId = e.DriverId,
            AdvanceAmount = e.AdvanceAmount,
            LoadingCharge = e.LoadingCharge,
            RatePerTon = e.RatePerTon,
            TotalTons = e.TotalTons,
            FreightAmount = e.FreightAmount,
            DieselAmount = e.DieselAmount,
            DriverSalary = e.DriverSalary,
            CleanerSalary = e.CleanerSalary,
            Commission = e.Commission,
            OtherExpenses = e.OtherExpenses,
            ExpenseTotal = e.ExpenseTotal,
            Profit = e.Profit,
            DriverBalance = e.DriverBalance,
            Notes = e.Notes
        };
    }
}
