using CITNASDaily.Entities.Dtos.OfficeDtos;
using CITNASDaily.Entities.Models;
using CITNASDaily.Repositories.Context;
using CITNASDaily.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CITNASDaily.Repositories.Repositories
{
    public class OfficeRepository : IOfficeRepository
    {
        private readonly NASContext _context;

        public OfficeRepository(NASContext context)
        {
            _context = context;
        }
        public async Task<Office?> CreateOfficeAsync(Office office)
        {
            var existingOffice = await _context.Offices.FindAsync(office.Id);

            if(existingOffice != null)
            {
                return null;
            }

            await _context.Offices.AddAsync(office);
            await _context.SaveChangesAsync();
            return office;
        }

        public async Task<Office?> GetOfficeByIdAsync(int id)
        {
            return await _context.Offices.Include(o => o.NAS).FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Office?> GetOfficeByNASIdAsync(int nasId)
        {
            var nas = await _context.NAS.FirstOrDefaultAsync(n => n.Id == nasId);
            if (nas != null) 
            {
                return await _context.Offices.Include(n => n.NAS.Where(n => n.Id == nasId)).FirstOrDefaultAsync(o => o.Id == nas.OfficeId);
            }
            return null;
        }

        public async Task<Office?> GetOfficeBySuperiorIdAsync(int superiorId)
        {
            var existingSuperior = await _context.Superiors.FindAsync(superiorId);

            if (existingSuperior == null)
            {
                return null;
            }

            var office = await _context.Offices.Include(o => o.NAS).FirstOrDefaultAsync(o => o.SuperiorFirstName == existingSuperior.FirstName && o.SuperiorLastName == existingSuperior.LastName);

            return office;
        }

        public async Task<IEnumerable<Office?>> GetOfficesAsync()
        {
            var office = await _context.Offices.Include(o => o.NAS).ToListAsync();
            if (office == null)
            {
                return await _context.Offices.ToListAsync();
            }
            return office;
        }

        public async Task<string?> GetOfficeNameAsync(int officeId)
        {
            var office = await _context.Offices.FindAsync(officeId);

            if (office == null)
            {
                return null;
            }

            return office.OfficeName;
        }

        public async Task<Office?> UpdateOfficeAsync(Office office)
        {
            office.SuperiorFirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(office.SuperiorFirstName);
            office.SuperiorLastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(office.SuperiorLastName);

            var existingOffice = await _context.Offices.Include(o => o.NAS).SingleOrDefaultAsync(o => o.Id == office.Id);
            
            if (existingOffice == null)
            {
                return null;
            }

            var existingSuperior = await _context.Superiors.SingleOrDefaultAsync(s => s.FirstName == office.SuperiorFirstName && s.LastName == office.SuperiorLastName);

            if(existingSuperior == null)
            {
                return null; 
            }

            //make the office name of previous superior null
            var previousSuperior = await _context.Superiors.SingleOrDefaultAsync(s => s.FirstName == existingOffice.SuperiorFirstName && s.LastName == existingOffice.SuperiorLastName);
            if (previousSuperior != null)
            {
                previousSuperior.OfficeName = null;
            }

            existingOffice.SuperiorFirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(office.SuperiorFirstName);
            existingOffice.SuperiorLastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(office.SuperiorLastName);
            existingSuperior.OfficeName = existingOffice.OfficeName;
            await _context.SaveChangesAsync();
            return existingOffice;
        } 
    }
}
