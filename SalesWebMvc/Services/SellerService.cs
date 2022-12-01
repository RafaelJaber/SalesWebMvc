using SalesWebMvc.Data;
using SalesWebMvc.Models;

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
            return _context.Sellers.FirstOrDefault(obj => obj.Id == id);
        }

        public void Insert(Seller obj)
        {
            _context.Add(obj);
            _context.SaveChanges();
        }

        public void Update(Seller obj)
        {
            _context.Update(obj);
            _context.SaveChanges();
        }

        public void Delete(long id)
        {
            var obj = _context.Sellers.Find(id);
            _context.Remove(obj);
            _context.SaveChanges();
        }
    }
}
