using Finomal.Domain.Personnel;
using Finomal.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Finomal.Infrastructure.Data.Repositories
{
    public class PersonnelRepository : IPersonnelRepository
    {
        private readonly ApplicationDbContext _context;

        public PersonnelRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Personnel?> GetByIdAsync(Guid id)
        {
            return await _context.Personnel.FindAsync(id);
        }

        public async Task<IEnumerable<Personnel>> GetAllAsync()
        {
            return await _context.Personnel.ToListAsync();
        }

        public async Task AddAsync(Personnel personnel)
        {
            await _context.Personnel.AddAsync(personnel);
        }

        public Task UpdateAsync(Personnel personnel)
        {
            _context.Personnel.Update(personnel);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            var personnel = await GetByIdAsync(id);
            if (personnel != null)
            {
                personnel.IsDeleted = true;
                _context.Personnel.Update(personnel);
            }
        }

        public async Task<PersonnelContract?> GetContractByIdAsync(Guid id)
        {
            return await _context.PersonnelContracts.FindAsync(id);
        }

        public async Task<IEnumerable<PersonnelContract>> GetAllContractsAsync()
        {
            return await _context.PersonnelContracts
                .Include(c => c.Personnel)
                .ToListAsync();
        }

        public async Task<IEnumerable<PersonnelContract>> GetContractsByPersonnelIdAsync(Guid personnelId)
        {
            return await _context.PersonnelContracts
                .Where(c => c.PersonnelId == personnelId)
                .ToListAsync();
        }

        public async Task AddContractAsync(PersonnelContract contract)
        {
            await _context.PersonnelContracts.AddAsync(contract);
        }

        public Task UpdateContractAsync(PersonnelContract contract)
        {
            _context.PersonnelContracts.Update(contract);
            return Task.CompletedTask;
        }

        public async Task DeleteContractAsync(Guid id)
        {
            var contract = await GetContractByIdAsync(id);
            if (contract != null)
            {
                contract.IsDeleted = true;
                _context.PersonnelContracts.Update(contract);
            }
        }

        public async Task<IEnumerable<AttendanceRecord>> GetAttendanceByDateRangeAsync(DateTime start, DateTime end)
        {
            return await _context.AttendanceRecords
                .Where(a => a.Date >= start && a.Date <= end)
                .ToListAsync();
        }

        public async Task UpdateAttendanceRangeAsync(IEnumerable<AttendanceRecord> records)
        {
            foreach (var record in records)
            {
                var existing = await _context.AttendanceRecords
                    .FirstOrDefaultAsync(a => a.PersonnelId == record.PersonnelId && a.Date == record.Date);

                if (existing != null)
                {
                    existing.Status = record.Status;
                    _context.AttendanceRecords.Update(existing);
                }
                else
                {
                    await _context.AttendanceRecords.AddAsync(record);
                }
            }
        }

        public async Task<IEnumerable<PayrollRecord>> GetPayrollByPeriodAsync(int year, int month)
        {
            return await _context.PayrollRecords
                .Include(p => p.Personnel)
                .Where(p => p.Year == year && p.Month == month)
                .ToListAsync();
        }

        public async Task UpdatePayrollRangeAsync(IEnumerable<PayrollRecord> records)
        {
            foreach (var record in records)
            {
                var existing = await _context.PayrollRecords
                    .FirstOrDefaultAsync(p => p.PersonnelId == record.PersonnelId && p.Year == record.Year && p.Month == record.Month);

                if (existing != null)
                {
                    _context.Entry(existing).CurrentValues.SetValues(record);
                    _context.PayrollRecords.Update(existing);
                }
                else
                {
                    await _context.PayrollRecords.AddAsync(record);
                }
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
