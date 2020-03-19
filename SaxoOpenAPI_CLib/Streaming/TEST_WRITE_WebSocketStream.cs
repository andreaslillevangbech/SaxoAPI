using System.Net.WebSockets;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace OurStreaming
{
    
    public sealed class WebSocketStream : System.IDisposable
    {
        public string _AccessToken { get; set; }
        public string _ContextId { get; set; } /* ContextId: It can be at most 50 characters (a-z, A-Z, -, and _). */
        public string _ReferenceId { get; set; }
        public string _uriWebSocketStream { get; set; }
        public string _uriWebSocketOAuth {get;set;}
        public string _lastSeenMessageId {get;set;}

        public bool _disposed {get;set;}
        public int _recievedMessages { get; set; }
    

        public List<string> _uriWebSocketSubscription {get;set;}
        public ClientWebSocket _ClientWebSocket {get;set;}
        public CancellationTokenSource _cts {get;set;}


        public WebSocketStream()
        {
            _AccessToken = OtherFunctionality.OtherFunctionality.OAuthToken();
            _disposed = false;
            _recievedMessages = 0;
        }

        public void SampleWebSocketStream ()
        {
            _ContextId = "SampleStream";
            _ReferenceId = "SampleSubscription";
            _uriWebSocketStream = "wss://streaming.saxobank.com/sim/openapi/streamingws/connect";
            _uriWebSocketOAuth = "https://streaming.saxobank.com/sim/openapi/streamingws/authorize";
            _uriWebSocketSubscription.Add("https://gateway.saxobank.com/sim/openapi/trade/v1/prices/subscriptions");
        }
        public void StartWebSocket()
        {
            ThrowIfDisposed();

            Uri url;
            if (_recievedMessages > 0)
            {
                url = new Uri($"{_uriWebSocketStream}?contextid")
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






    }



}