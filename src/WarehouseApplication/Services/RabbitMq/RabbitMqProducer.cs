using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Warehouse.Services.RabbitMq
{
    public interface IRabbitMQProducer
    {
        void SendMessage<T>(T message);
    }

    public class RabbitMQProducer : IRabbitMQProducer
    {
        private readonly IConnectionFactory connectionFactory;

        public RabbitMQProducer(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public void SendMessage<T>(T message)
        {
            using var connection = connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: "MyQueue",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "",
                       routingKey: "MyQueue",
                       basicProperties: null,
                       body: body);
        }
    }
}
