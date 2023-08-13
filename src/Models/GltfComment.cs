using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace marauderserver.Models
{
	public class GltfComment
	{
        public int CommentId { get; set; }

        public string CommentValue { get; set; }

        public string? MediaLink { get; set; }

        public string Type { get; set; } = "gltfcomment";

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        [NotMapped]
        public string? ImageSource { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public string? UserId { get; set; }
        public User? User { get; set; }

        public int GltfId { get; set; }
        public Gltf? Gltf { get; set; }

        public ICollection<Favorite>? Favorites { get; set; }
    }
}

