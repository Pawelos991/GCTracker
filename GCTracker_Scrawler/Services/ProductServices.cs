using GC_Tracker_Datalayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GC_Tracker_Datalayer.Model;

namespace GCTracker_Scrawler.Services
{
    public interface IProductSevices
    {
        Task SaveProductAsync(Product item);
        Task SaveProductsAsync(List<Product> item);
    }

    public class ProductServices : IProductSevices
    {
        private GC_Tracker_Context _context;
        public ProductServices(GC_Tracker_Context context)
        {
            _context = context;
        }

        public async Task SaveProductAsync(Product item)
        {
            _context.Products.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task SaveProductsAsync(List<Product> item)
        {
            _context.Products.AddRange(item);
            await _context.SaveChangesAsync();
        }

    }
}
