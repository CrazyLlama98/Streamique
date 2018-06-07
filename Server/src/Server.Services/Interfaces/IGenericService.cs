using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Server.Services.Interfaces
{
    public interface IGenericService<T, TGetDto> where T : class where TGetDto : class
    {
        IEnumerable<TGetDto> GetAll();
        TGetDto Find(int id);
        IEnumerable<TGetDto> Where(Expression<Func<T, bool>> predicate);
        TGetDto FirstOrDefault(Expression<Func<T, bool>> predicate);
    }
}
