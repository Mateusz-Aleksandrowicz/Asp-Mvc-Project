using Asp_Net.DataAcess.Repository.IRepository;
using Asp_Net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asp_Net.DataAcess.Repository
{
    public class ProductRepository: Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product item)
        {
            var itemFromDb = _db.Products.FirstOrDefault(x => x.Id == item.Id);
            if (itemFromDb != null)
            {
                itemFromDb.Name = item.Name;
                itemFromDb.Description = item.Description;
                itemFromDb.Provenance = item.Provenance;
                itemFromDb.ListPrice = item.ListPrice;
                itemFromDb.Price = item.Price;
                itemFromDb.Price50 = item.Price50;
                itemFromDb.Price100 = item.Price100;
                itemFromDb.CategoryId = item.CategoryId;

                if (item.ImageUrl != null)
                {
                    itemFromDb.ImageUrl = item.ImageUrl;
                }              
            }
        }
    }
}
