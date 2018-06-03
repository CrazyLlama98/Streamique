using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Server.EventBus;
using Server.EventBus.Events;
using Server.EventBus.Interfaces;
using Server.Services.IntegrationEvents.LobbyEvents;
using Server.Services.IntegrationEvents.LobbyJoinRequestsEvents;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("api/[controller]"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PollUpdatesController : Controller
    {
        private readonly IEventBus _eventBus;
        private List<IntegrationEvent> events = new List<IntegrationEvent>();
        private IntegrationEventHandler<IntegrationEvent> _eventHandler;
        private readonly int _waitingInterval;

        public PollUpdatesController(IEventBus eventBus, IConfiguration configuration)
        {
            _eventBus = eventBus;
            _eventHandler = new IntegrationEventHandler<IntegrationEvent>(ev => events.Add(ev));
            if (!int.TryParse(configuration["WaitingInterval"], out _waitingInterval))
                _waitingInterval = 1000;
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            _eventBus.Subscribe<CreateLobbyEvent, IntegrationEventHandler<IntegrationEvent>>(_eventHandler);
            _eventBus.Subscribe<DeleteLobbyEvent, IntegrationEventHandler<IntegrationEvent>>(_eventHandler);
            _eventBus.Subscribe<UpdateLobbyEvent, IntegrationEventHandler<IntegrationEvent>>(_eventHandler);
            _eventBus.Subscribe<CreateJoinRequestEvent, IntegrationEventHandler<IntegrationEvent>>(_eventHandler);
            _eventBus.Subscribe<AcceptJoinRequestEvent, IntegrationEventHandler<IntegrationEvent>>(_eventHandler);
            _eventBus.Subscribe<DeleteJoinRequestEvent, IntegrationEventHandler<IntegrationEvent>>(_eventHandler);
        }

        private void UnsubscribeEvents()
        {
            _eventBus.Unsubscribe<CreateLobbyEvent, IntegrationEventHandler<IntegrationEvent>>(_eventHandler);
            _eventBus.Unsubscribe<DeleteLobbyEvent, IntegrationEventHandler<IntegrationEvent>>(_eventHandler);
            _eventBus.Unsubscribe<UpdateLobbyEvent, IntegrationEventHandler<IntegrationEvent>>(_eventHandler);
            _eventBus.Unsubscribe<CreateJoinRequestEvent, IntegrationEventHandler<IntegrationEvent>>(_eventHandler);
            _eventBus.Unsubscribe<AcceptJoinRequestEvent, IntegrationEventHandler<IntegrationEvent>>(_eventHandler);
            _eventBus.Unsubscribe<DeleteJoinRequestEvent, IntegrationEventHandler<IntegrationEvent>>(_eventHandler);
        }

        [HttpGet]
        public IActionResult Get()
        {
            while (true)
            {
                if (events != null && events.Count != 0)
                {
                    UnsubscribeEvents();
                    return Ok(events);
                }
                else
                    Thread.Sleep(_waitingInterval);
            }
        }
    }
}
