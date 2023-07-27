namespace marauderserver.Models
{
	public class Shape
	{
		public int ShapeId { get; set; }

		public string ShapeName { get; set; }

		public int? Height { get; set; }

		public int? Width { get; set; }

		public int? Depth { get; set; }

		public int? Radius { get; set; }

		public int? Length { get; set; }

		public string? Color { get; set; }

		public int GltfId { get; set; }

		public Gltf Gltf { get; set; }
	}
}

