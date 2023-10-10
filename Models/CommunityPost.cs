using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace marauderserver.Models
{
	public class CommunityPost
	{
        public int CommunityPostId { get; set; }

        public string? PostValue { get; set; }

        public string? MediaLink { get; set; }

        public string Type { get; set; } = "communityPost";

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        [NotMapped]
        public string? ImageSource { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public string? UserId { get; set; }

        public User? User { get; set; }

        public ICollection<CommunityComment>? CommunityComments { get; set; }

        public ICollection<Favorite>? Favorites { get; set; }
    }
}

