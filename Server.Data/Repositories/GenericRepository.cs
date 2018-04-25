using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Server.Data.Interfaces;

namespace Server.Data.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected DbContext Context { get; set; }

        public GenericRepository(DbContext context)
        {
            Context = context;
        }

        public void Add(T entry)
        {
            Context.Set<T>().Add(entry);
        }

        public void AddRange(IEnumerable<T> entries)
        {
            Context.Set<T>().AddRange(entries);
        }

        public void Delete(T entry)
        {
            Context.Set<T>().Remove(entry);
        }

        public void DeleteRange(IEnumerable<T> entries)
        {
            Context.Set<T>().RemoveRange(entries);
        }

        public T Find(int id)
        {
            return Context.Set<T>().Find(id);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return GetSet().FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            return GetSet().AsEnumerable();
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        public void Update(T entry)
        {
            Context.Set<T>().Update(entry);
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return GetSet().Where(predicate).AsEnumerable();
        }

        protected virtual IQueryable<T> GetSet(bool useJoins = false)
        {
            return Context.Set<T>();
        }
    }
}
