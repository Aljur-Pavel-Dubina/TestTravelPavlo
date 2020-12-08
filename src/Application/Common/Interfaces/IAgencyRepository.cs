using Domain.Entities;
using Domain.Paging.Filters;
using Domain.Wrappers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IAgencyRepository
    {
        Task<Agency> Create(Agency agency);
        Task<Agency> GetById(Guid Id);
        Task<int> Update(Agency agency);
        Task<PagedResponse<IEnumerable<Agency>>> PaginationAsync(PaginationFilter filter);
    }
}
