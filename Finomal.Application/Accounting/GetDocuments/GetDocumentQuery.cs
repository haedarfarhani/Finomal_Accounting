using Finomal.Application.Abstractions;
using Finomal.Application.Accounting.Dtos;
using Finomal.Domain.Accounting;
using Finomal.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finomal.Application.Accounting.GetDocuments
{
    public class GetDocumentQuery: IQuery<PagedResult<AccountingDocumentDto>>
    {
       public int pageNumber { get; set; }
       public int pageSize { get; set; }
    }
}
