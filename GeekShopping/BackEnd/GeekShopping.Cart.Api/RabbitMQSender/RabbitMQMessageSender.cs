﻿using GeekShopping.Cart.Api.Messages;
using GeekShopping.MessageBus;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace GeekShopping.Cart.Api.RabbitMQSender;

public class RabbitMQMessageSender : IRabbitMQMessageSender
{
    private readonly string _hostName;
    private readonly string _userName;
    private readonly string _password;
    private IConnection _connection;

    public RabbitMQMessageSender()
    {
        _hostName = "localhost";
        _userName = "guest";
        _password = "guest";
    }

    public void SendMessage(BaseMessage message, string queueName)
    {
        var factory = new ConnectionFactory
        {
            HostName = _hostName,
            UserName = _userName,
            Password = _password,
        };

        _connection = factory.CreateConnection();

        using var channel = _connection.CreateModel();

        channel.QueueDeclare(queue: queueName, false, false, arguments: null);

        byte[] body = GetMessageAsByteArray(message);

        channel.BasicPublish(
            exchange: "", 
            routingKey: queueName, 
            basicProperties: null, 
            body: body);
    }

    private static byte[] GetMessageAsByteArray(BaseMessage message)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };

        var json = JsonSerializer.Serialize((CheckoutHeaderVO)message, options);

        return Encoding.UTF8.GetBytes(json);
    }
}
