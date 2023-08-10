namespace marauderserver.Odyssey;

public interface IPlayerState
{
    ICoordinates LastPosition { get; }
    int PositionX { get; }
    int PositionY { get; }
    int PositionZ { get; }

    void UpdatePosition(int PositionX, int PositionY, int PositionZ);
}