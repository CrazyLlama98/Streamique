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

        public virtual void Add(T entry)
        {
            Context.Set<T>().Add(entry);
        }

        public virtual void AddRange(IEnumerable<T> entries)
        {
            Context.Set<T>().AddRange(entries);
        }

        public virtual void Delete(T entry)
        {
            Context.Set<T>().Remove(entry);
        }

        public virtual void DeleteRange(IEnumerable<T> entries)
        {
            Context.Set<T>().RemoveRange(entries);
        }

        public virtual T Find(int id)
        {
            return Context.Set<T>().Find(id);
        }

        public virtual T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return GetSet().FirstOrDefault();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return GetSet().AsNoTracking().AsEnumerable();
        }

        public virtual void SaveChanges()
        { 
            Context.SaveChanges();
        }

        public virtual void Update(T entry)
        {
            Context.Set<T>().Update(entry);
        }

        public virtual IEnumerable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return GetSet().AsNoTracking().Where(predicate).AsEnumerable();
        }

        protected virtual IQueryable<T> GetSet(bool useJoins = false)
        {
            return Context.Set<T>();
        }
    }
}
