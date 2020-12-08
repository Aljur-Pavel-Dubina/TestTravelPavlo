using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Services;
using AutoMapper;
using Domain.Commands.AgencyCommands;
using Domain.Entities;
using Domain.Paging.Filters;
using Domain.Wrappers;

namespace Services
{
    public class AgencyService : IAgencyService
    {
        private readonly IAgencyRepository _agencyRepository;
        private readonly IMapper _mapper;

        public AgencyService(IAgencyRepository agencyRepository,
            IMapper mapper)
        {
            _agencyRepository = agencyRepository;
            _mapper = mapper;
        }

        public async Task<Agency> CreateAsync(CreateAgencyCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            var agency = _mapper.Map<Agency>(command);
            var result = await _agencyRepository.Create(agency);
            return result;
        }

        public async Task<Agency> GetById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException(nameof(id));
            var agency = await _agencyRepository.GetById(id);
            if (agency == null)
                throw new NotFoundException<Guid>(nameof(Agency), id);

            return agency;
        }

        public async Task<PagedResponse<IEnumerable<Agency>>> FilterAsync(PaginationFilter filter)
        {
            if (filter == null)
            {
                throw  new ArgumentNullException(nameof(filter));
            }

            return await _agencyRepository.PaginationAsync(filter);
        }
    }
}