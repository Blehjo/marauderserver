﻿using System.ComponentModel.DataAnnotations;

namespace nextjsarduinosolution.Models
{
    public class Favorite
    {
        public int FavoriteId { get; set; }

        public int ContentId { get; set; }

        public int UserId { get; set; }

        public User? User { get; set; }

        public string ContentType { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}