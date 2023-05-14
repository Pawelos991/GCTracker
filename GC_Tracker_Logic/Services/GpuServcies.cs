using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GC_Tracker_Datalayer.Context;
using GC_Tracker_Logic.Filters;
using GC_Tracker_Logic.Interfaces;
using GC_Tracker_Logic.Models;
using Microsoft.EntityFrameworkCore;

namespace GC_Tracker_Logic.Services
{
    public class GpuServcies : IGpuServices
    {
        private readonly GC_Tracker_Context _context;

        public GpuServcies(GC_Tracker_Context context)
        {
            _context = context;
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
                };
            }

            return null;
        }
    }
}
