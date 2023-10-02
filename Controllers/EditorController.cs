using System.Net;
using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using marauderserver.Data;
using marauderserver.Hubs;
using marauderserver.Models;
using Microsoft.AspNetCore.SignalR;

namespace marauderserver.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EditorController : ControllerBase
    {
        private new const int BadRequest = ((int)HttpStatusCode.BadRequest);

        private readonly MarauderContext _context;

        private readonly IWebHostEnvironment _hostEnvironment;

        private readonly ILogger<EditorController> _logger;

        private readonly IHubContext<EditorHub> _editorHub;

        public EditorController(MarauderContext context, IWebHostEnvironment hostEnvironment, ILogger<EditorController> logger, IHubContext<EditorHub> editorHub)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
            _editorHub = editorHub;
        }

        [Route("/ws/editor/{id}")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                _logger.Log(LogLevel.Information, "WebSocket connection established");
                await Nacho(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = BadRequest;
            }
        }

        [HttpPost("editor")]
        public async Task Post(Shape shape)
        {
            await _editorHub.Clients.All.SendAsync("shapeReceived", shape);
        }

        private async Task Echo(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            _logger.Log(LogLevel.Information, "Shape received from Client");

            while (!result.CloseStatus.HasValue)
            {
                var serverMsg = Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(buffer));
                List<string> listaDados = new List<string>();
                listaDados.AddRange(_context.Users.Select(user => user.Username));

                Encoding u8 = Encoding.UTF8;
                byte[] userLists = listaDados.SelectMany(x => u8.GetBytes(x)).ToArray();

                await webSocket.SendAsync(new ArraySegment<byte>(serverMsg, 0, serverMsg.Length), result.MessageType, result.EndOfMessage, CancellationToken.None);
                _logger.Log(LogLevel.Information, "Shape sent to Client");

                buffer = new byte[1024 * 4];
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                _logger.Log(LogLevel.Information, "Shape received from Client");
            }

            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            _logger.Log(LogLevel.Information, "WebSocket connection closed");
        }

        private static async Task Nacho(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            var receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!receiveResult.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(
                    new ArraySegment<byte>(buffer, 0, receiveResult.Count),
                    receiveResult.MessageType,
                    receiveResult.EndOfMessage,
                    CancellationToken.None);

                receiveResult = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await webSocket.CloseAsync(
                receiveResult.CloseStatus.Value,
                receiveResult.CloseStatusDescription,
                CancellationToken.None);
        }

        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "images", imageName);
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            return imageName;
        }

        [NonAction]
        public void DeleteImage(string imageName)
        {
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "images", imageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
        }
    }
}