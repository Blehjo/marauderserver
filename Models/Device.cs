namespace marauderserver.Models;

public class Device {

    public string DeviceId { get; set; } = Guid.NewGuid().ToString();

    public string DeviceName { get; set; }

    public int DeviceType { get; set; }

    public string? UserId { get; set;}
    public User? User { get; set; }

    public ICollection<Pin>? Pins { get; set; }
}

