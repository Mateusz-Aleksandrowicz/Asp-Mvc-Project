using Asp_Net.DataAcess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asp_Net.DataAcess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
           Category = new CategoryRepository(_db);
           Product = new ProductRepository(_db);
           ShoppingCart = new ShoppingCartRepository(_db);
           User = new UserRepository(_db);
        }
         
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IUserRepository User { get; private set; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
