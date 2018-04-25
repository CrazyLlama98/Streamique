using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Data.Interfaces;
using Server.Data.Models;

namespace Server.Data.Repositories
{
    public class LobbyRepository : GenericRepository<Lobby>, ILobbyRepository
    {
        public LobbyRepository(VideoPanzerDbContext context)
            : base(context)
        { }

        public IEnumerable<Lobby> GetLobbiesByHostId(int hostId, bool includeRequests = false)
        {
            return GetSet(includeRequests).Where(t => t.HostId == hostId).AsEnumerable();
        }

        public IEnumerable<Lobby> GetLobbiesWithJoinRequests()
        {
            return GetSet(true).AsEnumerable();
        }

        public Lobby GetLobbyWithJoinRequestById(int id)
        {
            return GetSet(true).FirstOrDefault();
        }

        protected override IQueryable<Lobby> GetSet(bool useJoins = false)
        {
            if (!useJoins)
                return Context.Set<Lobby>().Include(t => t.Host);
            return Context.Set<Lobby>().Include(t => t.Host).Include(t => t.JoinRequests);
        }
    }
}
