using Finomal.Domain.Abstractions;
using Finomal.Domain.Accounting;
using Finomal.Domain.Common;
using Finomal.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Finomal.Infrastructure.Data.Accounting
{
    public class AccountingRepository : IAccountingRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AccountingDocument?> GetDocumentByIdAsync(int documentId)
        {
            // با استفاده از Include، آیتم‌های سند نیز به صورت خودکار بارگذاری می‌شوند (Eager Loading)
            return await _context.AccountingDocuments
                .Include(d => d.Items)
                .Include(d => d.Branch) // بارگذاری اطلاعات شعبه
                .Include(d => d.Bank)   // بارگذاری اطلاعات بانک
                .FirstOrDefaultAsync(d => d.Id == documentId && !d.IsDeleted);
        }

        public async Task<List<AccountingDocument>> GetDocumentsByIdsAsync(IEnumerable<int> documentIds)
        {
            return await _context.AccountingDocuments
                .Include(d => d.Items)
                .Where(d => documentIds.Contains(d.Id) && !d.IsDeleted)
                .ToListAsync();
        }

        public async Task<PagedResult<AccountingDocument>> GetAllDocumentsAsync(int pageNumber, int pageSize)
        {
            var query = _context.AccountingDocuments.Where(d => !d.IsDeleted);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(d => d.Date)
                .ToListAsync();

            var result = new PagedResult<AccountingDocument>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            Console.WriteLine(items.Count);
            return result;
        }

        public async Task<List<AccountingDocument>> GetDocumentsByUserIdAsync(string userId)
        {
            return await _context.AccountingDocuments
                .Where(d => d.CreatedByUserId == userId && !d.IsDeleted)
                .OrderByDescending(d => d.Date)
                .ToListAsync();
        }

        public async Task<List<AccountingDocument>> GetDocumentByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.AccountingDocuments
                .Where(d => d.Date >= startDate && d.Date <= endDate && !d.IsDeleted)
                .OrderBy(d => d.Date)
                .ToListAsync();
        }

        public async Task<AccountingDocument> AddDocumentAsync(AccountingDocument document)
        {
            var result = _context.AccountingDocuments.Add(document);
            await _context.SaveChangesAsync();
            Console.WriteLine(result);

            return result.Entity;
        }

        /// <summary>
        /// یک سند حسابداری موجود را به‌روزرسانی کرده و پس از ذخیره تغییرات در پایگاه داده،
        /// موجودیت به‌روز شده را برمی‌گرداند.
        /// </summary>
        /// <param name="document">موجودیت سند که باید به‌روزرسانی شود.</param>
        /// <returns>موجودیت سند پس از اینکه تغییرات با موفقیت در پایگاه داده ذخیره شد.</returns>
        public async Task<AccountingDocument> UpdateDocumentAsync(AccountingDocument document)
        {
            // ۱. برچسب زمانی برای ردیابی آخرین زمان ویرایش را تنظیم می‌کند.
            document.UpdatedAt = DateTime.UtcNow;

            // ۲. به Entity Framework Core اعلام می‌کند که این موجودیت (و تمام آیتم‌های مرتبط با آن)
            // تغییر کرده‌اند. این کار وضعیت موجودیت را در Change Tracker به 'Modified' تغییر می‌دهد.
            _context.AccountingDocuments.Update(document);

            // ۳. تغییرات را به صورت غیرهمزمان در پایگاه داده ذخیره می‌کند.
            // این همان نقطه‌ای است که دستورات SQL UPDATE به دیتابیس ارسال می‌شود.
            await _context.SaveChangesAsync();

            // ۴. پس از ذخیره موفق، خود موجودیت 'document' را که اکنون وضعیت آن با پایگاه داده
            // همگام‌سازی شده است، برمی‌گرداند.
            return document;
        }

        /// <summary>
        /// یک سند را به صورت نرم (Soft Delete) حذف می‌کند.
        /// </summary>
        /// <param name="documentId">شناسه سند برای حذف</param>
        /// <returns>اگر سند پیدا و با موفقیت حذف شد true، در غیر این صورت false برمی‌گرداند.</returns>
        public async Task<bool> DeleteDocumentAsync(int documentId)
        {
            // سند مورد نظر را با استفاده از کلید اصلی پیدا می‌کند.
            var document = await _context.AccountingDocuments.FindAsync(documentId);

            // اگر سند وجود نداشت، عملیات ناموفق بوده است.
            if (document == null)
            {
                return false;
            }

            // پیاده‌سازی حذف نرم (Soft Delete)
            // فرض بر این است که مدل شما پراپرتی IsDeleted را دارد.
            document.IsDeleted = true;
            document.UpdatedAt = DateTime.UtcNow;

            // نیازی به فراخوانی .Update() نیست، چون FindAsync موجودیت را ردیابی می‌کند
            // و EF Core تغییرات اعمال شده روی آن را تشخیص می‌دهد.

            // تغییرات را در پایگاه داده ذخیره می‌کند.
            await _context.SaveChangesAsync();

            // عملیات با موفقیت انجام شد.
            return true;
        }

        public async Task<bool> ExistsDocumentAsync(int documentId)
        {
            return await _context.AccountingDocuments.AnyAsync(d => d.Id == documentId && !d.IsDeleted);
        }
    }
}
