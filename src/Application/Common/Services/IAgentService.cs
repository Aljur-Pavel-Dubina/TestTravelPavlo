using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Commands.AgentCommands;
using Domain.Entities;
using Domain.Paging.Filters;
using Domain.Wrappers;

namespace Application.Common.Services
{
    public interface IAgentService
    {
        Task<Agent> CreateAsync(CreateAgentCommand command);
        Task<bool> AddAgentToAgency(AddAgentToAgencyCommand command);
        Task<Agent> GetById(Guid Id);
        Task<PagedResponse<IEnumerable<Agent>>> FilterAsync(PaginationFilter filter);
    }
}