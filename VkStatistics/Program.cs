using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkStatistics.ViewModels;

namespace VkStatistics
{
    class Program
    {
        static void Main(string[] args)
        {
            VkRequestor vkRequestor = new VkRequestor();
            while (true)
            {
                Console.WriteLine("\nВведите id пользователя:");
                string userId = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(userId)) break;
                if (userId[0] >= '0' && userId[0] <= '9')
                    userId = "id" + userId;
                if (userId.StartsWith("-"))
                {
                    Console.WriteLine("Неккоректный идентификатор пользователя");
                    continue;
                }

                UserViewModel userInfo = vkRequestor.GetUserInfo(userId);
                if (userInfo == null) continue;

                string userName = userInfo.First_name + " " + userInfo.Last_name;
                List<WallItemViewModel> wallItems = vkRequestor.GetWallItems(userId);
                if (wallItems != null)
                {
                    string text = "";
                    foreach (var item in wallItems)
                    {
                        text += item.Text;
                    }
                    Dictionary<char, double> frequencies = FrequenciesCounter.GetFrequencies(text.ToLower());

                    string statisticsJSON = JsonConvert.SerializeObject(frequencies);
                    string message = $"{userName}, статистика для последних 5 постов: {statisticsJSON}";
                    vkRequestor.PostStatistics(message);
                    Console.WriteLine(message);
                }
                else
                    Console.WriteLine($"{userName}, нет записей или стена недоступна!");
            }
        }
    }
}
