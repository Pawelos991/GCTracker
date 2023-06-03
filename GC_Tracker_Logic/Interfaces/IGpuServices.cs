using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GC_Tracker_Logic.Filters;
using GC_Tracker_Logic.Models;

namespace GC_Tracker_Logic.Interfaces
{
    public interface IGpuServices
    {
        public Task<GpuDto> GetGpuById(int id);
        public Task<List<GpuDto>> GetFilterGpu(ProductFilter filter);
        public Task<int> GetCountFilterGpu(ProductFilter filter);
        public Task<List<GpuDto>> GetAllGpu();
        public Task<PredicedPrice> CheckGpuTrendByProducentCode(string producentCode);

    }
}
