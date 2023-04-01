namespace Post.Cmd.Infrastructure.Options
{
    public class MongoDbOption
    {
        public string ConnectionString { get; set; } = default!;
        public string Database { get; set; } = default!;
        public string Collection { get; set; } = default!;
    }
}
