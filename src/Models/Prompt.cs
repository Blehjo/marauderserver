namespace marauderserver.Models
{
    public class Prompt
    {
        public int PromptId { get; set; }
        public string Request { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
        public int ChatId { get; set; }
        public Chat? Chat { get; set; }
    }
}