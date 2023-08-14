using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using marauderserver.Models;

namespace marauderserver.Models
{
	public class Panel
	{
        public int PanelId { get; set; }

        public string? Title { get; set; }

        public int? XCoord { get; set; }

        public int? YCoord { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public string? UserId { get; set; }
        public User? User { get; set; }

        public int? DocFileId { get; set; }
        public DocFile? DocFile { get; set; }

        public ICollection<Note>? Notes { get; set; }
    }
}