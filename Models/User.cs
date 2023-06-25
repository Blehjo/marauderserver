using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace marauderserver.Models;

public class User
{
    public string UserId { get; set; } = Guid.NewGuid().ToString();

    public string? Username { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [DataType(DataType.EmailAddress)]
    public string? EmailAddress { get; set; }

    [JsonIgnore]
    public string? Password { get; set; }

    public string? About { get; set; }

    public string? ImageLink { get; set; }

    [NotMapped]
    public IFormFile? ImageFile { get; set; }

    [NotMapped]
    public string? ImageSource { get; set; }

    [DataType(DataType.Date)]
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    public ICollection<Device>? Devices { get; set; }

    public ICollection<Panel>? Panels { get; set; }

    public ICollection<Community>? Communities { get; set; }

    public ICollection<Chat>? Chats { get; set; }

    public ICollection<Post>? Posts { get; set; }

    public ICollection<Comment>? Comments { get; set; }

    public ICollection<Follower>? Followers { get; set; }

    public ICollection<Favorite>? Favorites { get; set; }
}

