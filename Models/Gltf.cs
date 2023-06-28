namespace marauderserver.Models
{
	public class Gltf
	{
		public int GltfId { get; set; }

		public string FileInformation { get; set; }

		public int UserId { get; set; }

		public User? User { get; set; }
	}
}

