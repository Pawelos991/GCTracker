using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCTracker_Scrawler.Services
{
    public interface IImageServices
    {
        Task<byte[]> GetImageByUrl(string url);
    }

    public class ImageServices : IImageServices
    {
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
    }
}
