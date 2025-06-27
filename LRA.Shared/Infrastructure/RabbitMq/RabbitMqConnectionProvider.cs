//using LRA.Shared.Abstractions.Interfaces.RabbitMq;
//using LRA.Shared.Abstractions.Settings;
//using Microsoft.Extensions.Options;
//using RabbitMQ.Client;

//namespace LRA.Shared.Infrastructure.RabbitMq;

//public class RabbitMqConnectionProvider : IRabbitMqConnectionProvider, IDisposable
//{
//    private readonly IConnection _connection;

//    public RabbitMqConnectionProvider(IOptions<RabbitMQSettings> options)
//    {
//        var factory = new ConnectionFactory
//        {
//            HostName = options.Value.HostName,
//            Port = options.Value.Port,
//            UserName = options.Value.UserName,
//            Password = options.Value.Password,
//        };

//        _connection = factory.CreateConnectionAsync();
//    }

//    public IChannel CreateChannel()
//    {
//        return _connection.CreateChannelAsync(); 
//    }

//    public void Dispose()
//    {
//        _connection.Dispose();
//    }
//}
