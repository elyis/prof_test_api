using Microsoft.EntityFrameworkCore;
using prof_tester_api.src.Domain.Entities.Request;
using prof_tester_api.src.Domain.IRepository;
using prof_tester_api.src.Domain.Models;
using prof_tester_api.src.Infrastructure.Data;

namespace prof_tester_api.src.Infrastructure.Repository
{
    public class LecternRepository : ILecternRepository
    {
        private readonly AppDbContext _context;

        public LecternRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LecternModel?> AddAsync(CreateLecternBody lecternBody, OrganizationModel organization)
        {
            var newLectern = new LecternModel
            {
                Name = lecternBody.Name,
                Organization = organization,
            };

            var result = await _context.Lecterns.AddAsync(newLectern);
            await _context.SaveChangesAsync();
            return result?.Entity;
        }

        public async Task<IEnumerable<LecternModel>> GetAllAsync(Guid organizationId)
            => await _context.Lecterns
                .Where(e => e.OrganizationId == organizationId)
                .ToListAsync();

        public async Task<LecternModel?> GetAsync(Guid id)
            => await _context.Lecterns
                .FirstOrDefaultAsync(e => e.Id == id);


        public async Task<bool> RemoveAsync(Guid id)
        {
            var lectern = await GetAsync(id);
            if (lectern == null)
                return true;

            var result = _context.Lecterns.Remove(lectern);
            await _context.SaveChangesAsync();
            return result != null;
        }

        public async Task<LecternModel?> UpdateFilenameAsync(Guid lecternId, string filename)
        {
            var lectern = await GetAsync(lecternId);
            if (lectern == null)
                return null;

            lectern.Filename = filename;
            await _context.SaveChangesAsync();
            return lectern;
        }
    }
}