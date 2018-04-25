using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Data.Interfaces;
using Server.Data.Models;

namespace Server.Data.Repositories
{
    public class LobbyJoinRequestRepository : GenericRepository<LobbyJoinRequest>, ILobbyJoinRequestRepository
    {
        public LobbyJoinRequestRepository(VideoPanzerDbContext context) 
            : base(context)
        { }

        public IEnumerable<LobbyJoinRequest> GetLobbyJoinRequestsByLobbyId(int id)
        {
            return GetSet().Where(t => t.LobbyId == id).AsEnumerable();
        }

        public IEnumerable<LobbyJoinRequest> GetLobbyJoinRequestsByUserId(int id)
        {
            return GetSet().Where(t => t.UserId == id).AsEnumerable();
        }

        protected override IQueryable<LobbyJoinRequest> GetSet(bool useJoins = false)
        {
            return Context.Set<LobbyJoinRequest>().Include(t => t.User);
        }
    }
}
