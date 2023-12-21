using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RumahMakanOnline_ViewModels;
using System.Text;

namespace RumahMakanOnline_FE.Services
{
    public class FoodVariantService
    {
        private static readonly HttpClient client = new HttpClient();
        private IConfiguration config;
        private string RouteAPI = "";
        private VMRespons respons = new VMRespons();

        public FoodVariantService(IConfiguration _config)
        {
            //Memanggil alamat port api di appsetting.json
            this.config = _config;
            this.RouteAPI = this.config["RouteAPI"];
        }

        public async Task<List<VMFoodVariant>> AllFoodVariant()
        {
            List<VMFoodVariant> DataVariant = new List<VMFoodVariant>();

            string apiRespons = await client.GetStringAsync(RouteAPI + "ApiFoodVariant/GetAllFoodVariant");

            //Proses mengbuah data string json ke list object
            DataVariant = JsonConvert.DeserializeObject<List<VMFoodVariant>>(apiRespons);

            return DataVariant;
        }

        public async Task<VMFoodVariant> GetById(int id)
        {
            VMFoodVariant data = new VMFoodVariant();

            string url = RouteAPI + $"ApiFoodVariant/GetById/{id}";
            string apiRespons = await client.GetStringAsync(url);
            data = JsonConvert.DeserializeObject<VMFoodVariant>(apiRespons);

            return data;
        }

        public async Task<List<VMFoodVariant>> GetFoodVariantByCategory(int idcategory)
        {
            List<VMFoodVariant> listFoodVariant = new List<VMFoodVariant>();

            string url = RouteAPI + $"ApiFoodVariant/GetFoodVariantByCategory/{idcategory}";
            string apiRespons = await client.GetStringAsync(url);

            listFoodVariant = JsonConvert.DeserializeObject<List<VMFoodVariant>>(apiRespons);

            return listFoodVariant;
        }

        public async Task<VMRespons> PutFoodVariant(VMFoodVariant data)
        {
            string dataJson = JsonConvert.SerializeObject(data);
            StringContent content = new StringContent(dataJson, UnicodeEncoding.UTF8, "application/json");

            string url = RouteAPI + "ApiFoodVariant/PutFoodVariant";

            //var request = await client.PutAsync(url, content);
            var request = await client.PutAsync(RouteAPI + "ApiFoodVariant/PutFoodVariant", content);
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
        public async Task<VMRespons> PostFoodVariant(VMFoodVariant data)
        {
            string dataJson = JsonConvert.SerializeObject(data);
            StringContent content = new StringContent(dataJson, UnicodeEncoding.UTF8, "application/json");

            string url = RouteAPI + "ApiFoodVariant/PostFoodVariant";

            var request = await client.PostAsync(url, content);

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
        public async Task<VMRespons> DeleteFoodVariant(int id)
        {
            string url = RouteAPI + $"ApiFoodVariant/DeleteFoodVariant/{id}";
            var request = await client.DeleteAsync(url);

            if (request.IsSuccessStatusCode)
            {
                var apiRespons = await request.Content.ReadAsStringAsync();
                respons = JsonConvert.DeserializeObject<VMRespons>(apiRespons);
            }
            else
            {
                respons.Success = false;
                respons.Message = $"{request.StatusCode} : {request.RequestMessage}";
            }
            return respons;
        }
    }
}
