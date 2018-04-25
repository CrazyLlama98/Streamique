using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Server.Data.DTOs;
using Server.Data.Interfaces;
using Server.Data.Models;
using Server.Services.Constants;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class UserService : GenericService<User, UserDto>, IUserService
    {
        public UserService(IUserRepository repository, IMapper mapper, ILogger<UserService> logger) 
            : base(repository, mapper, logger) { }

        public UserDto FindByEmail(string email)
        {
            try
            {
                var entity = ((IUserRepository)Repository).FindByEmail(email);
                return Mapper.Map<UserDto>(entity);
            }
            catch (Exception e)
            {
                Logger.LogError(LoggingEvents.CustomServiceEvents.UserLookupError, e, "Cannot find user with email = {EMAIL}", 
                    email);
                return null;
            }
        }
    }
}
