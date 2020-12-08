using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Paging.Filters;
using Domain.Wrappers;

namespace Application.Common.Interfaces
{
    public interface IRepository<TEntity, in TIdentifier> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        Task<TEntity> CreateRecordAsync(TEntity record, CancellationToken cancellationToken = default);
        Task<int> UpdateRecordAsync(TEntity record, CancellationToken cancellationToken = default);
        TEntity FindById(TIdentifier identifier);
        PagedResponse<IEnumerable<TEntity>> PaginationAsync(PaginationFilter filter);
    }
}