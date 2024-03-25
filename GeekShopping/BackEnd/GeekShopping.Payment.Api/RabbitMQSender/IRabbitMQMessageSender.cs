using GeekShopping.MessageBus;

namespace GeekShopping.Payment.Api.RabbitMQSender;

public interface IRabbitMQMessageSender
{
    void SendMessage(BaseMessage message, string queueName);
}
