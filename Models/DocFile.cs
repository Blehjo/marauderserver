using System;
namespace marauderserver.Models
{
	public class DocFile
	{
		public int DocFileId { get; set; }

		public string Title { get; set; }

		public ICollection<Moveable> Moveables { get; set; }
	}
}

