namespace Standard.Customer.Domain.Config
{
    public class StorageConfig
    {
        public string seedDataPath { get; set; }
    }

    public class CosmosConfig { 
        public string EndpointUrl { get; set; }
        public string PrimaryKey { get; set; }
        public string DatabaseId { get; set; }
        public string ContainerId { get; set; }
    }
}
