using Domain.Entities;
using Domain.Paging.Filters;
using Domain.Wrappers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IAgentRepository
    {
        Task<Agent> Create(Agent agent);
        Task<Agent> GetById(Guid Id);
        Task<int> Update(Agent agent);
        Task<int> AddToAgency(AgencyAgent agencyAgent);
        Task<PagedResponse<IEnumerable<Agent>>> PaginationAsync(PaginationFilter filter);
    }
}
