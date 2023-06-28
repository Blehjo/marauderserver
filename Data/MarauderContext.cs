using marauderserver.Models;
using Microsoft.EntityFrameworkCore;

namespace marauderserver.Data
{
	public class MarauderContext : DbContext
	{
		public MarauderContext(DbContextOptions<MarauderContext> dbContext) : base(dbContext)
		{
		}

        public DbSet<Models.Action> Actions { get; set; } = default!;

        public DbSet<ArtificialIntelligence> ArtificialIntelligences { get; set; } = default!;

        public DbSet<Channel> Channels { get; set; } = default!;

        public DbSet<ChannelComment> ChannelComments { get; set; } = default!;

        public DbSet<Chat> Chats { get; set; } = default!;

        public DbSet<ChatComment> ChatComments { get; set; } = default!;

        public DbSet<Comment> Comments { get; set; } = default!;

        public DbSet<Community> Communities { get; set; } = default!;

        public DbSet<Device> Devices { get; set; } = default!;

        public DbSet<DocFile> DocFiles { get; set; } = default!;

        public DbSet<Favorite> Favorites { get; set; } = default!;

        public DbSet<Follower> Followers { get; set; } = default!;

        public DbSet<Gltf> Gltfs { get; set; } = default!;

        public DbSet<Message> Messages { get; set; } = default!;

        public DbSet<MessageComment> MessageComments { get; set; } = default!;

        public DbSet<Member> Members { get; set; } = default!;

        public DbSet<Moveable> Moveables { get; set; } = default!;

        public DbSet<Note> Notes { get; set; } = default!;

        public DbSet<Panel> Panels { get; set; } = default!;

        public DbSet<Pin> Pins { get; set; } = default!;

        public DbSet<Post> Posts { get; set; } = default!;

        public DbSet<User> Users { get; set; } = default!;
    }
}