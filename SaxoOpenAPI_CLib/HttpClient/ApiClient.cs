using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace saxoOpenAPI_CLib
{
    public class ApiHelper
    {
        public static HttpClient ApiClient {get;set;}

        public static void Initialize()
        {
            string token = OtherFunctionality.OtherFunctionality.OAuthToken();
            ApiClient = new HttpClient();
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add( new MediaTypeWithQualityHeaderValue("application/json"));
            ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("BEARER",token);
            //ProductHeaderValue product = new ProductHeaderValue("Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.64 Safari/537.11",System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
            //ProductInfoHeaderValue UAgent = new ProductInfoHeaderValue(product);
            //ApiClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.64 Safari/537.11");
            ApiClient.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("appname", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()));
        }
    }
}