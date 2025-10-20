using Finomal.Domain.Common;

namespace Finomal.Domain.Accounting
{
    public interface IAccountingRepository
    {
        /// <summary>
        /// دریافت یک سند حسابداری با شناسه آن به همراه آیتم‌هایش
        /// </summary>
        /// <param name="documentId">شناسه سند</param>
        /// <returns>موجودیت سند یا null در صورت عدم وجود</returns>
        Task<AccountingDocument?> GetDocumentByIdAsync(int documentId);

        /// <summary>
        /// دریافت لیستی از اسناد با شناسه‌های مشخص
        /// </summary>
        /// <param name="documentIds">لیستی از شناسه‌های اسناد</param>
        /// <returns>لیستی از اسناد حسابداری</returns>
        Task<List<AccountingDocument>> GetDocumentsByIdsAsync(IEnumerable<int> documentIds);

        /// <summary>
        /// دریافت تمام اسناد حسابداری به صورت صفحه‌بندی شده
        /// </summary>
        /// <param name="pageNumber">شماره صفحه</param>
        /// <param name="pageSize">تعداد آیتم در هر صفحه</param>
        /// <returns>یک نتیجه صفحه‌بندی شده از اسناد</returns>
        Task<PagedResult<AccountingDocument>> GetAllDocumentsAsync(int pageNumber, int pageSize);

        /// <summary>
        /// دریافت اسناد ایجاد شده توسط یک کاربر خاص
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns>لیستی از اسناد</returns>
        Task<List<AccountingDocument>> GetDocumentsByUserIdAsync(string userId);

        /// <summary>
        /// جستجوی اسناد در یک محدوده تاریخی خاص
        /// </summary>
        /// <param name="startDate">تاریخ شروع</param>
        /// <param name="endDate">تاریخ پایان</param>
        /// <returns>لیستی از اسناد در آن محدوده</returns>
        Task<List<AccountingDocument>> GetDocumentByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// افزودن یک سند حسابداری جدید
        /// </summary>
        /// <param name="document">موجودیت سند جدید</param>
        Task<AccountingDocument> AddDocumentAsync(AccountingDocument document);

        /// <summary>
        /// به‌روزرسانی یک سند حسابداری موجود
        /// </summary>
        /// <param name="document">موجودیت سند برای به‌روزرسانی</param>
        Task<AccountingDocument> UpdateDocumentAsync(AccountingDocument document);

        /// <summary>
        /// حذف یک سند حسابداری با شناسه آن (حذف نرم)
        /// </summary>
        /// <param name="documentId">شناسه سند برای حذف</param>
        Task<bool> DeleteDocumentAsync(int documentId);

        /// <summary>
        /// بررسی وجود یک سند با شناسه مشخص
        /// </summary>
        /// <param name="documentId">شناسه سند</param>
        /// <returns>True اگر وجود داشته باشد</returns>
        Task<bool> ExistsDocumentAsync(int documentId);
    }
}