using Finomal.Application.Accounting.Dtos;
using Finomal.Domain.Common;
using System;

namespace Finomal.Application.Accounting
{
    public interface IAccountingService
    {
        /// <summary>
        /// ایجاد یک سند حسابداری جدید با آیتم‌های آن
        /// </summary>
        /// <param name="createDto">DTO حاوی اطلاعات سند جدید</param>
        /// <param name="creatorUserId">شناسه کاربری که سند را ایجاد می‌کند</param>
        /// <returns>شناسه سند ایجاد شده</returns>
        Task<int> CreateNewDocumentAsync(AccountingDocumentDto createDto, string creatorUserId);

        /// <summary>
        /// دریافت اطلاعات یک سند برای نمایش
        /// </summary>
        /// <param name="documentId">شناسه سند</param>
        /// <returns>DTO حاوی اطلاعات کامل سند</returns>
        Task<AccountingDocumentDto?> GetDocumentDetailsAsync(int documentId);

        /// <summary>
        /// دریافت لیست اسناد به صورت صفحه‌بندی شده
        /// </summary>
        Task<PagedResult<AccountingDocumentDto>> GetDocumentsAsync(int pageNumber, int pageSize);

        /// <summary>
        /// ویرایش یک سند حسابداری
        /// </summary>
        Task UpdateDocumentAsync(int documentId, AccountingDocumentDto updateDto, string editorUserId);

        /// <summary>
        /// حذف یک سند حسابداری
        /// </summary>
        /// <param name="documentId">شناسه سند برای حذف</param>
        Task DeleteDocumentAsync(int documentId);

        /// <summary>
        /// تولید یک شماره سند جدید و منحصر به فرد
        /// </summary>
        /// <returns>شماره سند پیشنهادی</returns>
        Task<string> GenerateNewDocumentCodeAsync();

        /// <summary>
        /// نهایی کردن یک سند (پس از آن قابل ویرایش نخواهد بود)
        /// </summary>
        Task FinalizeDocumentAsync(int documentId, string finalizerUserId);
    }
}
