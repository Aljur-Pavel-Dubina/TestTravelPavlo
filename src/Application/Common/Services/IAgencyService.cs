using System;
using System.Collections.Generic;
using Domain.Entities;
using System.Threading.Tasks;
using Domain.Commands.AgencyCommands;
using Domain.Paging.Filters;
using Domain.Wrappers;

namespace Application.Common.Services
{
    public interface IAgencyService
    {
        Task<Agency> CreateAsync(CreateAgencyCommand command);
        Task<Agency> GetById(Guid Id);
        Task<PagedResponse<IEnumerable<Agency>>> FilterAsync(PaginationFilter filter);
    }
}