using GC_Tracker_Logic.Filters;
using Moq;
using GC_Tracker_Logic.Interfaces;
using GC_Tracker_Logic.Models;
using GCTracker_Backend.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace GCTracker_Tests
{
    public class GpuControllerTests
    {

        private readonly Mock<IGpuServices> _mockGpu;
        private readonly GpuController _controller;

        public GpuControllerTests()
        {
            _mockGpu = new Mock<IGpuServices>();
            _controller = new GpuController(_mockGpu.Object);
        }

        [Fact]
        public async Task GpuGetAllReturnCorrectNumberOfRecords()
        {
            _mockGpu.Setup(service => service.GetAllGpu()).Returns(Task.FromResult(new List<GpuDto>
            {
                new GpuDto(),
                new GpuDto(),
                new GpuDto(),
                new GpuDto(),
            }));

            var result = await _controller.GetAllGpu();


            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<GpuDto>>(viewResult.Value);
            Assert.Equal(model.Count(), 4);
        }

        [Fact]
        public async Task GpuGetByIdReturnOneElement()
        {
            _mockGpu.Setup(service => service.GetGpuById(1)).Returns(Task.FromResult(new GpuDto()));

            var result = await _controller.GetGpuById(1);

            var viewResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<GpuDto>(viewResult.Value);
        }

        [Fact]
        public async Task GpuGetByIdReturnElementWithCorrectId()
        {
            _mockGpu.Setup(service => service.GetGpuById(2)).Returns(Task.FromResult(new GpuDto() {Id = 2}));

            var result = await _controller.GetGpuById(2);

            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<GpuDto>(viewResult.Value);
            Assert.Equal(model.Id, 2);
        }

        [Fact]
        public async Task GpuFilterGetByIdReturnElementWithCorrectName()
        {
            const string EXPECT_NAME = "Name";
            ProductFilter filter = new()
                { Name = EXPECT_NAME, PriceStart = null, PriceEnd = null, ProducentCode = null, Skip = 0, Take = 1 };
            _mockGpu.Setup(service => service.GetFilterGpu(filter)).Returns(Task.FromResult(new List<GpuDto>()
            {
                new GpuDto() { Name = EXPECT_NAME},
                new GpuDto() {Name = EXPECT_NAME},
            }));

            var result = await _controller.GetPagingProductWithFilter(filter);

            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<GpuDto>>(viewResult.Value);
            Assert.True(model.TrueForAll(x => x.Name == EXPECT_NAME));
        }

        [Fact]
        public async Task GpuPaggingReturnCorrectNumberOfElements()
        {
            ProductFilter filter = new()
                { Name = null, PriceStart = null, PriceEnd = null, ProducentCode = null, Skip = 2, Take = 5 };
            _mockGpu.Setup(service => service.GetFilterGpu(filter)).Returns(Task.FromResult(new List<GpuDto>()
            {
                new GpuDto(),
                new GpuDto(),
                new GpuDto(),
                new GpuDto(),
                new GpuDto(),
            }));

            var result = await _controller.GetPagingProductWithFilter(filter);

            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<GpuDto>>(viewResult.Value);
            Assert.Equal(model.Count(), filter.Take);
        }
    }
}