namespace marauderserver.Models;

public class Device {
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string DeviceName { get; set; }

    public int DeviceType { get; set; }
}

