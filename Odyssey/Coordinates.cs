namespace marauderserver.Odyssey
{
	public class Coordinates
	{
		public ICoordinates _coordinates;

		public Coordinates(int positionX, int positionY, int positionZ)
		{
			_coordinates.PositionX = positionX;
			_coordinates.PositionY = positionY;
			_coordinates.PositionZ = positionZ;
		}
	}
}

