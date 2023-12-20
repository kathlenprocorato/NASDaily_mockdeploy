using CITNASDaily.Entities.Models;
using CITNASDaily.Repositories.Context;
using CITNASDaily.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CITNASDaily.Repositories.Repositories
{
    public class SuperiorRepository : ISuperiorRepository
    {

        private readonly NASContext _context;

        public SuperiorRepository(NASContext context)
        {
            _context = context;
        }

        public async Task<Superior?> CreateSuperiorAsync(Superior superior)
        {
            superior.FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(superior.FirstName);
            superior.LastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(superior.LastName);
            await _context.Superiors.AddAsync(superior);
            await _context.SaveChangesAsync();
            return superior;
        }


        public async Task<Superior?> GetSuperiorAsync(int superiorId)
        {
            return await _context.Superiors
                .Include(s => s.User)
                .FirstOrDefaultAsync(c => c.Id == superiorId);
        }


        public async Task<IEnumerable<Superior?>> GetSuperiorsAsync()
        {
            return await _context.Superiors.ToListAsync();
        }

        public async Task<Superior?> GetSuperiorByUserIdAsync(Guid? userId)
        {
            try
            {
                return await _context.Superiors
                    .FirstOrDefaultAsync(s => s.UserId == userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Superior?> GetSuperiorByUsernameAsync(string username)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);

                if (user == null)
                {
                    return null; // The user with the specified username does not exist
                }

                var superior = await _context.Superiors.FirstOrDefaultAsync(s => s.UserId == user.Id);

                return superior;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Superior?> GetSuperiorByOfficeId(int officeId)
        {
            var office = await _context.Offices.FirstOrDefaultAsync(o => o.Id == officeId);

            if (office != null)
            {
                return await _context.Superiors.FirstOrDefaultAsync(o => office.SuperiorFirstName == o.FirstName && office.SuperiorLastName == o.LastName);
            }
            return null;
        }

        public async Task<int> GetSuperiorIdByUsernameAsync(string username)
        {
            var superior = await _context.Superiors.FirstOrDefaultAsync(c => c.Username == username);
            if (superior != null)
            {
                return superior.Id;
            }
            return 0;
        }

        public async Task<bool> ChangePasswordAsync(int superiorId, string newPassword)
        {
            var superior = await _context.Superiors.FindAsync(superiorId);

            if (superior == null)
            {
                return false;
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == superior.Username);

            if (user == null)
            {
                return false;
            }

            user.PasswordHash = newPassword;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
