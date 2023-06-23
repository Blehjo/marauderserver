namespace marauderserver.Models
{
	public class Pin
	{
		public int PinId { get; set; }

		public int PinLocation { get; set; }

        public bool IsAnalog { get; set; } = false;

		public int DeviceId { get; set; }

		public Device Device { get; set; }

        public ICollection<Action>? Actions { get; set; }
	}
}

