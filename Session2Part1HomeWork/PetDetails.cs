using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Session2Part1HomeWork;

[assembly: Parallelize(Workers = 10, Scope = ExecutionScope.MethodLevel)]

namespace Session2Part1HomeWork
{
    [TestClass]
    public class PetDetails
    {
        private static HttpClient httpClient;

        private static readonly string BaseURL = "https://petstore.swagger.io/v2/";

        private static readonly string PetEndpoint = "pet";

        private static string GetURL(string enpoint) => $"{BaseURL}{enpoint}";

        private static Uri GetURI(string endpoint) => new Uri(GetURL(endpoint));

        private readonly List<PetModel> cleanUpList = new List<PetModel>();

        [TestInitialize]
        public void TestInitialize()
        {
            httpClient = new HttpClient();
        }

        [TestCleanup]
        public async Task TestCleanUp()
        {
            foreach (var data in cleanUpList)
            {
                var httpResponse = await httpClient.DeleteAsync(GetURL($"{PetEndpoint}/{data.Id}"));
            }
        }

        [TestMethod]
        public async Task PutPetMethod()
        {

            #region create data
            Category category = new Category();
            category.Id = 99;
            category.Name = "Test99";

            Category tag1 = new Category()
            {
                Id = 25,
                Name = "Tag25"
            };
            Category tag2 = new Category()
            {
                Id = 26,
                Name = "Tag26"
            };
            // Create Json Object
            PetModel petData = new PetModel()
            {
                Id = 99,
                PetCategory = new Category()
                {
                    Id = 99,
                    Name = "Puspin"
                },

                Name = "Milkty",
                PhotoUrls = new List<string> { "99url", "100url" },
                PetTags = new List<Category> { tag1, tag2 },
                Status = "available",
            };

            // Serialize Content
            var request = JsonConvert.SerializeObject(petData);
            var postRequest = new StringContent(request, Encoding.UTF8, "application/json");

            // Send Post Request
            await httpClient.PostAsync(GetURL(PetEndpoint), postRequest);

            #endregion

            #region get Username of the created data

            // Get Request
            var getResponse = await httpClient.GetAsync(GetURI($"{PetEndpoint}/{petData.Id}"));

            // Deserialize Content
            var listPetData = JsonConvert.DeserializeObject<PetModel>(getResponse.Content.ReadAsStringAsync().Result);

            // filter created data
            var createdPetData = listPetData;

            #endregion

            #region send put request to update data

            // Update value of petData
            Category updatedCategory = new Category();
            category.Id = 99;
            category.Name = "Test";

            Category updatedTag1 = new Category()
            {
                Id = 25,
                Name = "taguro25"
            };
            Category updatedTag2 = new Category()
            {
                Id = 26,
                Name = "taguro26"
            };

            petData = new PetModel()
            {
                Id = listPetData.Id,
                PetCategory = new Category()
                {
                    Id = 100,
                    Name = "Siamese"
                },
                Name = "MIng",
                PhotoUrls = new List<string> { "909url", "1001url" },
                PetTags = new List<Category> { updatedTag1, updatedTag2 },
                Status = "unavailable"
            };


            // Serialize Content
            request = JsonConvert.SerializeObject(petData);
            postRequest = new StringContent(request, Encoding.UTF8, "application/json");

            // Send Put Request
            var httpResponse = await httpClient.PutAsync(GetURL($"{PetEndpoint}"), postRequest);

            // Get Status Code
            var statusCode = httpResponse.StatusCode;

            #endregion

            #region get updated data

            // Get Request
            getResponse = await httpClient.GetAsync(GetURI($"{PetEndpoint}/{petData.Id}"));

            // Deserialize Content
            listPetData = JsonConvert.DeserializeObject<PetModel>(getResponse.Content.ReadAsStringAsync().Result);

            // filter created data
            createdPetData = listPetData;

            #endregion

            #region cleanup data

            // Add data to cleanup list
            cleanUpList.Add(listPetData);

            #endregion

            #region assertion

            // Assertion
            Assert.AreEqual(HttpStatusCode.OK, statusCode, "Status code is not equal to 201");
            Assert.AreEqual(petData.Id, listPetData.Id, "not matching");
            Assert.AreEqual(petData.PetCategory.Id, listPetData.PetCategory.Id, "not matching");
            Assert.AreEqual(petData.PetCategory.Name, listPetData.PetCategory.Name, "not matching");
            Assert.AreEqual(petData.Name, listPetData.Name, "not matching");
            //Assert.AreEqual(petData.PhotoUrls, listPetData.PhotoUrls, "not matching");
            //Assert.AreEqual(petData.PetTags, listPetData.PetTags, "not matching");
            Assert.AreEqual(petData.Status, listPetData.Status, "not matching");

            #endregion
        }
/*
        [TestMethod]
        public async Task DeletePetMethod()
        {
            #region create data

            Category category = new Category();
            category.Id = 78;
            category.Name = "Test";

            Category tag1 = new Category()
            {
                Id = 25,
                Name = "Tag1"
            };
            Category tag2 = new Category()
            {
                Id = 26,
                Name = "Tag2"
            };
            // Create Json Object
            PetModel petData = new PetModel()
            {
                Id = 78,
                PetCategory = new Category()
                {
                    Id = 98,
                    Name = "Askal"
                },

                Name = "Tala",
                PhotoUrls = new List<string> { "url3", "url4" },
                PetTags = new List<Category> { tag1, tag2 },
                Status = "available",
            };

            // Serialize Content
            var request = JsonConvert.SerializeObject(petData);
            var postRequest = new StringContent(request, Encoding.UTF8, "application/json");

            // Send Post Request
            await httpClient.PostAsync(GetURL(PetEndpoint), postRequest);

            #endregion

            #region get Username of the created data

            // Get Request
            var getResponse = await httpClient.GetAsync(GetURI($"{PetEndpoint}/{petData.Id}"));

            // Deserialize Content
            var listPetData = JsonConvert.DeserializeObject<PetModel>(getResponse.Content.ReadAsStringAsync().Result);

            // filter created data
            var createdPetDataId = listPetData.Id;

            #endregion

            #region send delete request

            // Send Delete Request
            var httpResponse = await httpClient.DeleteAsync(GetURL($"{PetEndpoint}/{createdPetDataId}"));

            // Get Status Code
            var statusCode = httpResponse.StatusCode;

            #endregion

            #region assertion

            // Assertion
            Assert.AreEqual(HttpStatusCode.OK, statusCode, "Status code is not equal to 201");

            #endregion
        }
*/
    }
}