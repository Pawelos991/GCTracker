using GC_Tracker_Logic.Filters;
using GC_Tracker_Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GCTracker_Backend.Controllers
{
    public class GpuController : Controller
    {

        private readonly IGpuServices _gpuServices;

        public GpuController(IGpuServices gpuServices)
        {
            _gpuServices = gpuServices;
        }

        [HttpGet("api/gpu/all")]
        public async Task<IActionResult> GetAllGpu()
        {
            var elementsToRet = await _gpuServices.GetAllGpu();
            return Ok(elementsToRet);
        }

        [HttpGet("api/gpu/{Id}")]
        public async Task<IActionResult> GetGpuById(int id)
        {
            var elementsToRet = await _gpuServices.GetGpuById(id);
            return Ok(elementsToRet);
        }

        [HttpGet("api/gpu/filter")]
        public async Task<IActionResult> GetPagingProductWithFilter([FromQuery] ProductFilter filter)
        {
            var elementsToRet = await _gpuServices.GetFilterGpu(filter);
            return Ok(elementsToRet);
        }

        [HttpGet("api/gpu/filter/count")]
        public async Task<IActionResult> GetCountPagingProductWithFilter([FromQuery] ProductFilter filter)
        {
            var elementsToRet = await _gpuServices.GetCountFilterGpu(filter);
            return Ok(elementsToRet);
        }

    }
}
