using Newtonsoft.Json;
using RumahMakanOnline_ViewModels;
using System.Text;
using RumahMakanOnline_API.Models;

namespace RumahMakanOnline_FE.Services
{
    public class OrderService
    {
        private static readonly HttpClient client = new HttpClient();
        private IConfiguration config;
        private string RouteAPI = "";
        private VMRespons respons = new VMRespons();

        public OrderService(IConfiguration _config)
        {
            this.config = _config;
            this.RouteAPI = this.config["RouteAPI"];
        }

        public async Task<VMRespons> SubmitPayment(VMOrderHeader dataHeader)
        {
            string dataJson = JsonConvert.SerializeObject(dataHeader);
            StringContent content = new StringContent(dataJson, UnicodeEncoding.UTF8, "application/json");

            string url = RouteAPI + "ApiOrder/SubmitPayment";

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
        public async Task<List<VMOrderHeader>> AllOrderHeader(int IdCustomer)
        {
            List<VMOrderHeader> DataOrderHeader = new List<VMOrderHeader>();

            string apiRespons = await client.GetStringAsync(RouteAPI + $"ApiOrder/GetAllOrderHeader/{IdCustomer}");

            //Proses mengbuah data string json ke list object
            DataOrderHeader = JsonConvert.DeserializeObject<List<VMOrderHeader>>(apiRespons);

            return DataOrderHeader;
        }
        public async Task<int> countTransaction(int idUser)
        {
            int count = 0;

            string url = RouteAPI + $"ApiOrder/countTransaction/{idUser}";
            string apiRespons = await client.GetStringAsync(url);
            count = JsonConvert.DeserializeObject<int>(apiRespons);

            return count;
        }

        public async Task<decimal> AmounYear(int idUser)
        {
            decimal count = 0;

            string url = RouteAPI + $"ApiOrder/AmounYear/{idUser}";
            string apiRespons = await client.GetStringAsync(url);
            count = JsonConvert.DeserializeObject<decimal>(apiRespons);

            return count;
        }

    }
}
