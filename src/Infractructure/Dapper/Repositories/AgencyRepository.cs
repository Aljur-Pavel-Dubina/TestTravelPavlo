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
    public class AgencyRepository : IAgencyRepository
    {
        private readonly ConnectionService _connectionService;

        public AgencyRepository(ConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        public async Task<Agency> Create(Agency agency)
        {
            var insertQuery = SQLScriptGenerator.GenerateInsertQuery<Agency>(MSSQLTablesNameConstants.AgenciesTableName);

            using (var connection = _connectionService.CreateConnection())
            {
                await connection.ExecuteAsync(insertQuery, agency);

                return agency;
            }
        }

        public async Task<Agency> GetById(Guid Id)
        {
            var GetByIdQuery = SQLScriptGenerator.GenereteGetByIdQuery(Id, MSSQLTablesNameConstants.AgenciesTableName);

            using (var connection = _connectionService.CreateConnection())
            {
                var queryResult = (await connection.QueryAsync<Agency>(GetByIdQuery)).ToList();
                if (queryResult.Any())
                {
                    return queryResult.First();
                }
                else
                {
                    throw new NotFoundException<Guid>(nameof(Agency), Id);
                }
            }
        }

        public async Task<PagedResponse<IEnumerable<Agency>>> PaginationAsync(PaginationFilter filter)
        {
            var offset = (filter.PageNumber - 1) * filter.PageSize;
            var totalEntriesQuery = SQLScriptGenerator.GenerateTotalCountQuery(MSSQLTablesNameConstants.AgenciesTableName);
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

                var queryResult = await connection.QueryAsync<Agency>(pagedQuery);
                return new PagedResponse<IEnumerable<Agency>>(queryResult, pagingModel);
            }
        }

        public async Task<int> Update(Agency agency)
        {
            var updateQuery = SQLScriptGenerator.GenerateUpdateQuery<Agency>(MSSQLTablesNameConstants.AgenciesTableName);

            using (var connection = _connectionService.CreateConnection())
            {
                return await connection.ExecuteAsync(updateQuery, agency);
            }
        }
    }
}
