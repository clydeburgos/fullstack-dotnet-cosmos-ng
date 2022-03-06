using Standard.Customer.Domain;
using Standard.Customer.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Standard.Customer.Application
{
    public interface IRepository<T>
    {
        Task<T> Get(string id);
        Task<int> CountRecords();
        Task<IEnumerable<CustomerEntity>> GetMany(string searchKeyword, int? page, int pageSize, string orderByColumn, string orderBy);
        Task<string> Add(T entity, CreateType createType);
        Task<T> Update(T entity);
        Task<bool> Delete(string id);
    }
}
