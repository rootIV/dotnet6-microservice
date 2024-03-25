﻿using GeekShopping.Order.Api.Messages;
using GeekShopping.Order.Api.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GeekShopping.Order.Api.MessageConsumer;

public class RabbitMQPaymentConsumer : BackgroundService
{
    private readonly OrderRepository _orderRepository;
    private readonly IConnection _connection;
    private IModel _channel;

    public RabbitMQPaymentConsumer(OrderRepository orderRepository)
    {
        _orderRepository = orderRepository;

        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest",
        };

        _connection = factory.CreateConnection();

        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: "orderpaymentresultqueue", false, false, false, arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (chanel, evt) =>
        {
            var content = Encoding.UTF8.GetString(evt.Body.ToArray());

            UpdatePaymentResultVO vo = JsonSerializer.Deserialize<UpdatePaymentResultVO>(content);

            UpdatePaymentStatus(vo).GetAwaiter().GetResult();

            _channel.BasicAck(evt.DeliveryTag, false);
        };

        _channel.BasicConsume("orderpaymentresultqueue", false, consumer);

        return Task.CompletedTask;
    }

    private async Task UpdatePaymentStatus(UpdatePaymentResultVO vo)
    {
        try
        {
            await _orderRepository.UpdateOrderPaymentStatus(vo.OrderId, vo.Status);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
