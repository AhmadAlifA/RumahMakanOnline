using Newtonsoft.Json;
using RumahMakanOnline_ViewModels;

namespace RumahMakanOnline_FE.Services
{
    public class MenuService
    {
        private static readonly HttpClient client = new HttpClient();
        private IConfiguration config;
        private string RouteAPI = "";
        private VMRespons respons = new VMRespons();
        public MenuService(IConfiguration _config)
        {
            //Memanggil alamat port api di appsetting.json
            this.config = _config;
            this.RouteAPI = this.config["RouteAPI"];
        }
        public async Task<List<VMMenu>> GetMenu(int RoleId)
        {
            List<VMMenu> data = new List<VMMenu>();

            string url = RouteAPI + $"ApiMenu/GetParent/{RoleId}";
            string apiRespons = await client.GetStringAsync(url);
            data = JsonConvert.DeserializeObject<List<VMMenu>>(apiRespons);

            return data;
        }
    }
}
