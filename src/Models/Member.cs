using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace marauderserver.Models
{
	public class Member
	{
        public int MemberId { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public int CommunityId { get; set; }
        public Community? Community { get; set; }

        public string? UserId { get; set; }
        public User? User { get; set; }
	}
}

