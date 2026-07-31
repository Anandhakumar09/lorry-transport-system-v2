using LorryTransport.Application.DTOs;
using LorryTransport.Application.Interfaces;
using LorryTransport.Domain.Entities;

namespace LorryTransport.Application.Services
{
    public class DriverLedgerService : IDriverLedgerService
    {
        private readonly IGenericRepository<Driver> _driverRepository;
        private readonly IGenericRepository<DriverPayment> _paymentRepository;

        public DriverLedgerService(
            IGenericRepository<Driver> driverRepository,
            IGenericRepository<DriverPayment> paymentRepository)
        {
            _driverRepository = driverRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task<List<DriverLedgerDto>> GetAllLedgersAsync()
        {
            var drivers = await _driverRepository.GetAllAsync();
            var payments = await _paymentRepository.GetAllAsync();

            var result = new List<DriverLedgerDto>();
            foreach (var driver in drivers)
            {
                var driverPayments = payments.Where(p => p.DriverId == driver.Id).ToList();
                result.Add(BuildLedger(driver, driverPayments));
            }
            return result;
        }

        public async Task<DriverLedgerDto?> GetLedgerByDriverIdAsync(int driverId)
        {
            var driver = await _driverRepository.GetByIdAsync(driverId);
            if (driver == null) return null;

            var payments = await _paymentRepository.GetAllAsync();
            var driverPayments = payments.Where(p => p.DriverId == driverId).ToList();

            return BuildLedger(driver, driverPayments);
        }

        private static DriverLedgerDto BuildLedger(Driver driver, List<DriverPayment> payments)
        {
            var totalAdvance = payments.Sum(p => p.AdvanceGiven);
            var totalSalary = payments.Sum(p => p.Salary);
            var totalExtra = payments.Sum(p => p.ExtraPaid);
            var totalPaid = totalAdvance + totalSalary + totalExtra;

            return new DriverLedgerDto
            {
                DriverId = driver.Id,
                DriverName = driver.Name,
                TotalAdvanceGiven = totalAdvance,
                TotalSalary = totalSalary,
                TotalExtraPaid = totalExtra,
                TotalPaid = totalPaid,
                // Remaining balance = what's owed minus what's been paid.
                // Here we assume salary+advance is the amount owed, adjust to your real rule later.
                RemainingBalance = totalSalary - totalPaid + totalExtra
            };
        }
    }
}
