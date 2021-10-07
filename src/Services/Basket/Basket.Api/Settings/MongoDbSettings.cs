namespace Basket.Api.Settings
{
    public class MongoDbSettings
    {
        public const string ConfigurationName = "MongoDb";

        public string Server { get; set; }
        public string Database { get; set; }
        public string Collection { get; set; }
        public bool Seed { get; set; }
    }
}
