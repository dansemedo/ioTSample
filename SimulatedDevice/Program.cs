using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.IO;

namespace SimulatedDevice
{
    class Program
    {

        static DeviceClient deviceClient;
        static string iotHubUri = "<your_iot_HUB_URI>";
        static string deviceKey = "<your_device_key>";

        static void Main(string[] args)
        {
            Console.WriteLine("Simulated device\n");
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("myFirstDevice", deviceKey), TransportType.Mqtt);

            SendDeviceToCloudMessagesAsync();
            Console.ReadLine();
        }

        private static async void SendDeviceToCloudMessagesAsync()
        {
           // double avgWindSpeed = 10; // m/s
          //  Random rand = new Random();

            while (true)
            {
                //double currentWindSpeed = avgWindSpeed + rand.NextDouble() * 4 - 2;

                using (var fs = File.OpenRead(@"C:\output.csv"))
                using (var reader = new StreamReader(fs))
                {
                   // var coord0 = new List<strin();
                    //var coord1 = new List<string>();

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        var coord0 = (values[0]);
                        var coord1 = (values[1]);
                        var ratecsv = (values[4]);
                        var colorcsv = (values[6]);



                        //variavel para ser consumida no ioThub

                        var telemetryDataPoint = new
                        {
                            deviceId = "myFirstDevice",
                            latitude = coord0,
                            longitude = coord1,
                            rate = ratecsv,
                            color = colorcsv

                    };
                        var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                        var message = new Message(Encoding.ASCII.GetBytes(messageString));

                        await deviceClient.SendEventAsync(message);
                        Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

                       // await Task.Delay(1);

                    }
                }
            }
        }
    }
}
