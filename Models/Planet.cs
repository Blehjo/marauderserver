﻿
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nextjsarduinosolution.Models
{
	public class Planet
	{
		public int PlanetId { get; set; }

		public string? PlanetName { get; set; }

		public string? PlanetMass { get; set; }

		public string? Perihelion { get; set; }

		public string? Aphelion { get; set; }

		public string? Gravity { get; set; }

		public string? Temperature { get; set; }

		public string? Brief { get; set; }

		public string? Description { get; set; }

		public string? ModelLink { get; set; }

		public string Type { get; set; } = "planet";

		public string? ImageLink { get; set; }

		public int? UserId { get; set; }

		public User? User { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        [NotMapped]
        public string? ImageSource { get; set; }

        public ICollection<Moon>? Moons { get; set; }

		public ICollection<Favorite>? Favorites { get; set; }

		public ICollection<PlanetComment>? PlanetComments { get; set; }
    }
}

