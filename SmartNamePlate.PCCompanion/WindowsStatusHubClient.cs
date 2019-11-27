using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
namespace SmartNamePlate.PCCompanion
{
    public class WindowsStatusHubClient
    {
        private HubConnection hubConnection;
        public WindowsStatusHubClient(string hubUrl)
        {
            hubConnection = new HubConnectionBuilder()
                                    .WithUrl(hubUrl)
                                    //.WithAutomaticReconnect()
                                    .Build();

            hubConnection.Closed += async (error) =>
            {
                Logger.WriteLog("HubClient", "Closed");
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await hubConnection.StartAsync();
                Logger.WriteLog("HubClient", "Restarted");
            };
        }

        public async Task<bool> Connect()
        {
            bool isConnected = false;
            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                var newMessage = $"{user}: {message}";
                Logger.WriteLog("HubClient", string.Format("Received: {0}", newMessage));
            });

            try
            {
                await hubConnection.StartAsync();
                Logger.WriteLog("HubClient", "Hub Connection Success.");
                //messagesList.Items.Add("Connection started");
                //connectButton.IsEnabled = false;
                //sendButton.IsEnabled = true;
                isConnected = true;
            }
            catch (Exception ex)
            {
                isConnected = false;
                Logger.WriteLog("HubClient", "Hub Connection Failed");
                Logger.WriteLog("HubClient", ex.Message);
                Logger.WriteLog("HubClient", ex.StackTrace);
                //messagesList.Items.Add(ex.Message);
            }
            return isConnected;
        }

        public async Task<bool> Send(string message)
        {
            bool isSent = false;

            try
            {
                await hubConnection.InvokeAsync("SendMessage","SNP.PC.Companion", message);
                Logger.WriteLog("HubClient", "Message sent");
                Logger.WriteLog("HubClient", message);
                isSent = true;
            }
            catch (Exception ex)
            {
                //messagesList.Items.Add(ex.Message);
                Logger.WriteLog("HubClient", "Message sending failed");
                Logger.WriteLog("HubClient", ex.Message);
                Logger.WriteLog("HubClient", ex.StackTrace);
                isSent = false;
            }

            return isSent;
        }

        public async Task<bool> Disconnect()
        {
            bool returnVal = false;
            try
            {
                await hubConnection.StopAsync();
                returnVal = true;
            }
            catch (Exception ex)
            {
                returnVal = false;
                Logger.WriteLog("HubClient", "Disconnect failed");
                Logger.WriteLog("HubClient", ex.Message);
                Logger.WriteLog("HubClient", ex.StackTrace);
            }
            return returnVal;
        }
    }
}
