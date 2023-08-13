namespace marauderserver.Models
{
	public class Gltf
	{
		public int GltfId { get; set; }

		public string FileInformation { get; set; }

		public string? UserId { get; set; }

		public User? User { get; set; }

		public ICollection<Shape>? Shapes { get; set; }

		public ICollection<GltfComment>? GltfComments { get; set; }

		public ICollection<Favorite>? Favorites { get; set; }
	}
}

