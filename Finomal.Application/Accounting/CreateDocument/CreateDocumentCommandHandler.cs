using Finomal.Application.Abstractions;
using Finomal.Application.Accounting.Dtos;
using Finomal.Application.Users;
using Finomal.Domain.Abstractions;
using Finomal.Domain.Accounting;
using Mapster;
using MapsterMapper;
using System.ComponentModel.DataAnnotations;

namespace Finomal.Application.Accounting.CreateDocument
{
    public class CreateDocumentCommandHandler : ICommandHandler<CreateDocumentCommand, AccountingDocumentDto>
    {
        private readonly IAccountingRepository _accountingRepository;
        public CreateDocumentCommandHandler(IAccountingRepository accountingRepository)
        {
            _accountingRepository = accountingRepository;
        }
        public async Task<Result<AccountingDocumentDto>> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
        {
            try
            {
            var newAccountingDocument = request.Adapt<AccountingDocument>();
            var document = await _accountingRepository.AddDocumentAsync(newAccountingDocument);
            return document.Adapt<AccountingDocumentDto>();

            }

            catch (ValidationException valEx)
            {
                return Result.Fail<AccountingDocumentDto>($"error v {valEx.Data}  {valEx.Value}  req {request.ToString()}");
            }
            catch (Exception ex)
            {
                return Result.Fail<AccountingDocumentDto>($"error v {ex.Message}  req {request}");
            }
        }
    }
}
