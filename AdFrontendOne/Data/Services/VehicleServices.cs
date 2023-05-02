using AdFrontendOne.Data.DTO;
using Newtonsoft.Json;

namespace AdFrontendOne.Data.Services
{
    public class VehicleServices
    {
        private readonly HttpClient _httpClient;
        //private string baseUrl = "https://localhost:7190/";
        public VehicleServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<VehicleDTO>> GetVehicleAsync()
        {
            //localhostlink
            var response = await _httpClient.GetAsync("https://localhost:7050/api/vehicle");
            var result = response.Content.ReadAsStringAsync().Result;
            var rr = JsonConvert.DeserializeObject<List<VehicleDTO>>(result);
            return rr;
        }
    }
}
