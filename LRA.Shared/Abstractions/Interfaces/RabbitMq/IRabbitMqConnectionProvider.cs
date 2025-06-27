using RabbitMQ.Client;

namespace LRA.Shared.Abstractions.Interfaces.RabbitMq;

public interface IRabbitMqConnectionProvider
{
    IChannel CreateChannel();
}
