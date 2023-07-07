namespace marauderserver.Models
{
	public class ArtificialIntelligenceChat
	{
		public int ArtificialIntelligenceId { get; set; }

        public ArtificialIntelligence ArtificialIntelligence { get; set; }

        public int ChatId { get; set; }

		public Chat Chat { get; set; }
	}
}

