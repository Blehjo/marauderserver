namespace marauderserver.Models
{
	public class Action
	{
		public int ActionId { get; set; }

		public string EventType { get; set; } 

		public string? Activity { get; set; }

        public int PinId { get; set; }

        public Pin Pin { get; set; }
	}
}