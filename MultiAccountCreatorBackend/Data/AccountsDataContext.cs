using Microsoft.EntityFrameworkCore;

namespace MultiAccountCreatorBackend.Data
{
    public class AccountsDataContext(DbContextOptions<AccountsDataContext> options) : DbContext(options)
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Service> Services { get; set; }
    }
}
