using Newtonsoft.Json;
using RumahMakanOnline_API.Models;
using RumahMakanOnline_ViewModels;
using System.Text;

namespace RumahMakanOnline_FE.Services
{
    public class AuthService
    {
        private static readonly HttpClient client = new HttpClient();
        private IConfiguration config;
        private string RouteAPI = "";
        private VMRespons respons = new VMRespons();

        public AuthService(IConfiguration _config)
        {
            //Memanggil alamat port api di appsetting.json
            this.config = _config;
            this.RouteAPI = this.config["RouteAPI"];
        }

        public async Task<List<TblRole>> AllRole()
        {
            List<TblRole> DataRole = new List<TblRole>();

            string apiRespons = await client.GetStringAsync(RouteAPI + "ApiAuth/GetAllRole");

            //Proses mengbuah data string json ke list object
            DataRole = JsonConvert.DeserializeObject<List<TblRole>>(apiRespons);

            return DataRole;
        }
        public async Task<VMRespons> Register(VMUserCustomer data)
        {
            string dataJson = JsonConvert.SerializeObject(data);
            StringContent content = new StringContent(dataJson, UnicodeEncoding.UTF8, "application/json");

            string url = RouteAPI + "ApiAuth/Register";

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
        public async Task<bool> CheckEmailDuplicate(string email)
        {
            bool checkEmail = false;

            string url = RouteAPI + $"ApiAuth/CheckEmail/{email}";
            string apiRespons = await client.GetStringAsync(url);
            checkEmail = JsonConvert.DeserializeObject<bool>(apiRespons);

            return checkEmail;
        }
        public async Task<VMUserCustomer> Login(VMUserCustomer data)
        {
            VMUserCustomer dataUser = new VMUserCustomer();

            string url = RouteAPI + $"ApiAuth/Login/{data.Email}/{data.Password}";
            var apiRespons = await client.GetStringAsync(url);

            dataUser = JsonConvert.DeserializeObject<VMUserCustomer>(apiRespons);

            return dataUser;
        }
    }
}
