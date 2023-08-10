using marauderserver.Hubs;
using marauderserver.Odyssey;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace marauderserver.Controllers
{
    public class OdysseyController : Controller
    {
        private readonly IPlayerState _playerState;
        private readonly IHubContext<OdysseyHub> _odysseyHub;

        public OdysseyController(IPlayerState playerState, IHubContext<OdysseyHub> odysseyHub)
        {
            _playerState = playerState;
            _odysseyHub = odysseyHub;
        }

        [HttpPost]
        public async Task<ActionResult> ProvideReading(int positionX, int positionY, int positionZ)
        {
            // Update our players state.
            _playerState.UpdatePosition(positionX, positionY, positionZ);

            // Write the sample to file (our sample writer) and update all clients
            // Wait for them both to finish.
            await Task.WhenAll(
              //sampleWriter.ProvideSample(_playerState.LastSample, _playerState.Speed, _playerState.Count),
              _odysseyHub.Clients.All.SendAsync("newData",
                                            _playerState.PositionX,
                                            _playerState.PositionY,
                                            _playerState.PositionZ)
            );

            return StatusCode(200);
        }
    }
}