using AutoMapper;
using marauderserver.Authorization;
using marauderserver.Helpers;
using marauderserver.Models;
using marauderserver.Models.Users;
using marauderserver.Data;
using Microsoft.AspNetCore.Mvc;

namespace marauderserver.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();
        User GetById(string id);
        void Register(RegisterRequest model);
        void Update(string id, UpdateRequest model);
        void Delete(string id);
    }

    public class UserService : IUserService
    {
        private MarauderContext _context;
        private IJwtUtils _jwtUtils;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;

        public UserService(
            MarauderContext context,
            IJwtUtils jwtUtils,
            IMapper mapper,
            IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _jwtUtils = jwtUtils;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _context.Users.SingleOrDefault(x => x.Username == model.Username);

            // validate
            if (user == null)
                throw new AppException("User does not exist");

            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                throw new AppException("Password is incorrect");

            // authentication successful
            var response = _mapper.Map<AuthenticateResponse>(user);
            response.Token = _jwtUtils.GenerateToken(user);
            return response;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }

        public User GetById(string id)
        {
            return GetUser(id);
        }

        public void Register(RegisterRequest model)
        {
            // validate
            if (_context.Users.Any(x => x.Username == model.Username))
                throw new AppException("Username '" + model.Username + "' is already taken");

            // map model to new user object
            var user = _mapper.Map<User>(model);

            // hash password
            user.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);

            // save user
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(string id, UpdateRequest model)
        {
            var user = GetUser(id);

            // validate
            if (model.Username != user.Username && _context.Users.Any(x => x.Username == model.Username))
                throw new AppException("Username '" + model.Username + "' is already taken");

            // hash password if it was entered
            if (!string.IsNullOrEmpty(model.Password))
                user.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);

            // copy model to user and save
            _mapper.Map(model, user);
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(string id)
        {
            var user = GetUser(id);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        // helper methods

        private User GetUser(string id)
        {
            var user = _context.Users.Find(id);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }

        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            return imageName;
        }

        [NonAction]
        public void DeleteImage(string imageName)
        {
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
        }
    }
}