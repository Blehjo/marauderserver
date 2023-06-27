using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace marauderserver.Models
{
	public class Note
	{
        public int NoteId { get; set; }

        public string? NoteValue { get; set; }

        public string? MediaLink { get; set; }

        public int? XCoord { get; set; }

        public int? YCoord { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        [NotMapped]
        public string? ImageSource { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public int PanelId { get; set; }
        public Panel? Panel { get; set; }
	}
}

