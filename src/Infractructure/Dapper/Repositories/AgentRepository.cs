using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Dapper;
using Domain.Entities;
using Domain.Paging;
using Domain.Paging.Filters;
using Domain.Wrappers;
using Infractructure.Dapper.SQL.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infractructure.Dapper.Repositories
{
    public class AgentRepository : IAgentRepository
    {
        private readonly ConnectionService _connectionService;

        public AgentRepository(ConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        public async Task<int> AddToAgency(AgencyAgent agencyAgent)
        {
            var insertQuery = SQLScriptGenerator.GenerateInsertQuery<AgencyAgent>(MSSQLTablesNameConstants.AgentsToAgenciesTableName);
            using (var connection = _connectionService.CreateConnection())
            {
                return await connection.ExecuteAsync(insertQuery, agencyAgent);
            }
        }

        public async Task<Agent> Create(Agent agency)
        {
            var insertQuery = SQLScriptGenerator.GenerateInsertQuery<Agent>(MSSQLTablesNameConstants.AgentsTableName);

            using (var connection = _connectionService.CreateConnection())
            {
                await connection.ExecuteAsync(insertQuery, agency);

                return agency;
            }
        }

        public async Task<Agent> GetById(Guid Id)
        {
            var GetByIdQuery = SQLScriptGenerator.GenereteGetByIdQuery(Id, MSSQLTablesNameConstants.AgentsTableName);

            using (var connection = _connectionService.CreateConnection())
            {
                var queryResult = (await connection.QueryAsync<Agent>(GetByIdQuery)).ToList();
                if (queryResult.Any())
                {
                    return queryResult.First();
                }
                else
                {
                    throw new NotFoundException<Guid>(nameof(Agent), Id);
                }
            }
        }

        public async Task<PagedResponse<IEnumerable<Agent>>> PaginationAsync(PaginationFilter filter)
        {
            var offset = (filter.PageNumber - 1) * filter.PageSize;
            var totalEntriesQuery = SQLScriptGenerator.GenerateTotalCountQuery(MSSQLTablesNameConstants.AgentsTableName);
            var pagedQuery = SQLScriptGenerator.GeneratePagedScript(offset, filter.PageSize, MSSQLTablesNameConstants.AgenciesTableName);
            using (var connection = _connectionService.CreateConnection())
            {
                var totalEntries = await connection.QueryFirstAsync<int>(totalEntriesQuery);
                var pagingModel = new PagingModel()
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    TotalPages = (int)Math.Ceiling((double)totalEntries / filter.PageSize),
                    TotalEntries = totalEntries
                };

                var queryResult = await connection.QueryAsync<Agent>(pagedQuery);
                return new PagedResponse<IEnumerable<Agent>>(queryResult, pagingModel);
            }
        }

        public async Task<int> Update(Agent agent)
        {
            var updateQuery = SQLScriptGenerator.GenerateUpdateQuery<Agent>(MSSQLTablesNameConstants.AgentsTableName);

            using (var connection = _connectionService.CreateConnection())
            {
                return await connection.ExecuteAsync(updateQuery, agent);
            }
        }
    }
}