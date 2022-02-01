using Newtonsoft.Json;
using PetStore.Web.Models;
using PetStore.Web.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Web.Services
{

    public class BaseService : IBaseService
    {

        public ResponseDto ResponseModel { get; set; }
        public IHttpClientFactory HttpClient { get; set; }
        public void  Dispose()
        {
            GC.SuppressFinalize(true);
        }
    
        public BaseService(IHttpClientFactory httpclient)

        {

            ResponseModel =  new ResponseDto();
            HttpClient = httpclient;



        }
        public async Task<T> SendAsync<T>(ApiRequest ApiRequest)
        {
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                // Pass the handler to httpclient(from you are calling api)
                HttpClient Client = new HttpClient(clientHandler);
                //var Client = HttpClient.CreateClient("PetApi");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(ApiRequest.Url);
                Client.DefaultRequestHeaders.Clear();
                if (ApiRequest.Data != null)
                {

                    message.Content = new StringContent(JsonConvert.SerializeObject(ApiRequest.Data), Encoding.UTF8, "application/json");
                }
                if (ApiRequest.AccessToken != null)
                {

                    Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ApiRequest.AccessToken);
                }
                HttpResponseMessage apiResponse = new HttpResponseMessage();
                switch (ApiRequest.ApiType)
                {
                    case SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case SD.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;


                }
                apiResponse = await Client.SendAsync(message);
                var apicontent = await apiResponse.Content.ReadAsStringAsync();
                var ApiResponseDto = JsonConvert.DeserializeObject<T>(apicontent);
                return ApiResponseDto;

            }
            catch (Exception e)
            {

                var dto = new ResponseDto()
                {
                    IsSuccess = false,
                    ErrorMessages=new List<string> { e.Message},
                    DisplayMessage="ERROR"
                 
                };
                var res = JsonConvert.SerializeObject(dto);
                var apiResponse = JsonConvert.DeserializeObject<T>(res);
                return apiResponse;
            }
        }
    }
}
