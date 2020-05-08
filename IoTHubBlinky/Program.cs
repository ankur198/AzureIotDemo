using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace IoTHubBlinky
{
    public class Program
    {
        private static DeviceClient s_deviceClient;

        // Using the Azure CLI:
        // az iot hub device-identity show-connection-string --hub-name {YourIoTHubName} --device-id MyDotnetDevice --output table
        private readonly static string s_connectionString = "HostName=demoHub198.azure-devices.net;DeviceId=blinkyDotnet;SharedAccessKey=4Lz3/WcLZAUIlcS8Cpl3sTtKLgRk+Ia8Lr5iME5Voac=";

        private static async Task Main(string[] args)
        {
            s_deviceClient = DeviceClient.CreateFromConnectionString(s_connectionString);

            Console.WriteLine("Hello World!");

            RecieveMessage();

            while (true)
            {
                Console.ReadLine();
                Console.WriteLine("Send data");
                await sendData();
            }
        }

        private static async Task sendData()
        {
            var data = new
            {
                roomTemperature = new Random().NextDouble() * 45
            };

            var jsonString = JsonConvert.SerializeObject(data);

            var message = new Message(Encoding.ASCII.GetBytes(jsonString));

            await s_deviceClient.SendEventAsync(message);
        }

        private static async Task RecieveMessage()
        {
            while (true)
            {
                var message = await s_deviceClient.ReceiveAsync();

                Console.WriteLine($"Message recieved... {Encoding.ASCII.GetString(message.GetBytes())}");

                await s_deviceClient.CompleteAsync(message);
            }
        }
    }
}