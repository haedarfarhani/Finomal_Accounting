using Finomal.Application.Personnel.Dtos;
using Finomal.Domain.Personnel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Finomal.API.Hubs
{
    [Authorize]
    public class PersonnelHub : Hub
    {
        private readonly IPersonnelRepository _repository;
        private readonly ILogger<PersonnelHub> _logger;

        public PersonnelHub(IPersonnelRepository repository, ILogger<PersonnelHub> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task GetPersonnelList()
        {
            try
            {
                var personnelList = await _repository.GetAllAsync();
                var dtos = personnelList.Where(p => !p.IsDeleted).Select(p => new PersonnelDto
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    FatherName = p.FatherName,
                    NationalCode = p.NationalCode,
                    IdNumber = p.IdNumber,
                    BirthDate = p.BirthDate,
                    Gender = p.Gender,
                    MaritalStatus = p.MaritalStatus,
                    ChildrenCount = p.ChildrenCount,
                    Mobile = p.Mobile,
                    Phone = p.Phone,
                    Address = p.Address,
                    PersonnelCode = p.PersonnelCode,
                    HireDate = p.HireDate,
                    LeaveDate = p.LeaveDate,
                    EmploymentType = p.EmploymentType,
                    Department = p.Department,
                    JobTitle = p.JobTitle,
                    CostCenter = p.CostCenter,
                    WorkLocation = p.WorkLocation,
                    BankName = p.BankName,
                    AccountNumber = p.AccountNumber,
                    Iban = p.Iban,
                    AccountCodeCurrent = p.AccountCodeCurrent,
                    AccountCodeLoan = p.AccountCodeLoan,
                    AccountCodeAdvance = p.AccountCodeAdvance,
                    BaseSalary = p.BaseSalary,
                    HousingAllowance = p.HousingAllowance,
                    FoodAllowance = p.FoodAllowance,
                    ChildAllowance = p.ChildAllowance,
                    OvertimeFixed = p.OvertimeFixed,
                    OtherAllowance = p.OtherAllowance,
                    InsuranceNumber = p.InsuranceNumber,
                    InsuranceType = p.InsuranceType,
                    EmployeeInsurancePct = p.EmployeeInsurancePct,
                    EmployerInsurancePct = p.EmployerInsurancePct,
                    UnemploymentInsurance = p.UnemploymentInsurance,
                    TaxExemptionType = p.TaxExemptionType,
                    TaxServiceLocation = p.TaxServiceLocation,
                    EmployerEconomicCode = p.EmployerEconomicCode,
                    EmployerNationalId = p.EmployerNationalId,
                    IncomeTaxable = p.IncomeTaxable,
                    IncludeInInsuranceList = p.IncludeInInsuranceList,
                    Status = p.Status
                }).ToList();

                await Clients.Caller.SendAsync("ReceivePersonnelList", dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting personnel list");
                await Clients.Caller.SendAsync("ReceiveError", "خطا در دریافت لیست پرسنل");
            }
        }

        public async Task CreatePersonnel(CreatePersonnelRequest request)
        {
            try
            {
                var personnel = new Finomal.Domain.Personnel.Personnel
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    FatherName = request.FatherName,
                    NationalCode = request.NationalCode,
                    IdNumber = request.IdNumber,
                    BirthDate = request.BirthDate,
                    Gender = request.Gender,
                    MaritalStatus = request.MaritalStatus,
                    ChildrenCount = request.ChildrenCount,
                    Mobile = request.Mobile,
                    Phone = request.Phone,
                    Address = request.Address,
                    PersonnelCode = request.PersonnelCode,
                    HireDate = request.HireDate,
                    LeaveDate = request.LeaveDate,
                    EmploymentType = request.EmploymentType,
                    Department = request.Department,
                    JobTitle = request.JobTitle,
                    CostCenter = request.CostCenter,
                    WorkLocation = request.WorkLocation,
                    BankName = request.BankName,
                    AccountNumber = request.AccountNumber,
                    Iban = request.Iban,
                    AccountCodeCurrent = request.AccountCodeCurrent,
                    AccountCodeLoan = request.AccountCodeLoan,
                    AccountCodeAdvance = request.AccountCodeAdvance,
                    BaseSalary = request.BaseSalary,
                    HousingAllowance = request.HousingAllowance,
                    FoodAllowance = request.FoodAllowance,
                    ChildAllowance = request.ChildAllowance,
                    OvertimeFixed = request.OvertimeFixed,
                    OtherAllowance = request.OtherAllowance,
                    InsuranceNumber = request.InsuranceNumber,
                    InsuranceType = request.InsuranceType,
                    EmployeeInsurancePct = request.EmployeeInsurancePct,
                    EmployerInsurancePct = request.EmployerInsurancePct,
                    UnemploymentInsurance = request.UnemploymentInsurance,
                    TaxExemptionType = request.TaxExemptionType,
                    TaxServiceLocation = request.TaxServiceLocation,
                    EmployerEconomicCode = request.EmployerEconomicCode,
                    EmployerNationalId = request.EmployerNationalId,
                    IncomeTaxable = request.IncomeTaxable,
                    IncludeInInsuranceList = request.IncludeInInsuranceList,
                    Status = "active"
                };

                await _repository.AddAsync(personnel);
                await _repository.SaveChangesAsync();

                // Broadcast to all clients to refresh lists
                await Clients.All.SendAsync("PersonnelAdded", new { id = personnel.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating personnel");
                await Clients.Caller.SendAsync("ReceiveError", "خطا در ثبت پرسنل. لطفا اطلاعات وارد شده را چک کنید.");
            }
        }

        public async Task TogglePersonnelStatus(Guid id, string status)
        {
            try
            {
                var personnel = await _repository.GetByIdAsync(id);
                if (personnel != null)
                {
                    personnel.Status = status;
                    await _repository.UpdateAsync(personnel);
                    await _repository.SaveChangesAsync();

                    await Clients.All.SendAsync("PersonnelStatusUpdated", new { id, status });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling status");
                await Clients.Caller.SendAsync("ReceiveError", "خطا در تغییر وضعیت");
            }
        }

        public async Task DeletePersonnel(Guid id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                await _repository.SaveChangesAsync();

                await Clients.All.SendAsync("PersonnelDeleted", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting personnel");
                await Clients.Caller.SendAsync("ReceiveError", "خطا در حذف پرسنل");
            }
        }

        public async Task GetContractsList()
        {
            try
            {
                var contracts = await _repository.GetAllContractsAsync();
                // To avoid errors with substring if names are empty, use safe access
                var dtos = contracts.Where(c => !c.IsDeleted).Select(c => new PersonnelContractDto
                {
                    Id = c.Id,
                    PersonnelId = c.PersonnelId,
                    PersonnelName = c.Personnel != null ? $"{c.Personnel.FirstName} {c.Personnel.LastName}".Trim() : "نامشخص",
                    PersonnelInitials = c.Personnel != null && c.Personnel.FirstName.Length > 0 && c.Personnel.LastName.Length > 0
                        ? $"{c.Personnel.FirstName[0]}.{c.Personnel.LastName[0]}"
                        : "-",
                    JobTitle = c.Personnel != null ? c.Personnel.JobTitle : "",
                    Title = c.Title,
                    ContractNumber = c.ContractNumber,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    StartDateString = c.StartDate.ToString("yyyy/MM/dd"),
                    EndDateString = c.EndDate.ToString("yyyy/MM/dd"),
                    MonthlySalary = c.MonthlySalary,
                    IsActive = c.IsActive,
                    IsExpiringSoon = (c.EndDate - DateTime.UtcNow).TotalDays < 30 && c.EndDate >= DateTime.UtcNow,
                    Status = c.IsActive ? "فعال" : "پایان یافته"
                }).ToList();

                await Clients.Caller.SendAsync("ReceiveContractsList", dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting contracts");
                await Clients.Caller.SendAsync("ReceiveError", "خطا در دریافت لیست قراردادها");
            }
        }

        public async Task DeleteContract(Guid id)
        {
            try
            {
                await _repository.DeleteContractAsync(id);
                await _repository.SaveChangesAsync();

                await Clients.All.SendAsync("ContractDeleted", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting contract");
                await Clients.Caller.SendAsync("ReceiveError", "خطا در حذف قرارداد");
            }
        }

        public async Task GetMonthlyAttendance(int year, int month)
        {
            try
            {
                var pc = new System.Globalization.PersianCalendar();
                var start = pc.ToDateTime(year, month, 1, 0, 0, 0, 0);
                var daysInMonth = month <= 6 ? 31 : month <= 11 ? 30 : pc.IsLeapYear(year) ? 30 : 29;
                var end = pc.ToDateTime(year, month, daysInMonth, 23, 59, 59, 0);

                var records = await _repository.GetAttendanceByDateRangeAsync(start, end);
                var dtos = records.Select(a => new AttendanceDto
                {
                    PersonnelId = a.PersonnelId,
                    Date = a.Date,
                    Day = pc.GetDayOfMonth(a.Date),
                    Status = a.Status
                }).ToList();

                await Clients.Caller.SendAsync("ReceiveAttendance", dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting attendance");
                await Clients.Caller.SendAsync("ReceiveError", "خطا در دریافت اطلاعات کارکرد");
            }
        }

        public async Task SaveAttendance(SaveAttendanceRequest request)
        {
            try
            {
                var pc = new System.Globalization.PersianCalendar();
                var records = request.Records.Select(r => new AttendanceRecord
                {
                    PersonnelId = r.PersonnelId,
                    Date = pc.ToDateTime(request.Year, request.Month, r.Day, 0, 0, 0, 0),
                    Status = r.Status
                }).ToList();

                await _repository.UpdateAttendanceRangeAsync(records);
                await _repository.SaveChangesAsync();

                await Clients.Caller.SendAsync("AttendanceSaved");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving attendance");
                await Clients.Caller.SendAsync("ReceiveError", "خطا در ذخیره کارکرد");
            }
        }

        public async Task GetPayrollList(int year, int month)
        {
            try
            {
                var payrolls = await _repository.GetPayrollByPeriodAsync(year, month);
                var dtos = payrolls.Select(p => new PayrollDto
                {
                    Id = p.Id,
                    PersonnelId = p.PersonnelId,
                    PersonnelName = p.Personnel != null ? $"{p.Personnel.FirstName} {p.Personnel.LastName}" : "نامشخص",
                    PersonnelCode = p.Personnel?.PersonnelCode ?? "-",
                    Year = p.Year,
                    Month = p.Month,
                    BaseSalary = p.BaseSalary,
                    TotalAllowances = p.HousingAllowance + p.FoodAllowance + p.ChildAllowance + p.OvertimeAmount + p.OtherAllowances,
                    TotalDeductions = p.InsuranceDeduction + p.TaxDeduction + p.OtherDeductions,
                    NetPay = p.NetPay,
                    Status = p.Status == "Calculated" ? "محاسبه شده" : p.Status == "Confirmed" ? "تایید نهایی" : "خام",
                    HousingAllowance = p.HousingAllowance,
                    FoodAllowance = p.FoodAllowance,
                    ChildAllowance = p.ChildAllowance,
                    OvertimeAmount = p.OvertimeAmount,
                    OtherAllowances = p.OtherAllowances,
                    InsuranceDeduction = p.InsuranceDeduction,
                    TaxDeduction = p.TaxDeduction,
                    OtherDeductions = p.OtherDeductions,
                    WorkDays = 30 // Simplified for now, should ideally come from attendance summary
                }).ToList();

                await Clients.Caller.SendAsync("ReceivePayrollList", dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payroll list");
                await Clients.Caller.SendAsync("ReceiveError", "خطا در دریافت لیست حقوق");
            }
        }

        public async Task CalculatePayroll(int year, int month)
        {
            try
            {
                var personnel = await _repository.GetAllAsync();
                var activePersonnel = personnel.Where(p => !p.IsDeleted && p.Status == "active").ToList();

                var pc = new System.Globalization.PersianCalendar();
                var start = pc.ToDateTime(year, month, 1, 0, 0, 0, 0);
                var daysInMonth = month <= 6 ? 31 : month <= 11 ? 30 : pc.IsLeapYear(year) ? 30 : 29;
                var end = pc.ToDateTime(year, month, daysInMonth, 23, 59, 59, 0);

                var attendance = await _repository.GetAttendanceByDateRangeAsync(start, end);

                var newPayrolls = new List<PayrollRecord>();

                foreach (var p in activePersonnel)
                {
                    var pAttendance = attendance.Where(a => a.PersonnelId == p.Id).ToList();
                    var workDays = pAttendance.Count(a => a.Status == "work" || a.Status == "overtime");
                    var overtimeDays = pAttendance.Count(a => a.Status == "overtime");

                    // Simple Calculation Logic
                    decimal dayRate = p.BaseSalary / 30;
                    decimal currentBase = dayRate * workDays;

                    var payroll = new PayrollRecord
                    {
                        PersonnelId = p.Id,
                        Year = year,
                        Month = month,
                        BaseSalary = Math.Round(currentBase),
                        HousingAllowance = p.HousingAllowance,
                        FoodAllowance = p.FoodAllowance,
                        ChildAllowance = p.ChildAllowance,
                        OvertimeAmount = Math.Round(dayRate * 1.4m * overtimeDays), // 1.4x for overtime
                        OtherAllowances = p.OtherAllowance,

                        InsuranceDeduction = Math.Round((currentBase + p.HousingAllowance + p.FoodAllowance) * 0.07m), // 7% Employee share
                        TaxDeduction = currentBase > 100000000 ? Math.Round(currentBase * 0.1m) : 0, // Simplified tax
                        OtherDeductions = 0,

                        Status = "Calculated"
                    };

                    payroll.NetPay = (payroll.BaseSalary + payroll.HousingAllowance + payroll.FoodAllowance + payroll.ChildAllowance + payroll.OvertimeAmount + payroll.OtherAllowances)
                                   - (payroll.InsuranceDeduction + payroll.TaxDeduction + payroll.OtherDeductions);

                    newPayrolls.Add(payroll);
                }

                await _repository.UpdatePayrollRangeAsync(newPayrolls);
                await _repository.SaveChangesAsync();

                await GetPayrollList(year, month); // Refresh list for caller
                await Clients.Caller.SendAsync("PayrollCalculated");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating payroll");
                await Clients.Caller.SendAsync("ReceiveError", "خطا در محاسبه حقوق");
            }
        }
    }

    public class CreatePersonnelRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FatherName { get; set; } = string.Empty;
        public string NationalCode { get; set; } = string.Empty;
        public string IdNumber { get; set; } = string.Empty;
        public string BirthDate { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string MaritalStatus { get; set; } = string.Empty;
        public int ChildrenCount { get; set; }
        public string Mobile { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public string PersonnelCode { get; set; } = string.Empty;
        public string HireDate { get; set; } = string.Empty;
        public string LeaveDate { get; set; } = string.Empty;
        public string EmploymentType { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string CostCenter { get; set; } = string.Empty;
        public string WorkLocation { get; set; } = string.Empty;

        public string BankName { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string Iban { get; set; } = string.Empty;
        public string AccountCodeCurrent { get; set; } = string.Empty;
        public string AccountCodeLoan { get; set; } = string.Empty;
        public string AccountCodeAdvance { get; set; } = string.Empty;
        public decimal BaseSalary { get; set; }
        public decimal HousingAllowance { get; set; }
        public decimal FoodAllowance { get; set; }
        public decimal ChildAllowance { get; set; }
        public decimal OvertimeFixed { get; set; }
        public decimal OtherAllowance { get; set; }

        public string InsuranceNumber { get; set; } = string.Empty;
        public string InsuranceType { get; set; } = string.Empty;
        public string EmployeeInsurancePct { get; set; } = string.Empty;
        public string EmployerInsurancePct { get; set; } = string.Empty;
        public bool UnemploymentInsurance { get; set; }
        public string TaxExemptionType { get; set; } = string.Empty;
        public string TaxServiceLocation { get; set; } = string.Empty;
        public string EmployerEconomicCode { get; set; } = string.Empty;
        public string EmployerNationalId { get; set; } = string.Empty;
        public bool IncomeTaxable { get; set; }
        public bool IncludeInInsuranceList { get; set; }
    }
}
