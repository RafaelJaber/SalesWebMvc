using SalesWebMvc.Data;
using SalesWebMvc.Models;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Services
{
    public class SellerService
    {
        private readonly SalesWebMvcContext _context;


        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Sellers.ToListAsync();
        }

        public async Task<Seller> FindByIdAsync(long id)
        {
            Seller? seller = await _context.Sellers
                .Include(obj => obj.Department)
                .FirstOrDefaultAsync(obj => obj.Id == id);
            if (seller == null) return null;
            return seller;
        }

        public async Task InsertAsync (Seller obj)
        {
            _context.Add(obj);
            await _context.SaveChangesAsync();    
        }

        public async Task UpdateAsync(Seller obj)
        {
            bool hasAny = await _context.Sellers.AnyAsync(x => x.Id == obj.Id);
            if (!hasAny) throw new NotFoundException("Id not found");
            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }

        public async Task DeleteAsync(long id)
        {
            Seller? obj = await _context.Sellers.FindAsync(id);
            _context.Remove(obj);
            await _context.SaveChangesAsync();
        }
    }
}
