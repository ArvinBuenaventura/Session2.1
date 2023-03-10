using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Session2Part1HomeWork
{
    public class PetModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("category")]
        public Category PetCategory { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("photoUrls")]
        public List<string> PhotoUrls { get; set; }
        [JsonProperty("tags")]
        public List<Category> PetTags { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }
    public class Category
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
    public class Tags
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
