using Microsoft.AspNetCore.SignalR;
using marauderserver.Odyssey;

namespace marauderserver.Hubs;

public class OdysseyHub : Hub
{
	private readonly IPlayerState _playerState;
	
	public OdysseyHub(IPlayerState playerState)
	{
		_playerState = playerState;
	}
}