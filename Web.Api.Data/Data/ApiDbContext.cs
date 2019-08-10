using Microsoft.EntityFrameworkCore;
using System.Linq;
using Web.Api.Core.Entities;
using Web.Api.Core.Interfaces;

namespace Web.Api.Infrastructure.Data
{
    public class ApiDbContext : DbContext
    {
        private readonly IEventDispatcher _dispatcher;

        public ApiDbContext(DbContextOptions<ApiDbContext> options, IEventDispatcher dispatcher)
            : base(options)
        {
            _dispatcher = dispatcher;
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Cart> Carts { get; set; }

        public override int SaveChanges()
        {
            int result = base.SaveChanges();

            // dispatch events only if save was successful
            var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
                .Select(e => e.Entity)
                .Where(e => e.Events.Any())
                .ToArray();

            foreach (var entity in entitiesWithEvents)
            {
                var events = entity.Events.ToArray();
                entity.Events.Clear();
                foreach (var domainEvent in events)
                {
                    _dispatcher.Dispatch(domainEvent);
                }
            }

            return result;
        }
    }
}
