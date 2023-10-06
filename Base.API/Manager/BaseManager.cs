using Base.API.Interface.Manager;
using Base.API.Interface.Repository;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Base.API.Manager
{
    public class BaseManager<T> : IBaseManager<T> where T : class
    {
        private IBaseRepository<T> repository;
        public BaseManager(IBaseRepository<T> common)
        {
            repository = common;
        }


        public bool Add(T entity)
        {
            return repository.Add(entity);
        }

        public bool Add(ICollection<T> entity)
        {
            return repository.Add(entity);
        }

        public bool Delete(T entity)
        {
            return repository.Delete(entity);
        }

        public bool Delete(ICollection<T> entity)
        {
            return repository.Delete(entity);
        }

        public int ExecuteNonQuerySql(string sql)
        {
            return repository.ExecuteNonQuerySql(sql);
        }

        public DataTable ExecuteRawSql(string  sql)
        {
            return repository.ExecuteRawSql(sql);          
        }

        public ICollection<T> Get(System.Linq.Expressions.Expression<Func<T, bool>> predicate, params System.Linq.Expressions.Expression<Func<T, object>>[] includes)
        {
            return repository.Get(predicate, includes);
        }

        public ICollection<T> GetAll(params System.Linq.Expressions.Expression<Func<T, object>>[] include)
        {
            return repository.GetAll(include);
        }

        public T GetFirstOrDefault(System.Linq.Expressions.Expression<Func<T, bool>> predicate, params System.Linq.Expressions.Expression<Func<T, object>>[] includes)
        {
            return repository.GetFirstOrDefault(predicate, includes);
        }

        public bool Update(T entity)
        {
            return repository.Update(entity);
        }

        public bool Update(ICollection<T> entity)
        {
            return repository.Update(entity);
        }
    }
}
