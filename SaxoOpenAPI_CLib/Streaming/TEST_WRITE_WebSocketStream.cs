using System.Net.WebSockets;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using saxoOpenAPI_CLib;
using System.Net.Http;
using SaxoServiceGroupsModels;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;

namespace OurStreaming
{
    
    public sealed class WebSocketStream : System.IDisposable
    {

        public string _AccessToken { get; set; }
        public string _AccessScheme {get;set;}

        public string _ContextId { get; set; } /* ContextId: It can be at most 50 characters (a-z, A-Z, -, and _). */

        public string _ReferenceId { get; set; }
        public Uri _ReferenceDeleteUri {get;set;}

        public string _uriWebSocketStream { get; set; }
        public string _uriWebSocketOAuth {get;set;}
        public string _uriWebSocketSubscription {get;set;}

        public string _lastSeenMessageId {get;set;}
        public int _recievedMessages { get; set; }

        public bool _disposed {get;set;}

        public ClientWebSocket _ClientWebSocket {get;set;}
        public CancellationTokenSource _cts {get;set;}


        public WebSocketStream()
        {
            _AccessToken = OtherFunctionality.OtherFunctionality.OAuthToken();
            _AccessScheme = OtherFunctionality.OtherFunctionality.OAuthScheme();
            _disposed = false;
            _recievedMessages = 0;
        }

        public void SampleWebSocketStream ()
        {
            _ContextId = "SampleStream";
            _ReferenceId = "SampleSubscription";
            _uriWebSocketStream = "wss://streaming.saxobank.com/sim/openapi/streamingws/connect";
            _uriWebSocketOAuth = "https://streaming.saxobank.com/sim/openapi/streamingws/authorize";
            _uriWebSocketSubscription = ("https://gateway.saxobank.com/sim/openapi/trade/v1/prices/subscriptions");
        }
        public async Task StartWebSocket()
        {
            ThrowIfDisposed();

            Uri url;
            if (_recievedMessages > 0)
            {
                url = new Uri($"{_uriWebSocketStream}?contextid{_ContextId}&messageid={_lastSeenMessageId}");
            }
            else
            {
                url = new Uri($"{_uriWebSocketStream}?contextid{_ContextId}");
            }

            string OAuth = $"BEARER {_AccessToken}";
            _ClientWebSocket = new ClientWebSocket();
            _ClientWebSocket.Options.SetRequestHeader("Authorization",OAuth);

            try
            {
                await _ClientWebSocket.ConnectAsync(url,_cts.Token);
            }
            catch (TaskCanceledException)
            {
                return;
            }
            catch (Exception e)
            {
                var flattenedExceptionMessages = FlattenExceptionMessages(e);
				Console.WriteLine("WebSocket connection error.");
				Console.WriteLine(flattenedExceptionMessages);
				_cts.Cancel(false);
				return;
            }
        }

        private async Task ReauthorizeWhenNeeded(DateTime tokenExpiryTime, CancellationToken cts)
        {   
            var tokenRenewalDelay = tokenExpiryTime.AddSeconds(-60).Subtract(DateTime.Now);

            while (!_cts.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromHours(1));
            }
        }

        private async Task Reauthorize(string token)
        {
            Uri ReOAuth = new Uri($"{_uriWebSocketOAuth}?contextid={_ContextId}");
            using(var request = new HttpRequestMessage(HttpMethod.Put,ReOAuth))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("BEARER",_AccessToken);
                var response = await ApiHelper.ApiClient.SendAsync(request,_cts.Token);
                response.EnsureSuccessStatusCode();
                System.Console.WriteLine("ReAuthoerization is completed");
            }
        }

        private async Task DeleteSubscription(string[] referenceIds)
        {
            Uri Url = new Uri($"{_uriWebSocketSubscription}/{_ContextId}/{_ReferenceId}");

            using (var request = new HttpRequestMessage(HttpMethod.Delete,Url))
            {
                var response = await ApiHelper.ApiClient.SendAsync(request,_cts.Token);
                response.EnsureSuccessStatusCode();
                System.Console.WriteLine("Delete Subscription is succesful");
            }
        }

        public async Task CreateSubscription(string[] referenceIds)
        {

            ThrowIfDisposed();

            Uri Url = new Uri($"{_uriWebSocketSubscription}");

            SubscriptionModel content = new SubscriptionModel {
                ContextId = _ContextId,
                ReferenceId = _ReferenceId,
                Arguments = new Arguments {
                    AssetType = "FxSpot",
                    Uic = 21
                }
            };
            string jsonContent = JsonConvert.SerializeObject(content);

            using (var request = new HttpRequestMessage(HttpMethod.Post,Url))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(_AccessScheme,_AccessToken);
                request.Content = new StringContent(jsonContent,Encoding.UTF8,"application/json");
                
                try
                {         
                    HttpResponseMessage response = await ApiHelper.ApiClient.SendAsync(request,_cts.Token);
                    response.EnsureSuccessStatusCode();
                    string response_body = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Received snapshot:");
                    Console.WriteLine(JToken.Parse(response_body).ToString(Formatting.Indented));
                    Console.WriteLine();
                    _ReferenceDeleteUri = response.Headers.Location;
                }
                catch (TaskCanceledException)
                {
                    return;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("Subscription creation error.");
                    Console.WriteLine(e.Message);
                    _cts.Cancel(false);
                }

            }

        }










        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(WebSocketStream));
            }
        }
        
        public void Dispose()
        {
            _ClientWebSocket?.Dispose();
            _ClientWebSocket = null;
            _disposed = true;
        }

        private string FlattenExceptionMessages(Exception exp)
		{
			string message = string.Empty;
			Exception innerException = exp;

			do
			{
				message = message + Environment.NewLine + (string.IsNullOrEmpty(innerException.Message) ? string.Empty : innerException.Message);
				innerException = innerException.InnerException;
			}
			while (innerException != null);

			if (message.Contains("409"))
				message += Environment.NewLine + "ContextId cannot be reused. Please create a new one and try again.";

			if (message.Contains("429"))
				message += Environment.NewLine + "You have made too many request. Please wait and try again.";

			return message;
		}




    }



}