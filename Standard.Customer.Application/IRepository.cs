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
        Task<IEnumerable<CustomerEntity>> GetMany(string searchKeyword, int? page = 0, int pageSize = 20);
        Task<string> Add(T entity, CreateType createType);
        Task<T> Update(T entity);
        Task<bool> Delete(string id);
    }
}
