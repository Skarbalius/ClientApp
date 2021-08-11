using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityFrameworkCore.Triggers;

namespace ClientApp.Models
{
    public class ClientContext : DbContext
    {
        public ClientContext(DbContextOptions<ClientContext> options)
            :base (options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Client> Clients { get; set; }

        //public override Task<int> SaveChangesAsync(CancellationToken cancellationToken=default(CancellationToken))
        //{
        //    return this.SaveChangesWithTriggersAsync(base.SaveChangesAsync, acceptAllChangesOnSuccess: true, cancellationToken: cancellationToken);
        //}
    }
}
