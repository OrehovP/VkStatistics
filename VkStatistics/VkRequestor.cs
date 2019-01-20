using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VkStatistics.ViewModels;

namespace VkStatistics
{
    class VkRequestor
    {
        private const string accessToken = "37955cf68519a72f3c166e1a3b001973448e37e73156663ac1c17be43fc356b7463c378bc108979968b13";
        private const string host = "https://api.vk.com";
        private const string resource = "/method";
        private const string apiVersion = "5.52";
        private const int publicId = -176935598;

        private readonly IRestClient client = new RestClient(host);

        public List<WallItemViewModel> GetWallItems(string userId)
        {
            string method = "/wall.get";
            IRestRequest request = new RestRequest(resource + method, Method.GET);

            request.AddParameter("v", apiVersion);
            request.AddParameter("access_token", accessToken);
            request.AddParameter("domain", userId);
            request.AddParameter("count", 5);

            IRestResponse response = client.Execute(request);
            if (!ValidateResponse(response)) return null;

            string wallItemsJSON = JObject.Parse(response.Content)["response"]["items"].ToString();
            List<WallItemViewModel> wallItems = JsonConvert.DeserializeObject<List<WallItemViewModel>>(wallItemsJSON);
            if (wallItems.Count > 0)
                return wallItems;
            return null;
        }

        public UserViewModel GetUserInfo(string userId)
        {
            var method = "/users.get";
            IRestRequest request = new RestRequest(resource + method, Method.GET);

            request.AddParameter("v", apiVersion);
            request.AddParameter("access_token", accessToken);
            request.AddParameter("user_ids", userId);

            IRestResponse response = client.Execute(request);
            if (!ValidateResponse(response)) return null;

            string userInfoJSON = JObject.Parse(response.Content)["response"][0].ToString();
            UserViewModel userInfo = JsonConvert.DeserializeObject<UserViewModel>(userInfoJSON);
            return userInfo;
        }

        public void PostStatistics(string statistics)
        {
            var method = "/wall.post";
            IRestRequest request = new RestRequest(resource + method, Method.POST);

            request.AddParameter("v", apiVersion);
            request.AddParameter("access_token", accessToken);
            request.AddParameter("owner_id", publicId);
            request.AddParameter("message", statistics);

            IRestResponse response = client.Execute(request);
            if (ValidateResponse(response))
                Console.WriteLine("Опубликовано в группе https://vk.com/club176935598");
            else
                Console.WriteLine("Публикация в группе https://vk.com/club176935598 не удалась!");
        }

        private bool ValidateResponse(IRestResponse response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var error = JObject.Parse(response.Content)["error"];
                if (error == null) return true;

                var errorCode = error["error_code"];
                var errorTitle = error["error_msg"];
                Console.WriteLine($"Код ошибки: {errorCode}. {errorTitle}");
            }
            return false;
        }
    }
}