using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Server.Data.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> Where(Expression<Func<T, bool>> predicate);
        T Find(int id);
        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        void Add(T entry);
        void AddRange(IEnumerable<T> entries);
        void Delete(T entry);
        void DeleteRange(IEnumerable<T> entries);
        void Update(T entry);
        void SaveChanges();
    }
}
