using Finomal.Application.Abstractions;
using Finomal.Application.Accounting.Dtos;
using Finomal.Application.Users;
using Finomal.Domain.Abstractions;
using Finomal.Domain.Accounting;
using Finomal.Domain.Common;
using Finomal.Domain.Users;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finomal.Application.Accounting.GetDocuments
{
    public class GetDocumentsQueryHandler : IQueryHandler<GetDocumentQuery, PagedResult<AccountingDocumentDto>>
    {
        private readonly IAccountingRepository _accountingRepository;
        public GetDocumentsQueryHandler(IAccountingRepository accountingRepository)
        {
            _accountingRepository = accountingRepository;
            
        }
        public async Task<Result<PagedResult<AccountingDocumentDto>>> Handle(GetDocumentQuery request, CancellationToken cancellationToken)
        {
            var documents = await _accountingRepository.GetAllDocumentsAsync(pageNumber : request.pageNumber, pageSize:request.pageSize);
            return documents.Adapt<PagedResult<AccountingDocumentDto>>();
        }
    }
}
