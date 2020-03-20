﻿using System;
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

/*
namespace SaxoOpenAPI_UI
{
    class Program
    {
        static void Main(string[] arg)
        {
            System.Console.WriteLine("Stream:");
            //string test = OtherFunctionality.OtherFunctionality.OAuthToken();
            var cts = new CancellationTokenSource();
            RunSample(cts);
            //System.Console.WriteLine(test);
        }
    }
}
*/


namespace Streaming.WebSocket.Samples
{
	/// <summary>
	/// Sample runner.
	/// </summary>
	class Program
	{
		private static Action _closeConnectionAction;
		static void Main(string[] args)
		{
			Console.WriteLine("Press ESC to disconnect.");

			var taskKeys = new Task(ListenForEscape);

			var cts = new CancellationTokenSource();
			var taskRunSample = new Task(async () => { await RunSample(cts); });

			taskKeys.Start();
			try
			{
				taskRunSample.Start();
			}
			catch (TaskCanceledException)
			{
				taskRunSample.Dispose();
			}

			Task.WaitAll(taskKeys);
			
			cts.Cancel();
			taskRunSample.Wait(cts.Token);
			Console.WriteLine("Press ESC to close window.");
			Console.ReadKey();
		}

		/// <summary>
		/// Run the sample and set up a callback to handle Web Socket clonse on ESC.
		/// </summary>
		/// <param name="cts"></param>
		private static async Task RunSample(CancellationTokenSource cts)
		{
			var sample = new WebSocketSample();
			async void CloseConnectionCallback() => await sample.StopWebSocket();
			_closeConnectionAction = CloseConnectionCallback;

			try
			{
				await sample.RunSample(cts);
			}
			finally
			{
				sample.Dispose();
				cts.Cancel();
			}

			Console.WriteLine("Stopped sample.");
		}

		#region Input handling
		/// <summary>
		/// Listen for keyboard input from user.
		/// </summary>
		private static void ListenForEscape()
		{
			ConsoleKeyInfo key = new ConsoleKeyInfo();

			while (!Console.KeyAvailable)
			{
				key = Console.ReadKey(true);
				if (key.Key == ConsoleKey.Escape)
				{
					_closeConnectionAction?.Invoke();
					break;
				}
			}
		}
		#endregion 
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