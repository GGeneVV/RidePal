using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RidePal.Data;
using RidePal.Models;
using RidePal.Services.Contracts;
using RidePal.Services.DTOModels;
using RidePal.Services.Extensions;
using RidePal.Services.Helpers;
using RidePal.Services.Wrappers.Contracts;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RidePal.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly IUserManagerWrapper _userManagerWrapper;
        private readonly AppSettings _appSettings;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(AppDbContext db, IMapper mapper,
            IUserManagerWrapper userManagerWrapper, IOptions<AppSettings> appSettings, IPasswordHasher<User> passwordHasher)
        {
            _db = db;
            _mapper = mapper;
            _userManagerWrapper = userManagerWrapper;
            _appSettings = appSettings.Value;
            _passwordHasher = passwordHasher;
        }

        public string Authenticate(string username, string password)
        {
            var user = _db.Users.SingleOrDefault(x => x.UserName == username);

            // return null if user not found
            if (user == null)
                return null;

            var passwordCheck = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, password);



            if (passwordCheck.ToString() != "Success")
            {
                return null;
            }

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var userToken = _db.UserTokens.SingleOrDefault(x => x.UserId == user.Id);

            if (userToken == null)
            {
                userToken = _db.UserTokens.Add(new IdentityUserToken<Guid>
                {
                    LoginProvider = "JWT-Custom",
                    UserId = user.Id,
                    Name = "API-token"
                }).Entity;
            }

            userToken.Value = tokenHandler.WriteToken(token);

            _db.SaveChanges();

            return userToken.Value;
        }
        public bool ValidateUserById(Guid UserId)
        {
            try
            {
                var userExist = _db.Users.Any(u => u.Id == UserId);

                return userExist;
            }
            catch (Exception)
            {

                throw new NotImplementedException();
            }
        }

        public IQueryable<UserDTO> GetAllUsersAsync(int? pageNumber = 1,
            string sortOrder = "",
            string currentFilter = "",
            string searchString = "")
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }


            currentFilter = searchString;

            var users = _db.Users
                .Where(t => t.IsDeleted == false)
                .Include(p => p.Playlists)
                .WhereIf(!String.IsNullOrEmpty(searchString), s => s.UserName.Contains(searchString))
                .Select(t => _mapper.Map<UserDTO>(t));

            switch (sortOrder)
            {
                case "Name_desc":
                    users = users.OrderByDescending(b => b.UserName);
                    break;
                case "PlaylistsCount":
                    users = users.OrderBy(b => b.Playlists.Count);
                    break;
                case "PlaylistsCount_decs":
                    users = users.OrderByDescending(s => s.Playlists.Count);
                    break;
                default:
                    users = users.OrderBy(s => s.UserName);
                    break;
            }

            int pageSize = 10;

            return users.AsQueryable();
        }

    }
}
