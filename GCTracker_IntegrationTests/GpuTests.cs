using Azure.Core;
using GCTracker_IntegrationTests.Provider;
using System.Net;
using GC_Tracker_Logic.Models;
using Newtonsoft.Json;
using System.Collections.Specialized;

namespace GCTracker_IntegrationTests
{
    public class GpuTests
    {
        [Fact]
        public async Task TestGetAllGpu()
        {

            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("/api/gpu/all");

                response.EnsureSuccessStatusCode();
                string jsonContent = await response.Content.ReadAsStringAsync();
                var contentObject = JsonConvert.DeserializeObject<List<GpuDto>>(jsonContent);

                Assert.NotNull(contentObject);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task TestGetGpuById()
        {

            using (var client = new TestClientProvider().Client)
            {
                int testId = 1;
                var response = await client.GetAsync("/api/gpu/1");

                response.EnsureSuccessStatusCode();
                string jsonContent = await response.Content.ReadAsStringAsync();
                var contentObject = JsonConvert.DeserializeObject<GpuDto>(jsonContent);

                Assert.NotNull(contentObject);
                Assert.Equal(contentObject.Id, testId);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task TestGetGpuFilter()
        {

            using (var client = new TestClientProvider().Client)
            {
                NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
                queryString.Add("Name", "Name");
                queryString.Add("ProducentCode", "pro");
                queryString.Add("Skip", "0");
                queryString.Add("Take", "20");

                var response = await client.GetAsync($"/api/gpu/filter?{queryString}");
                var responseCount = await client.GetAsync($"/api/gpu/filter/count?{queryString}");

                response.EnsureSuccessStatusCode();
                responseCount.EnsureSuccessStatusCode();

                string jsonContent = await response.Content.ReadAsStringAsync();
                string jsonContentCount = await responseCount.Content.ReadAsStringAsync();
                var contentObject = JsonConvert.DeserializeObject<List<GpuDto>>(jsonContent);
                var contentObjectCount = JsonConvert.DeserializeObject<int>(jsonContentCount);

                Assert.NotNull(contentObject);
                Assert.Equal(contentObject.Count(), contentObjectCount);
                Assert.True(contentObject.TrueForAll(x => x.Name.Contains("Name")));
                Assert.True(contentObject.TrueForAll(x => x.ProducentCode.Contains("pro")));
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }
    }
}