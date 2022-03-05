using Newtonsoft.Json;
using Standard.Customer.Application;
using Standard.Customer.Domain;
using Standard.Customer.Domain.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Extensions.Logging;
using Standard.Customer.Domain.Config;
using System.Text;

namespace Standard.Customer.Infrastructure
{
    public class CustomerService : IRepository<CustomerEntity>, IMigrationService
    {
        private readonly StorageConfig storageConfig;
        private readonly CosmosConfig cosmosConfig;
        private readonly ILogger<CustomerService> logger;        

        private CosmosClient cosmosClient;
        private Database database;

        // The container we will create.
        private Container customerContainer;
        public CustomerService(StorageConfig storageConfig, CosmosConfig cosmosConfig, ILogger<CustomerService> logger)
        {
            this.cosmosConfig = cosmosConfig;
            this.storageConfig = storageConfig;
            this.cosmosClient = new CosmosClient(this.cosmosConfig.EndpointUrl, this.cosmosConfig.PrimaryKey);
            this.database = cosmosClient.GetDatabase(this.cosmosConfig.DatabaseId);
            this.customerContainer = database.GetContainer(this.cosmosConfig.ContainerId);
            this.logger = logger;
        }
        public async Task<string> Add(CustomerEntity entity, CreateType creatType)
        {
            try
            {
                ModelTransform(entity, creatType);

                logger.LogInformation($"Saving data to CosmosDB");

                await this.customerContainer.CreateItemAsync<CustomerEntity>(entity);

                logger.LogInformation($"Customer data with Id : {entity.id} saved");

                return entity.id;
            }
            catch (CosmosException cox) when (cox.StatusCode == HttpStatusCode.Conflict)
            {
                logger.LogError($"There was a problem with CosmosDB saving. Please check : {cox.Message}");
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception has been caught, please check : {ex.Message}");
            }

            return string.Empty;
        }
        public async Task<bool> Migrate()
        {
            logger.LogInformation("Migration starting");
            if (!Directory.Exists(storageConfig.seedDataPath)) throw new Exception("Can't find path");

            using (StreamReader streamReader = new StreamReader(storageConfig.seedDataPath))
            {
                logger.LogInformation($"Reading the json file from {storageConfig.seedDataPath} starting");

                string json = streamReader.ReadToEnd();
                
                if(json.Length == 0) return false;

                List<CustomerEntity> customers = JsonConvert.DeserializeObject<List<CustomerEntity>>(json);
                foreach (var customer in customers)
                {
                    await Add(customer, CreateType.Migrate);
                }
            }

            return true;
        }
        public async Task<CustomerEntity> Update(CustomerEntity entity)
        {
            try 
            {
                var customer = await Get(entity.id);
                if (customer != null)
                {
                    logger.LogInformation($"Updating data to CosmosDB");

                    ItemResponse<CustomerEntity> response = await customerContainer.ReplaceItemAsync(
                    partitionKey: new PartitionKey(entity.id),
                    id: entity.id,
                    item: entity);

                    logger.LogInformation($"Customer data with Id : {entity.id} updated");

                    return response.Resource;
                }
            }
            catch (CosmosException cox) when (cox.StatusCode == HttpStatusCode.Conflict)
            {
                logger.LogError($"There was a problem with CosmosDB updating. Please check : {cox.Message}");
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception has been caught, please check : {ex.Message}");
            }

            return entity;
        }
        public async Task<CustomerEntity> Get(string id)
        {
            var customerSingleRequest = await customerContainer.ReadItemAsync<CustomerEntity>(id, new PartitionKey(id));
            return customerSingleRequest;
        }
        public async Task<IEnumerable<CustomerEntity>> GetMany(string searchKeyword, int ? page, int pageSize = 20)
        {
            string query = BuildSelectQuery(searchKeyword, page, pageSize);
            QueryDefinition queryDefinition = new QueryDefinition(query);
            List<CustomerEntity> customers = new List<CustomerEntity>();
            FeedIterator<CustomerEntity> queryResultSetIterator = this.customerContainer.GetItemQueryIterator<CustomerEntity>(queryDefinition);
            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<CustomerEntity> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (CustomerEntity customer in currentResultSet)
                {
                    customers.Add(customer);
                }
            }
            return customers;
        }
        public async Task<bool> Delete(string id)
        {
            try
            {
                logger.LogInformation($"Deleting data to CosmosDB");

                ItemResponse<CustomerEntity> response = await customerContainer.DeleteItemAsync<CustomerEntity>(partitionKey: new PartitionKey(id), id: id);

                logger.LogInformation($"Customer data with Id : {id} deleted");

                return response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent;
            }
            catch (CosmosException cox) when (cox.StatusCode == HttpStatusCode.Conflict)
            {
                logger.LogError($"There was a problem with CosmosDB deleting. Please check : {cox.Message}");
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception has been caught, please check : {ex.Message}");
            }

            return false;
        }
        private void ModelTransform(CustomerEntity entity, CreateType createType)
        {
            // unique id will represent the id from json if it's migration
            entity.uniqueid = (createType == CreateType.Migrate ? entity.id : entity.uniqueid);
            // we will use the id property to be the unique identifier on cosmos
            entity.id = Guid.NewGuid().ToString(); 
        }
        private string BuildSelectQuery(string searchKeyword, int? page, int pageSize = 20) {
            string whereQuery = !string.IsNullOrEmpty(searchKeyword) ? $"WHERE c.last_name LIKE '%{searchKeyword}%' OR c.first_name LIKE '%{searchKeyword}%' OR c.email LIKE '%{searchKeyword}%'" : string.Empty;   
            string query = @$"SELECT * FROM c { whereQuery } ORDER BY c.uniqueid OFFSET {((page ?? 1) - 1) * pageSize} LIMIT {pageSize}";
            return query;
        }
    }
}
