using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.Context
{
    public class OrderDbContextDesignFactory : IDesignTimeDbContextFactory<OrderDbContext>
    {
        public OrderDbContextDesignFactory()
        {

        }
        public OrderDbContext CreateDbContext(string[] args)
        {
            var connStr = "Server=.\\MSSQLSERVER1;Initial Catalog=Order;Integrated Security=true;TrustServerCertificate=True";

            var optionsBuilder = new DbContextOptionsBuilder<OrderDbContext>()
                .UseSqlServer(connStr);

            // uygulama çalışmadan context yaratmak için fake bir IMediator (NoMediator) verildi.
            return new OrderDbContext(optionsBuilder.Options, new NoMediator());
        }


        // Uygulama, domain eventleri MediatR ile fırlatıyor. (OrderDbContext'te)
        // migration gibi işlemlerde uygulama ayağa kalkmadığı için IMediator çözümlenemez, dolayısıyla DbContext oluşturamaz, migration gibi işlemler patlar. Bu sebeple NoMediator oluşturduk.
        // NoMediator, herhangi bir işlem yapmayacak sadece hata vermeyi engelleyecek.
        public class NoMediator : IMediator
        {
            public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default)
            {
                return AsyncEnumerable.Empty<TResponse>();
            }

            public IAsyncEnumerable<object?> CreateStream(object request, CancellationToken cancellationToken = default)
            {
                return AsyncEnumerable.Empty<object?>();
            }

            public Task Publish(object notification, CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
            }

            public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
            {
                return Task.CompletedTask;
            }

            public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
            {
                return Task.FromResult<TResponse>(default!);
            }

            public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
            {
                return Task.CompletedTask;
            }

            public Task<object?> Send(object request, CancellationToken cancellationToken = default)
            {
                return Task.FromResult<object?>(default);
            }
        }
    }
}
