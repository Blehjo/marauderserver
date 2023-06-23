namespace marauderserver.Models
{
	public class Action
	{
		public int ActionId { get; set; }

		public string Event { get; set; } = "";

		public string? Function { get; set; }

        public int PinId { get; set; }

        public Pin Pin { get; set; }
	}
}