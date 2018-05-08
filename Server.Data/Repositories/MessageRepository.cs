using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Data.Interfaces;
using Server.Data.Models;

namespace Server.Data.Repositories
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(VideoPanzerDbContext context) 
            : base(context)
        { }

        public IEnumerable<Message> GetMessagesByLobbyId(int lobbyId, int? skip = null, int? take = null)
        {
            var query = GetSet().Where(t => t.LobbyId == lobbyId);
            if (skip.HasValue && take.HasValue)
                query = query.Skip(skip.Value).Take(take.Value);
            return query.AsEnumerable();
        }

        public IEnumerable<Message> GetMessagesByUserId(int userId, int? skip = null, int? take = null)
        {
            var query = GetSet().Where(t => t.SenderId == userId);
            if (skip.HasValue && take.HasValue)
                query = query.Skip(skip.Value).Take(take.Value);
            return query.AsEnumerable();
        }

        protected override IQueryable<Message> GetSet(bool useJoins = false)
        {
            return Context.Set<Message>().Include(t => t.Sender);
        }
    }
}
