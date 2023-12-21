using System.Text;
using RumahMakanOnline_API.Models;
using RumahMakanOnline_ViewModels;
using Newtonsoft.Json;

namespace RumahMakanOnline_FE.Services
{
    public class FoodCategoryService
    {
        private static readonly HttpClient client = new HttpClient();
        private IConfiguration config;
        private string RouteAPI = "";
        private VMRespons respons = new VMRespons();

        public FoodCategoryService(IConfiguration _config)
        {
            this.config = _config;
            this.RouteAPI = this.config["RouteAPI"];
        }
        public async Task<List<FoodCategory>> AllFoodCategory()
        {
            List<FoodCategory> dataFoodCategory = new List<FoodCategory>();

            string apiRespons = await client.GetStringAsync(RouteAPI + "ApiFoodCategory/GetAllFoodCategory");

            dataFoodCategory = JsonConvert.DeserializeObject<List<FoodCategory>>(apiRespons);

            return dataFoodCategory;
        }
        public async Task<FoodCategory> GetById(int id)
        {
            FoodCategory data = new FoodCategory();
            string url = RouteAPI + $"ApiFoodCategory/GetById/{id}";
            string apiRespons = await client.GetStringAsync(url);
            data = JsonConvert.DeserializeObject<FoodCategory>(apiRespons);
            return data;
        }
        public async Task<VMRespons> PutFoodCategory(FoodCategory data)
        {
            string DataJson = JsonConvert.SerializeObject(data);
            StringContent content = new StringContent
                                    (DataJson, UnicodeEncoding.UTF8, "application/json");
            string url = RouteAPI + "ApiFoodCategory/PutFoodCategory";
            var request = await client.PutAsync(url, content);
            if (request.IsSuccessStatusCode)
            {
                var apiRespons = await request.Content.ReadAsStringAsync();
                respons = JsonConvert.DeserializeObject<VMRespons>(apiRespons);
            }
            else
            {
                respons.Success = false;
                respons.Message = $"{request.StatusCode} : {request.ReasonPhrase}";
            }
            return respons;
        }
        public async Task<VMRespons> PostFoodCategory(FoodCategory data)
        {
            string DataJson = JsonConvert.SerializeObject(data);

            StringContent content = new StringContent
                                    (DataJson, UnicodeEncoding.UTF8, "application/Json");
            string url = RouteAPI + "ApiFoodCategory/PostFoodCategory";
            var request = await client.PostAsync(url, content);


            if (request.IsSuccessStatusCode)
            {
                var apiRespond = await request.Content.ReadAsStringAsync();
                respons = JsonConvert.DeserializeObject<VMRespons>(apiRespond);

            }
            else
            {
                respons.Success = false;
                respons.Message = $"{request.StatusCode} : {request.ReasonPhrase}";
            }
            return respons;
        }
        public async Task<VMRespons> DeleteFoodCategory(int id)
        {
            string url = RouteAPI + $"ApiFoodCategory/DeleteFoodCategory/{id}";
            var request = await client.DeleteAsync(url);

            if (request.IsSuccessStatusCode)
            {
                var apiRespon = await request.Content.ReadAsStringAsync();
                respons = JsonConvert.DeserializeObject<VMRespons>(apiRespon);
            }
            else
            {
                respons.Success = false;
                respons.Message = $"{request.StatusCode} : {request.ReasonPhrase}";
            }
            return respons;
        }
    }
}
