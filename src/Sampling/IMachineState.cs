namespace marauderserver.Sampling
{
    public interface IMachineState
    {
        DateTime LastSample { get; }
        int Count { get; }
        double Speed { get; }

        void UpdateMachineState(uint sensorMilliseconds, double speed, int seatCount);

        void ZeroCount();
    }
}