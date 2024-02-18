using GeekShopping.MessageBus;

namespace GeekShopping.Order.Api.RabbitMQSender;

public interface IRabbitMQMessageSender
{
    void SendMessage(BaseMessage message, string queueName);
}
