using Finomal.Application.Accounting.Dtos;
using Finomal.Domain.Accounting;
using Finomal.Domain.Common;

namespace Finomal.Application.Accounting
{
    public class AccountingService : IAccountingService
    {
        private readonly IAccountingRepository _repository;

        public AccountingService(IAccountingRepository repository)
        {
            _repository = repository;
        }

        public Task<int> CreateNewDocumentAsync(AccountingDocumentDto createDto, string creatorUserId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteDocumentAsync(int documentId)
        {
            throw new NotImplementedException();
        }

        public Task FinalizeDocumentAsync(int documentId, string finalizerUserId)
        {
            throw new NotImplementedException();
        }

        public Task<string> GenerateNewDocumentCodeAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AccountingDocumentDto?> GetDocumentDetailsAsync(int documentId)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<AccountingDocumentDto>> GetDocumentsAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task UpdateDocumentAsync(int documentId, AccountingDocumentDto updateDto, string editorUserId)
        {
            throw new NotImplementedException();
        }
    }
}
