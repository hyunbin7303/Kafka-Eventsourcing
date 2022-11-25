using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Query.Infrastructure.DataAccess
{
    public class DatabaseContextFactory
    {
        private readonly Action<DbContextOptionsBuilder> _configDbContext;
        public DatabaseContextFactory(Action<DbContextOptionsBuilder> configDbContext)
        {
            _configDbContext = configDbContext;
        }
        public DatabaseContext CreateDbContext()
        {
            DbContextOptionsBuilder<DatabaseContext> options = new();
            _configDbContext(options);
            return new DatabaseContext(options.Options);
        }
    }
}
