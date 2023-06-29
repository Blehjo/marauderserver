using AutoFixture;
using marauderserver.Models;

namespace marauderserver.Data
{
    public static class Seeder
    {
        public static void Seed(this MarauderContext marauderContext)
        {
            if (!marauderContext.Users.Any())
            {
                Fixture fixture = new Fixture();
                fixture.Customize<User>(user => user.Without(u => u.UserId));
                //--- The next two lines add 100 rows to your database
                List<User> users = fixture.CreateMany<User>(100).ToList();
                marauderContext.AddRange(users);
                marauderContext.SaveChanges();
            }

            if (!marauderContext.Actions.Any())
            {
                Fixture fixture = new Fixture();
                fixture.Customize<Models.Action>(action => action.Without(a => a.ActionId));
                //--- The next two lines add 100 rows to your database
                List<Models.Action> actions = fixture.CreateMany<Models.Action>(100).ToList();
                marauderContext.AddRange(actions);
                marauderContext.SaveChanges();
            }

            if (!marauderContext.ArtificialIntelligences.Any())
            {
                Fixture fixture = new Fixture();
                fixture.Customize<ArtificialIntelligence>(artificialIntelligence => artificialIntelligence.Without(a => a.ArtificialIntelligenceId));
                //--- The next two lines add 100 rows to your database
                List<ArtificialIntelligence> artificialIntelligences = fixture.CreateMany<ArtificialIntelligence>(100).ToList();
                marauderContext.AddRange(artificialIntelligences);
                marauderContext.SaveChanges();
            }

            if (!marauderContext.Channels.Any())
            {
                Fixture fixture = new Fixture();
                fixture.Customize<Channel>(channel => channel.Without(a => a.ChannelId));
                //--- The next two lines add 100 rows to your database
                List<Channel> channels = fixture.CreateMany<Channel>(100).ToList();
                marauderContext.AddRange(channels);
                marauderContext.SaveChanges();
            }

            if (!marauderContext.ChannelComments.Any())
            {
                Fixture fixture = new Fixture();
                fixture.Customize<ChannelComment>(channelcomment => channelcomment.Without(a => a.ChannelCommentId));
                //--- The next two lines add 100 rows to your database
                List<ChannelComment> channelcomments = fixture.CreateMany<ChannelComment>(100).ToList();
                marauderContext.AddRange(channelcomments);
                marauderContext.SaveChanges();
            }

            if (!marauderContext.Chats.Any())
            {
                Fixture fixture = new Fixture();
                fixture.Customize<Chat>(chat => chat.Without(a => a.ChatId));
                //--- The next two lines add 100 rows to your database
                List<Chat> chats = fixture.CreateMany<Chat>(100).ToList();
                marauderContext.AddRange(chats);
                marauderContext.SaveChanges();
            }

            if (!marauderContext.ChatComments.Any())
            {
                Fixture fixture = new Fixture();
                fixture.Customize<ChatComment>(chatcomment => chatcomment.Without(a => a.ChatCommentId));
                //--- The next two lines add 100 rows to your database
                List<ChatComment> chatcomments = fixture.CreateMany<ChatComment>(100).ToList();
                marauderContext.AddRange(chatcomments);
                marauderContext.SaveChanges();
            }

            if (!marauderContext.Comments.Any())
            {
                Fixture fixture = new Fixture();
                fixture.Customize<Comment>(comment => comment.Without(a => a.CommentId));
                //--- The next two lines add 100 rows to your database
                List<Comment> comments = fixture.CreateMany<Comment>(100).ToList();
                marauderContext.AddRange(comments);
                marauderContext.SaveChanges();
            }

            if (!marauderContext.Communities.Any())
            {
                Fixture fixture = new Fixture();
                fixture.Customize<Community>(community => community.Without(a => a.CommunityId));
                //--- The next two lines add 100 rows to your database
                List<Community> communities = fixture.CreateMany<Community>(100).ToList();
                marauderContext.AddRange(communities);
                marauderContext.SaveChanges();
            }

            if (!marauderContext.Devices.Any())
            {
                Fixture fixture = new Fixture();
                fixture.Customize<Device>(device => device.Without(a => a.DeviceId));
                //--- The next two lines add 100 rows to your database
                List<Device> devices = fixture.CreateMany<Device>(100).ToList();
                marauderContext.AddRange(devices);
                marauderContext.SaveChanges();
            }

            if (!marauderContext.DocFiles.Any())
            {
                Fixture fixture = new Fixture();
                fixture.Customize<DocFile>(docFile => docFile.Without(a => a.DocFileId));
                //--- The next two lines add 100 rows to your database
                List<DocFile> docFiles = fixture.CreateMany<DocFile>(100).ToList();
                marauderContext.AddRange(docFiles);
                marauderContext.SaveChanges();
            }

            if (!marauderContext.Favorites.Any())
            {
                Fixture fixture = new Fixture();
                fixture.Customize<Favorite>(favorite => favorite.Without(a => a.FavoriteId));
                //--- The next two lines add 100 rows to your database
                List<Favorite> favorites = fixture.CreateMany<Favorite>(100).ToList();
                marauderContext.AddRange(favorites);
                marauderContext.SaveChanges();
            }

            if (!marauderContext.Followers.Any())
            {
                Fixture fixture = new Fixture();
                fixture.Customize<Follower>(follower => follower.Without(a => a.FollowerId));
                //--- The next two lines add 100 rows to your database
                List<Follower> followers = fixture.CreateMany<Follower>(100).ToList();
                marauderContext.AddRange(followers);
                marauderContext.SaveChanges();
            }

            if (!marauderContext.Gltfs.Any())
            {
                Fixture fixture = new Fixture();
                fixture.Customize<Gltf>(gltf => gltf.Without(a => a.GltfId));
                //--- The next two lines add 100 rows to your database
                List<Gltf> gltfs = fixture.CreateMany<Gltf>(100).ToList();
                marauderContext.AddRange(gltfs);
                marauderContext.SaveChanges();
            }

            if (!marauderContext.Members.Any())
            {
                Fixture fixture = new Fixture();
                fixture.Customize<Member>(member => member.Without(a => a.MemberId));
                //--- The next two lines add 100 rows to your database
                List<Member> members = fixture.CreateMany<Member>(100).ToList();
                marauderContext.AddRange(members);
                marauderContext.SaveChanges();
            }

            if (!marauderContext.Messages.Any())
            {
                Fixture fixture = new Fixture();
                fixture.Customize<Message>(message => message.Without(a => a.MessageId));
                //--- The next two lines add 100 rows to your database
                List<Message> messages = fixture.CreateMany<Message>(100).ToList();
                marauderContext.AddRange(messages);
                marauderContext.SaveChanges();
            }

            if (!marauderContext.MessageComments.Any())
            {
                Fixture fixture = new Fixture();
                fixture.Customize<MessageComment>(messagecomments => messagecomments.Without(a => a.MessageCommentId));
                //--- The next two lines add 100 rows to your database
                List<MessageComment> messageComments = fixture.CreateMany<MessageComment>(100).ToList();
                marauderContext.AddRange(messageComments);
                marauderContext.SaveChanges();
            }

            if (!marauderContext.Moveables.Any())
            {
                Fixture fixture = new Fixture();
                fixture.Customize<Moveable>(moveables => moveables.Without(a => a.MoveableId));
                //--- The next two lines add 100 rows to your database
                List<Moveable> moveables = fixture.CreateMany<Moveable>(100).ToList();
                marauderContext.AddRange(moveables);
                marauderContext.SaveChanges();
            }

            if (!marauderContext.Notes.Any())
            {
                Fixture fixture = new Fixture();
                fixture.Customize<Note>(note => note.Without(a => a.NoteId));
                //--- The next two lines add 100 rows to your database
                List<Note> notes = fixture.CreateMany<Note>(100).ToList();
                marauderContext.AddRange(notes);
                marauderContext.SaveChanges();
            }

            if (!marauderContext.Panels.Any())
            {
                Fixture fixture = new Fixture();
                fixture.Customize<Panel>(panel => panel.Without(a => a.PanelId));
                //--- The next two lines add 100 rows to your database
                List<Panel> panels = fixture.CreateMany<Panel>(100).ToList();
                marauderContext.AddRange(panels);
                marauderContext.SaveChanges();
            }

            if (!marauderContext.Pins.Any())
            {
                Fixture fixture = new Fixture();
                fixture.Customize<Pin>(pin => pin.Without(a => a.PinId));
                //--- The next two lines add 100 rows to your database
                List<Pin> pins = fixture.CreateMany<Pin>(100).ToList();
                marauderContext.AddRange(pins);
                marauderContext.SaveChanges();
            }

            if (!marauderContext.Posts.Any())
            {
                Fixture fixture = new Fixture();
                fixture.Customize<Post>(post => post.Without(a => a.PostId));
                //--- The next two lines add 100 rows to your database
                List<Post> posts = fixture.CreateMany<Post>(100).ToList();
                marauderContext.AddRange(posts);
                marauderContext.SaveChanges();
            }
        }
    }
}
