namespace marauderserver.Models
{
	public class Moveable
	{
		public int MoveableId { get; set; }

		public int XCoord { get; set; }

		public int YCoord { get; set; }

		public int ZCoord { get; set; }

		public int FileId { get; set; }

		public DocFile DocFile { get; set; }
	}
}

