using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Domain.Interfaces.Repositories
{
    public interface IRepositoryBase<T> where T : class
    {
        IQueryable<T> GetAll();
        T GetById(object id);
        IQueryable<T> GetByPredicate(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Remove(T entity);
        void Update(T entity);
        bool Exist(Expression<Func<T, bool>> predicate);
        void Save();
        void AddRange(IEnumerable<T> entities);
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
