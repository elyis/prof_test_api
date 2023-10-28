using prof_tester_api.src.Domain.Entities.Request;
using prof_tester_api.src.Domain.IRepository;
using prof_tester_api.src.Domain.Models;
using prof_tester_api.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace prof_tester_api.src.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserModel?> AddAsync(SignUpBody body, string role, Guid adminId)
        {
            var oldUser = await GetAsync(body.Phone);
            if (oldUser != null)
                return null;

            var admin = await GetWithOrganizationAsync(adminId);


            var newUser = new UserModel
            {
                Phone = body.Phone,
                Password = body.Password,
                RoleName = role,
                Organization = admin.Organization
            };

            var result = await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return result?.Entity;
        }

        public async Task<UserModel?> GetAsync(Guid id)
            => await _context.Users
                .FirstOrDefaultAsync(e => e.Id == id);

        public async Task<UserModel?> GetAsync(string phone)
            => await _context.Users
                .FirstOrDefaultAsync(e => e.Phone == phone);

        public async Task<UserModel?> GetByTokenAsync(string refreshToken)
            => await _context.Users
                .FirstOrDefaultAsync(e => e.Token == refreshToken);

        public async Task<UserModel?> GetWithOrganizationAsync(Guid id)
            => await _context.Users.Include(e => e.Organization)
                .FirstOrDefaultAsync(e => e.Id == id);

        public async Task<UserModel?> UpdateProfileAsync(Guid userId, UpdateProfileBody body)
        {
            var user = await GetAsync(userId);
            if (user == null)
                return null;

            user.Fullname = body.Fullname;
            user.Email = body.Email;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<UserModel?> UpdateProfileIconAsync(Guid userId, string filename)
        {
            var user = await GetAsync(userId);
            if (user == null)
                return null;

            user.FilenameIcon = filename;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UpdateTokenAsync(string newRefreshToken, Guid id)
        {
            var user = await GetAsync(id);
            if (user == null)
                return false;

            user.Token = newRefreshToken;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}