using System;
using System.Net;
using saxoOpenAPI_CLib;
using System.Threading.Tasks;
using System.Reflection;
using SaxoServiceGroupsModels;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading;
using Streaming.WebSocket.Samples;
using System.IO;

namespace SaxoOpenAPI_UI
{
    class Program
    {
        static void Main(string[] arg)
        {
            System.Console.WriteLine("test");
            string test = OtherFunctionality.OtherFunctionality.OAuthToken();
            System.Console.WriteLine(test);
            System.Console.Write("here");
        }
    }
}















































/*
       private static async void LoadAccountInfo()
        {
            var acc = await TradingProcessor.GET_AccountInfo();

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

        private static async void PlaceOrder()
        {
            var ord = await TradingProcessor.POST_Order();
            System.Console.WriteLine($"OrderID: {ord.OrderId}");
        }
*/