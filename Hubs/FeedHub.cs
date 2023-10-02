using Microsoft.AspNetCore.SignalR;
using marauderserver.Sampling;

namespace marauderserver.Hubs;

public class FeedHub : Hub
{
    private readonly IMachineState machineState;
    private readonly ISampleWriter sampleWriter;

    public FeedHub(IMachineState machineState, ISampleWriter sampleWriter)
    {
        this.machineState = machineState;
        this.sampleWriter = sampleWriter;
    }

    public void ResetCount()
    {
        // Reset the state, and start a new data file
        machineState.ZeroCount();
        sampleWriter.StartNewFile();
    }
}