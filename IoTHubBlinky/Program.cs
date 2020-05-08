using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Device.Gpio;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IoTHubBlinky
{
    public class Program
    {
        private static DeviceClient s_deviceClient;

        // Using the Azure CLI:
        // az iot hub device-identity show-connection-string --hub-name {YourIoTHubName} --device-id MyDotnetDevice --output table
        private readonly static string s_connectionString = "HostName=demoHub198.azure-devices.net;DeviceId=blinkyDotnet;SharedAccessKey=4Lz3/WcLZAUIlcS8Cpl3sTtKLgRk+Ia8Lr5iME5Voac=";

        private static GpioController gpioController;
        private static readonly int ledPin = 8;

        private static async Task Main(string[] args)
        {
            s_deviceClient = DeviceClient.CreateFromConnectionString(s_connectionString);

            try
            {
                gpioController = new GpioController();
                gpioController.OpenPin(ledPin, PinMode.Output);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error connecting to gpio controller. " + e.Message);
            }

            Console.WriteLine("Device ready");

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

                var messageString = Encoding.ASCII.GetString(message.GetBytes());

                Console.WriteLine($"Message recieved... {messageString}");

                if (gpioController != null)
                {
                    switch (messageString)
                    {
                        case "on":
                            turnOn();
                            break;

                        case "off":
                            turnOff();
                            break;

                        case "disco":
                            toggleDisco();
                            break;

                        default:
                            Console.WriteLine("No actions specified");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("GpioController not found...Can't perform actions.");
                }

                await s_deviceClient.CompleteAsync(message);
            }
        }

        private static void turnOn()
        {
            gpioController.Write(ledPin, PinValue.High);
        }

        private static void turnOff()
        {
            gpioController.Write(ledPin, PinValue.Low);
        }

        private static bool isDiscoStarted = false;

        private static void toggleDisco()
        {
            if (isDiscoStarted)
            {
                isDiscoStarted = false;
            }
            else
            {
                isDiscoStarted = true;
                startDisco();
            }
        }

        private static async Task startDisco()
        {
            while (isDiscoStarted)
            {
                gpioController.Write(ledPin, PinValue.High);
                Thread.Sleep(1000);
                gpioController.Write(ledPin, PinValue.Low);
                Thread.Sleep(1000);
            }
        }
    }
}