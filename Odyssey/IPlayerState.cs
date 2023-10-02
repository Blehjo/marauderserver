namespace marauderserver.Odyssey;

public interface IPlayerState
{
    ICoordinates LastPosition { get; }
    int PositionX { get; }
    int PositionY { get; }
    int PositionZ { get; }

    Coordinates UpdatePosition(int PositionX, int PositionY, int PositionZ);
}