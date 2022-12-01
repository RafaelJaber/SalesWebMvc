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

        public List<Seller> FindAll()
        {
            return _context.Sellers.ToList();
        }

        public Seller FindById(long id)
        {
            var seller = _context.Sellers
                .Include(obj => obj.Department)
                .FirstOrDefault(obj => obj.Id == id);
            if (seller == null) return null;
            return seller;
        }

        public void Insert(Seller obj)
        {
            _context.Add(obj);
            _context.SaveChanges();
        }

        public void Update(Seller obj)
        {
            if (!_context.Sellers.Any(x => x.Id == obj.Id)) throw new NotFoundException("Id not found");
            try
            {
                _context.Update(obj);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }

        public void Delete(long id)
        {
            var obj = _context.Sellers.Find(id);
            _context.Remove(obj);
            _context.SaveChanges();
        }
    }
}
