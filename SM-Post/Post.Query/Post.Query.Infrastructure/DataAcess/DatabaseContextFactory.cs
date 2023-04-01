using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Query.Infrastructure.DataAcess
{
    public class DatabaseContextFactory
    {
        private readonly Action<DbContextOptionsBuilder> _action;

        public DatabaseContextFactory(Action<DbContextOptionsBuilder> action)
        {
            _action = action;
        }

        public DatabaseContext CreateDbContext()
        {
            DbContextOptionsBuilder<DatabaseContext> options = new();
            _action(options);

            return new DatabaseContext(options.Options);
        }
    }
}
