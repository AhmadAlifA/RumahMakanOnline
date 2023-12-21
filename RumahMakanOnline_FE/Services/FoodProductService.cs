using Newtonsoft.Json;
using RumahMakanOnline_ViewModels;
using System.Text;

namespace RumahMakanOnline_FE.Services
{
    public class FoodProductService
    {
        private static readonly HttpClient client = new HttpClient();
        private IConfiguration config;
        private string RouteAPI = "";
        private VMRespons respons = new VMRespons();

        public FoodProductService(IConfiguration _config)
        {
            //Memanggil alamat port api di appsetting.json
            this.config = _config;
            this.RouteAPI = this.config["RouteAPI"];
        }
        public async Task<List<VMFoodProduct>> AllProduct()
        {
            List<VMFoodProduct> DataProduct = new List<VMFoodProduct>();

            string apiRespons = await client.GetStringAsync(RouteAPI + "ApiFoodProduct/GetAllFoodProduct");

            //Proses mengbuah data string json ke list object
            DataProduct = JsonConvert.DeserializeObject<List<VMFoodProduct>>(apiRespons);

            return DataProduct;
        }
        public async Task<VMFoodProduct> GetById(int id)
        {
            VMFoodProduct data = new VMFoodProduct();

            string url = RouteAPI + $"ApiFoodProduct/GetById/{id}";
            string apiRespons = await client.GetStringAsync(url);
            data = JsonConvert.DeserializeObject<VMFoodProduct>(apiRespons);

            return data;
        }

        public async Task<VMRespons> PutProduct(VMFoodProduct data)
        {
            //Proses mengubah object menjadi string
            string dataJson = JsonConvert.SerializeObject(data);
            //Proses mengubah string menjadi json yang dapat di kenali oleh API
            StringContent content = new StringContent(dataJson, UnicodeEncoding.UTF8, "application/json");

            string url = RouteAPI + "ApiFoodProduct/PutFoodProduct";

            //Proses memanggil Endpoint API dan kirim data body
            var request = await client.PutAsync(url, content);
            //Jika berhasil hasil API nya
            if (request.IsSuccessStatusCode)
            {
                //Membaca hasil respon dari API
                var apiRespons = await request.Content.ReadAsStringAsync();
                //Convert ke object VMRespons
                respons = JsonConvert.DeserializeObject<VMRespons>(apiRespons);
            }
            else
            {
                respons.Success = false;
                respons.Message = $"{request.StatusCode} : {request.ReasonPhrase}";
            }
            return respons;
        }
        public async Task<VMRespons> PostProduct(VMFoodProduct data)
        {
            string dataJson = JsonConvert.SerializeObject(data);
            StringContent content = new StringContent(dataJson, UnicodeEncoding.UTF8, "application/json");

            string url = RouteAPI + "ApiFoodProduct/PostFoodProduct";

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
        public async Task<VMRespons> DeleteProduct(int id)
        {
            string url = RouteAPI + $"ApiFoodProduct/DeleteFoodProduct/{id}";
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
