using System;
using System.Collections.Generic;
using Moq;
using Server.Data.Interfaces;

namespace Server.UnitTests.Lobby
{
    public class LobbyControllerTest
    {
        private readonly Mock<ILobbyRepository> _lobbyRepository;

        public LobbyControllerTest()
        {
            _lobbyRepository = new Mock<ILobbyRepository>(null, null, null);
            _lobbyRepository.Setup(x => x.GetAll()).Returns(
                new List<Data.Models.Lobby>() {
                    new Data.Models.Lobby() { DateCreated = DateTime.Now, Description = "Test1", HostId = 1, Id = 1, Name = "Test1" }
                });
        }
    }
}
