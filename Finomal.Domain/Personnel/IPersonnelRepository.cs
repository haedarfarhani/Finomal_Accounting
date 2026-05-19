using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Finomal.Domain.Personnel
{
    public interface IPersonnelRepository
    {
        Task<Personnel?> GetByIdAsync(Guid id);
        Task<IEnumerable<Personnel>> GetAllAsync();
        Task AddAsync(Personnel personnel);
        Task UpdateAsync(Personnel personnel);
        Task DeleteAsync(Guid id);

        Task<PersonnelContract?> GetContractByIdAsync(Guid id);
        Task<IEnumerable<PersonnelContract>> GetAllContractsAsync();
        Task<IEnumerable<PersonnelContract>> GetContractsByPersonnelIdAsync(Guid personnelId);
        Task AddContractAsync(PersonnelContract contract);
        Task UpdateContractAsync(PersonnelContract contract);
        Task DeleteContractAsync(Guid id);

        Task<IEnumerable<AttendanceRecord>> GetAttendanceByDateRangeAsync(DateTime start, DateTime end);
        Task UpdateAttendanceRangeAsync(IEnumerable<AttendanceRecord> records);

        Task<IEnumerable<PayrollRecord>> GetPayrollByPeriodAsync(int year, int month);
        Task UpdatePayrollRangeAsync(IEnumerable<PayrollRecord> records);

        Task SaveChangesAsync();
    }
}
