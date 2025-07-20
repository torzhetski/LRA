using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LRA.Shared.Abstractions.Settings.RabbitMq;

public class RabbitMqConsumerSettings
{
    public required string QueueName { get; set; }
}
