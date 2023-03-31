using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Cmd.Infrastructure.Options
{
    public class MongoDbOptions
    {
        public string ConnectionString { get; set; } = default!;
        public string Database { get; set; } = default!;
        public string Collection { get; set; } = default!;
    }
}
