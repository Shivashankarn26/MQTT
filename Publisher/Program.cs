// See https://aka.ms/new-console-template for more information
using MQTTnet;
using MQTTnet.Client;

var factory = new MqttFactory();
var client = factory.CreateMqttClient();
var options = new MqttClientOptionsBuilder()
    .WithClientId(Guid.NewGuid().ToString())
    .WithTcpServer("test.mosquitto.org", 1883)
    .WithCleanSession()
    .Build();

client.ConnectedAsync += Client_Connected;

async Task Client_Connected(MqttClientConnectedEventArgs arg)
{
    Console.WriteLine("Connected..!");
    //return;
}

client.DisconnectedAsync += Client_Disconnected;
async Task Client_Disconnected(MqttClientDisconnectedEventArgs arg)
{
    Console.WriteLine("Disconnected..!");
    //return null;
}

await client.ConnectAsync(options);

Console.WriteLine("Press any key");
Console.ReadLine();

string messagePayload = "Hello";
var message = new MqttApplicationMessageBuilder()
    .WithPayload(messagePayload)
    .WithTopic("SampleMessage")
    .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
    .Build();

if (client.IsConnected)
{
    await client.PublishAsync(message);
}

await client.DisconnectAsync();