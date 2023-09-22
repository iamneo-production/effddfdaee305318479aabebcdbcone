using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BookStoreApp.Models;

namespace BookStoreApp.Services
{
    public interface IApplicationService
    {
        bool AddApplication(Application Application);
        List<Application> GetAllApplications();
        bool DeleteApplication(int id);
    }
    public class ApplicationService : IApplicationService
    {
        private readonly HttpClient _httpClient;
            public ApplicationService(HttpClient httpClient,IConfiguration configuration)
        {
HttpClientHandler clientHandler = new HttpClientHandler();
clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
_httpClient=new HttpClient(clientHandler);
         var apiSettings = configuration.GetSection("ApiSettings").Get<ApiSettings>();
        _httpClient.BaseAddress =new Uri(apiSettings.BaseUrl) ;
        }

        public bool AddApplication(Application application)
        {
            try
            {
                var json = JsonConvert.SerializeObject(application);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress+$"/Application", content).Result;

                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }

        public List<Application> GetAllApplications()
        {
            try
            {
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress+"/Application").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<List<Application>>(data);
                }

                return new List<Application>();
            }
            catch (HttpRequestException)
            {
                return new List<Application>();
            }
        }

        public bool DeleteApplication(int id)
        {
            try
            {
                HttpResponseMessage response = _httpClient.DeleteAsync(_httpClient.BaseAddress+$"/Application/{id}").Result;

                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }
    }
}
