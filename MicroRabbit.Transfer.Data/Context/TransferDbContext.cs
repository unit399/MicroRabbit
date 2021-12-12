
using MicroRabbit.Transfer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroRabbit.Transfer.Data.Context
{
    public class TransferDbContext : DbContext
    {
        public TransferDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
#if DEBUG
            Database.EnsureCreated();
#endif
        }

        public DbSet<TransferLog> TransferLogs { get; set; }        
    }
}