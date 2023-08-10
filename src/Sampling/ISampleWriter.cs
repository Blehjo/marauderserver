namespace marauderserver.Sampling
{
    public interface ISampleWriter
    {
        Task ProvideSample(DateTime timestamp, double speed, int count);

        void StartNewFile();
    }
}