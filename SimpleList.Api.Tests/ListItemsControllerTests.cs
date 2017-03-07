using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using SimpleList.Models;
using SimpleList.Shared;
using Xunit;

namespace SimpleList.Api.Tests
{
    public class ListItemsControllerTests : IClassFixture<StartFixture<TestStartup, Program>>
    {
        private readonly HttpClient _client;
        private string _authorizationCookie;

        private const string EndPoint = "api/listitems/";

        public ListItemsControllerTests(StartFixture<TestStartup, Program> fixture)
        {
            _client = fixture.Client;
            _authorizationCookie = fixture.AuthorizationCookie;
        }

        private HttpRequestMessage CreatePutRequest(string content, string parameters = "")
        {
            var request = CreateRequest(HttpMethod.Put, parameters);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            return request;
        }

        private HttpRequestMessage CreatePostRequest(string content, string parameters = "")
        {
            var request = CreateRequest(HttpMethod.Post, parameters);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            return request;
        }

        private HttpRequestMessage CreateRequest(HttpMethod method, string parameters = "")
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(_client.BaseAddress + EndPoint + parameters),
                Method = method
            };

            request.Headers.Add("Cookie", _authorizationCookie);

            return request;
        }

        [Fact]
        public async Task Remove()
        {
            // Ensure we get something to delete
            var getRequest = CreateRequest(HttpMethod.Get, TestDbInitializer.RemoveItem.Id.ToString());
            var getResponse = await _client.SendAsync(getRequest);
            getResponse.EnsureSuccessStatusCode();
            var getResult = await getResponse.Content.ReadAsStringAsync();
            var getted = JsonConvert.DeserializeObject<ListItem>(getResult);

            // Delete item
            var deleteRequest = CreateRequest(HttpMethod.Delete, TestDbInitializer.RemoveItem.Id.ToString());
            var deleteResponse = await _client.SendAsync(deleteRequest);
            deleteResponse.EnsureSuccessStatusCode();

            // We should get 404 after deleting the entry
            var invalidGetRequest = CreateRequest(HttpMethod.Get, TestDbInitializer.RemoveItem.Id.ToString()); 
            var getInvalidResponse = await _client.SendAsync(invalidGetRequest);
            Assert.True(getInvalidResponse.StatusCode == System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Create()
        {
            var item = new ListItem
            {
                Description = "Test add",
                Done = false
            };

            var postRequest = CreatePostRequest(JsonConvert.SerializeObject(item));

            // Insert Item
            var postResponse = await _client.SendAsync(postRequest);
            postResponse.EnsureSuccessStatusCode();
            var postResult = await postResponse.Content.ReadAsStringAsync();
            var inserted = JsonConvert.DeserializeObject<ListItem>(postResult);

            // Inserted item data should be equal
            Assert.Equal(item, inserted, new BaseListItemComparer(false));

            var getAllRequest = CreateRequest(HttpMethod.Get);
            var getAllResponse = await _client.SendAsync(getAllRequest);
            getAllResponse.EnsureSuccessStatusCode();
            var getAllResult = await getAllResponse.Content.ReadAsStringAsync();
            var allItems = JsonConvert.DeserializeObject<List<ListItem>>(getAllResult);

            // New item should be in All collection
            Assert.True(allItems.Count > 0);
            Assert.Contains(inserted, allItems, new BaseListItemComparer());

            // New item should be availiable by id
            var getRequest = CreateRequest(HttpMethod.Get, inserted.Id.ToString());
            var getResponse = await _client.SendAsync(getRequest);
            getResponse.EnsureSuccessStatusCode();
            var getResult = await getResponse.Content.ReadAsStringAsync();
            var getted = JsonConvert.DeserializeObject<ListItem>(getResult);

            Assert.Equal(inserted, getted, new BaseListItemComparer());
        }

        [Fact]
        public async Task Update()
        {
            var updateItem = new ListItem
            {
                Id = TestDbInitializer.UpdateItem.Id,
                Description = "Updated!",
                Done = true
            };

            var putRequest = CreatePutRequest(JsonConvert.SerializeObject(updateItem), TestDbInitializer.UpdateItem.Id.ToString());
            var putResponse = await _client.SendAsync(putRequest);
            putResponse.EnsureSuccessStatusCode();

            var getRequest = CreateRequest(HttpMethod.Get, TestDbInitializer.UpdateItem.Id.ToString());
            var getResponse = await _client.SendAsync(getRequest);
            getResponse.EnsureSuccessStatusCode();
            var getResult = await getResponse.Content.ReadAsStringAsync();
            var getted = JsonConvert.DeserializeObject<ListItem>(getResult);

            Assert.Equal(updateItem, getted, new BaseListItemComparer());
        }
    }
}
