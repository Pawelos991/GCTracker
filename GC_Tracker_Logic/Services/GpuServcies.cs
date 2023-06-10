using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GC_Tracker_Datalayer.Context;
using GC_Tracker_Logic.Filters;
using GC_Tracker_Logic.Interfaces;
using GC_Tracker_Logic.ML;
using GC_Tracker_Logic.Models;
using Microsoft.EntityFrameworkCore;

namespace GC_Tracker_Logic.Services
{
    public class GpuServcies : IGpuServices
    {
        private readonly GC_Tracker_Context _context;
        private readonly PredictPriceChange _mlPriceChange;
        public GpuServcies(GC_Tracker_Context context)
        {
            _context = context;
            _mlPriceChange = new PredictPriceChange();
        }

        public async Task<List<GpuDto>> GetAllGpu()
        {
            return await _context.Products.Select(x => new GpuDto()
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                ProducentCode = x.ProducentCode,
                ImageAddress = x.ImageAddress,
                StoreName = x.StoreName,
            }).ToListAsync();
        }

        public async Task<List<GpuDto>> GetFilterGpu(ProductFilter filter)
        {
            return await _context.Products.Where(x => 
                ((String.IsNullOrEmpty(filter.Name))|| x.Name.Contains(filter.Name)) &&
                ((String.IsNullOrEmpty(filter.ProducentCode)) || x.ProducentCode.Contains(filter.ProducentCode)) &&
                ((filter.PriceStart == null) || x.Price >= filter.PriceStart) &&
                ((filter.PriceEnd == null) || x.Price <= filter.PriceEnd)
                ).Skip(filter.Skip).Take(filter.Take).Select(x => new GpuDto()
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                ProducentCode = x.ProducentCode,
                ImageAddress = x.ImageAddress,
                StoreName = x.StoreName,
                }).ToListAsync();
        }

        public async Task<int> GetCountFilterGpu(ProductFilter filter)
        {
            return await _context.Products.Where(x =>
                (String.IsNullOrEmpty(filter.Name) || x.Name.Contains(filter.Name)) &&
                (String.IsNullOrEmpty(filter.ProducentCode) || x.ProducentCode.Contains(filter.ProducentCode)) &&
                (filter.PriceStart == null || x.Price >= filter.PriceStart) &&
                (filter.PriceEnd == null || x.Price <= filter.PriceEnd)
            ).CountAsync();
        }

        public async Task<GpuDto> GetGpuById(int id)
        {
            var elemToRet = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (elemToRet != null)
            {
                return new GpuDto()
                {
                    Id = elemToRet.Id,
                    Name = elemToRet.Name,
                    Price = elemToRet.Price,
                    ImageAddress = elemToRet.ImageAddress,
                    ProducentCode = elemToRet.ProducentCode,
                    StoreName = elemToRet.StoreName,
                    Image = (await _context.Images.FirstOrDefaultAsync(x => x.ProducentCode == elemToRet.ProducentCode))?.Img
                };
            }

            return null;
        }

        public async Task<PredicedPrice> CheckGpuTrendByProducentCode(string producentCode)
        {

            var elemsToPredict = await _context.Products.Where(x => x.ProducentCode == producentCode).Select(x => new GpuDto()
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                ProducentCode = x.ProducentCode,
                ImageAddress = x.ImageAddress,
                StoreName = x.StoreName,
            }).ToListAsync();
            while (elemsToPredict.Count < 5) elemsToPredict.Add(elemsToPredict.Last());
            var status = await _mlPriceChange.GpuPriceChange(elemsToPredict);
            return status;
        }
    }
}
