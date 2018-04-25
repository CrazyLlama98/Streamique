using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Server.Data.Interfaces;
using Server.Services.Constants;
using Server.Services.Interfaces;

namespace Server.Services
{
    public abstract class GenericService<T, TDto> : IGenericService<T, TDto> 
        where T : class where TDto : class
    {
        protected IGenericRepository<T> Repository { get; set; }
        protected IMapper Mapper { get; set; }
        protected ILogger Logger { get; set; }

        public GenericService(IGenericRepository<T> repository, IMapper mapper, ILogger logger)
        {
            Repository = repository;
            Mapper = mapper;
            Logger = logger;
        }

        public TDto Find(int id)
        {
            try
            {
                var entity = Repository.Find(id);
                return Mapper.Map<TDto>(entity);
            }
            catch (Exception e)
            {
                Logger.LogError(LoggingEvents.GenericServiceEvents.LookupError, e, "Cannot find entity of type {TYPE} with id = {ID}", 
                    nameof(T), id);
                return null;
            }
        }

        public TDto FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var entity = Repository.FirstOrDefault(predicate);
                return entity != null ? Mapper.Map<TDto>(entity) : null;
            }
            catch (Exception e)
            {
                Logger.LogError(LoggingEvents.GenericServiceEvents.LookupError, e, "Cannot find entity of type {TYPE} using query", nameof(T));
                return null;
            }
        }

        public IEnumerable<TDto> GetAll()
        {
            try
            {
                var entities = Repository.GetAll();
                return Mapper.Map<IEnumerable<TDto>>(entities);
            }
            catch (Exception e)
            {
                Logger.LogError(LoggingEvents.GenericServiceEvents.LookupError, e, "Cannot fetch all entities of type {TYPE}", nameof(T));
                return null;
            }
        }

        public IEnumerable<TDto> Where(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var entities = Repository.Where(predicate);
                return Mapper.Map<IEnumerable<TDto>>(entities);
            }
            catch (Exception e)
            {
                Logger.LogError(LoggingEvents.GenericServiceEvents.LookupError, e, "Cannot fetch entities of type {TYPE} with query", nameof(T));
                return null;
            }
        }
    }
}
