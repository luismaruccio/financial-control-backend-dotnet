using FinancialControl.Infra.Bus.Interfaces.Connections;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FinancialControl.Infra.Bus.Connections
{
    public class RabbitMQConnection(IConnectionFactory connectionFactory, ILogger<RabbitMQConnection> logger) : IRabbitMQConnection, IDisposable
    {
        private readonly IConnectionFactory _connectionFactory = connectionFactory;
        private readonly ILogger<RabbitMQConnection> _logger = logger;
        private IConnection? _connection;
        private bool _disposed;

        public bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            return _connection!.CreateModel();
        }

        public void TryConnect()
        {
            _logger.LogInformation("RabbitMQ Client is trying to connect");

            lock (this)
            {
                _connection = _connectionFactory.CreateConnection();

                if (IsConnected)
                {
                    _connection.ConnectionShutdown += OnConnectionShutdown;
                    _connection.CallbackException += OnCallbackException;
                    _connection.ConnectionBlocked += OnConnectionBlocked;

                    _logger.LogInformation("RabbitMQ Client acquired a persistent connection to '{HostName}' and is subscribed to failure events", _connection.Endpoint.HostName);
                }
                else
                {
                    _logger.LogCritical("FATAL ERROR: RabbitMQ connections could not be created and opened");
                }
            }
        }

        private void OnConnectionBlocked(object? sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;

            _logger.LogWarning("A RabbitMQ connection is shutdown. Trying to re-connect...");

            TryConnect();
        }

        private void OnCallbackException(object? sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;

            _logger.LogWarning("A RabbitMQ connection throw exception. Trying to re-connect...");

            TryConnect();
        }

        private void OnConnectionShutdown(object? sender, ShutdownEventArgs reason)
        {
            if (_disposed) return;

            _logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");

            TryConnect();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _connection?.Dispose();
            }

            _disposed = true;
        }
    }
}
