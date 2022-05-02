using Locadora.Domain.Interfaces.Repositories;
using Locadora.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Repository.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly LocadoraContext _context;
        public RepositoryBase(LocadoraContext context)
        {
            _context = context;
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public T GetById(object id)
        {
            return _context.Set<T>().Find(id);
        }

        public IQueryable<T> GetByPredicate(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public void Add(T entity)
        {
            _context.Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _context.AddRange(entities);
        }

        public void Remove(T entity)
        {
            _context.Remove(entity);
        }
        public void Update(T entity)
        {
            _context.Update(entity);
        }

        public bool Exist(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Any(predicate);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void BeginTransaction()
        {
            _context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _context.Database.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            _context.Database.RollbackTransaction();
        }
    }
}
