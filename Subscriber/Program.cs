// See https://aka.ms/new-console-template for more information
using MQTTnet;
using MQTTnet.Client;
using System.Text;

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
    var topic = new MqttTopicFilterBuilder()
        .WithTopic("SampleMessage")
        .Build();
    await client.SubscribeAsync(topic);
    //return;
}

client.DisconnectedAsync += Client_Disconnected;
async Task Client_Disconnected(MqttClientDisconnectedEventArgs arg)
{
    Console.WriteLine("Disconnected..!");
    //return null;
}

client.ApplicationMessageReceivedAsync += Client_MessageReceived;
async Task Client_MessageReceived(MqttApplicationMessageReceivedEventArgs arg)
{
    Console.WriteLine("Message Received: " + Encoding.UTF8.GetString(arg.ApplicationMessage.Payload));
}

await client.ConnectAsync(options);

Console.ReadLine();

await client.DisconnectAsync();
