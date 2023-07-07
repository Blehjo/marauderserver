using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace marauderserver.Models
{
	public class ArtificialIntelligence
	{
        public int ArtificialIntelligenceId { get; set; }

        public string? Name { get; set; }

        public string? Role { get; set; }

        public string? ImageLink { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        [NotMapped]
        public string? ImageSource { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public string? UserId { get; set; }

        public User? User { get; set; }

        public ICollection<ArtificialIntelligenceChat>? Chats { get; set; }

        public ICollection<ChatComment>? ChatComments { get; set; }
    }
}

