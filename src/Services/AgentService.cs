using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Services;
using AutoMapper;
using Domain.Commands.AgentCommands;
using Domain.Entities;
using Domain.Paging.Filters;
using Domain.Wrappers;

namespace Services
{
    public class AgentService : IAgentService
    {
        private readonly IAgentRepository _agentRepository;
        private readonly IAgencyService _agencyService;
        private readonly IMapper _mapper;

        public AgentService(IAgentRepository agentRepository, IAgencyService agencyService, IMapper mapper)
        {
            _agentRepository = agentRepository;
            _agencyService = agencyService;
            _mapper = mapper;
        }

        public async Task<bool> AddAgentToAgency(AddAgentToAgencyCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            var agency = await _agencyService.GetById(command.AgencyId);
            if (agency == null) throw new NotFoundException<Guid>(nameof(Agency), command.AgencyId);
            var agent = await _agentRepository.GetById(command.AgentId);
            if (agent == null) throw new NotFoundException<Guid>(nameof(Agent), command.AgencyId);

            var affectedRecordsCount = await _agentRepository.AddToAgency(new AgencyAgent { AgenciesId = command.AgencyId, AgentsId = command.AgentId});

            return affectedRecordsCount >= 1;
        }

        public async Task<Agent> CreateAsync(CreateAgentCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            var agency = _mapper.Map<Agent>(command);
            var result = await _agentRepository.Create(agency);

            return result;
        }

        public async Task<Agent> GetById(Guid id)
        {
            if(id == Guid.Empty)
                throw new ArgumentException(nameof(id));
            var agent = await _agentRepository.GetById(id);
            if (agent == null)
                throw new NotFoundException<Guid>(nameof(Agent), id);

            return agent;
        }

        public async Task<PagedResponse<IEnumerable<Agent>>> FilterAsync(PaginationFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            return await _agentRepository.PaginationAsync(filter);
        }
    }
}