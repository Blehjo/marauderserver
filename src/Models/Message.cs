using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace marauderserver.Models
{
	public class Message
	{
        public int MessageId { get; set; }

        public string? MessageValue { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public string? UserId { get; set; }
        public User? User { get; set; }

        public string? ReceiverId { get; set; }
        [NotMapped]
        public User? Receiver { get; set; } 

        public ICollection<MessageComment>? MessageComments { get; set; }
    }
}

