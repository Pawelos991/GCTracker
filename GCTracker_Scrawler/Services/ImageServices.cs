using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GC_Tracker_Datalayer.Context;
using GC_Tracker_Datalayer.Model;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GCTracker_Scrawler.Services
{
    public interface IImageServices
    {
        Task<byte[]> GetImageByUrl(string url);
        Task<Images> SaveImages(Images images);
        Task<bool> CheckIsImageExistInDatabase(string producetCode);
    }

    public class ImageServices : IImageServices
    {
        private GC_Tracker_Context _context;
        public ImageServices(GC_Tracker_Context context)
        {
            _context = context;
        }

        public async Task<byte[]> GetImageByUrl(string url)
        {
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(url))
                {
                    byte[] imageBytes =  await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                    return imageBytes;
                }
            }
        }

        public async Task<Images> SaveImages(Images images)
        {
            _context.Images.Add(images);
            await _context.SaveChangesAsync();
            return images;
        }

        public async Task<bool> CheckIsImageExistInDatabase(string producetCode)
        {
            return await _context.Images.AnyAsync(x => x.ProducentCode == producetCode);
        }
    }
}
