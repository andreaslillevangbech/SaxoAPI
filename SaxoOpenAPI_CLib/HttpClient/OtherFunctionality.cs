using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using saxoOpenAPI_CLib;
using SaxoServiceGroupsModels;

namespace OtherFunctionality
{
    public static class OtherFunctionality
    {
        public static string OAuthToken()
        {
            return System.IO.File.ReadAllText(@"C:\Users\alexa\Desktop\alillevangbech\SaxoOpenAPI\token.txt");
        }
        public static string OAuthScheme()
        {
            return "BEARER";
        }
        public static async void AccountInfo()
        {
            string url = "https://gateway.saxobank.com/sim/openapi/port/v1/users/me";
            AccountInfoModel acc = await TradingProcessor.GetRequest<AccountInfoModel>(url);

            System.Console.WriteLine($"UserKey: {acc.UserKey}");
            System.Console.WriteLine($"ClientKey: {acc.ClientKey}");
            System.Console.WriteLine($"Name: {acc.Name}");
            System.Console.WriteLine($"LastLoginStatus: {acc.LastLoginStatus}");
            System.Console.WriteLine($"UserId: {acc.UserId}");
            
            System.Console.WriteLine("#######");
            foreach (var item in acc.LegalAssetTypes)
            {
                System.Console.WriteLine($"LegalAssetType: {item}");
            }
            System.Console.WriteLine("#######");

        }

        public static async void OrderInfo()
        {
            string url = "https://gateway.saxobank.com/sim/openapi/trade/v2/orders";

            OrderParameters content = new OrderParameters { 
                Uic = 16,
                BuySell = "Buy",
                AssetType = "FxSpot",
                Amount = 100000,
                OrderPrice = 7,
                OrderType = "Limit",
                OrderRelation = "StandAlone",
                ManuelOrder = true,
                OrderDuration = new OrderDuration { 
                    DurationType = "GoodTillCancel"
                },
                AccountKey = "GvKzTZwRymwaKZCZkl3|1g=="
            };

            // Serialize our concrete class into a JSON String
            String stringContent = JsonConvert.SerializeObject(content);
            StringContent httpContent = new StringContent(stringContent, Encoding.UTF8, "application/json");

            Order ord = await TradingProcessor.PostRequest<Order>(url,httpContent);
            System.Console.WriteLine($"OrderID: {ord.OrderId}");
        }
    }
}
     
     
  
