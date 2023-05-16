using GC_Tracker_Logic.Interfaces;
using GCTracker_Backend.Controllers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GC_Tracker_Logic.Services;
using Microsoft.AspNetCore.Mvc;
using GC_Tracker_Datalayer.Context;
using GC_Tracker_Datalayer.Model;
using GC_Tracker_Logic.Filters;
using GC_Tracker_Logic.Models;
using GCTracker_Tests.Helper;
using MockQueryable.Moq;
using System.Data.Entity;
using System.Web.Http.Filters;

namespace GCTracker_Tests
{
    public class GpuServicesTests
    {
        private readonly Mock<GC_Tracker_Context> _mockContext;

        public GpuServicesTests()
        {
            _mockContext = new Mock<GC_Tracker_Context>();
        }

        [Fact]
        public async Task GpuGetAllReturnCorrectNumberOfRecords()
        {
            var mock = ProductMock.GetFakeProductList().BuildMock().BuildMockDbSet();
            var employeeContextMock = new Mock<GC_Tracker_Context>();
                employeeContextMock.Setup(x => x.Products).Returns(mock.Object);

            GpuServcies service = new GpuServcies(employeeContextMock.Object);
            var result = await service.GetAllGpu();

            var viewResult = Assert.IsType<List<GpuDto>>(result);
            Assert.NotNull(viewResult);
            Assert.Equal(viewResult.Count(), ProductMock.GetFakeProductList().Count());
        }

        [Fact]
        public async Task GpuGetByIdReturnCorrectId()
        {
            var mock = ProductMock.GetFakeProductList().BuildMock().BuildMockDbSet();
            mock.Setup(x => x.FindAsync(1)).ReturnsAsync(ProductMock.GetFakeProductList().Find(x => x.Id == 1));

            var employeeContextMock = new Mock<GC_Tracker_Context>();
            employeeContextMock.Setup(x => x.Products).Returns(mock.Object);

            GpuServcies service = new GpuServcies(employeeContextMock.Object);
            var result = await service.GetGpuById(1);

            var viewResult = Assert.IsType<GpuDto>(result);
            Assert.NotNull(viewResult);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task GpuGetPaggingReturnCorrectSequecnce()
        {
            ProductFilter filter = new ProductFilter() { Skip = 2, Take = 5};
            var mock = ProductMock.GetFakeProductList().BuildMock().BuildMockDbSet();

            var employeeContextMock = new Mock<GC_Tracker_Context>();
            employeeContextMock.Setup(x => x.Products).Returns(mock.Object);

            GpuServcies service = new GpuServcies(employeeContextMock.Object);
            var result = await service.GetFilterGpu(filter);

            var viewResult = Assert.IsType<List<GpuDto>>(result);
            Assert.NotNull(viewResult);
            Assert.Equal(result.Count, filter.Take);
            Assert.Equal(result.First().Id, filter.Skip );
        }

        [Fact]
        public async Task IsGpuFilterReturnCorrectValues()
        {
            ProductFilter filter = new ProductFilter() { Name = "Name", PriceStart = 2, PriceEnd = 10, ProducentCode = "Prd1", Skip = 0, Take = 10 };
            var mock = ProductMock.GetFakeProductList().BuildMock().BuildMockDbSet();

            var employeeContextMock = new Mock<GC_Tracker_Context>();
            employeeContextMock.Setup(x => x.Products).Returns(mock.Object);

            GpuServcies service = new GpuServcies(employeeContextMock.Object);
            var result = await service.GetFilterGpu(filter);

            var viewResult = Assert.IsType<List<GpuDto>>(result);
            Assert.NotNull(viewResult);
            Assert.True(result.TrueForAll(x => x.Name.Contains(filter.Name)));
            Assert.True(result.TrueForAll(x => x.ProducentCode.Contains(filter.ProducentCode)));
            Assert.True(result.TrueForAll(x => x.Price >= filter.PriceStart));
            Assert.True(result.TrueForAll(x => x.Price <= filter.PriceEnd));
        }

    }
}
