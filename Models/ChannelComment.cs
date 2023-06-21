﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace marauderserver.Models
{
	public class ChannelComment
	{
        public int ChannelCommentId { get; set; }

        public string ChannelCommentValue { get; set; }

        public string? MediaLink { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        [NotMapped]
        public string? ImageSource { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public int ChannelId { get; set; }
        public Channel? Channel { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}

