using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Orderino.Repository.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Orderino.Repository
{
    public class Repository<T> where T : IEntity
    {
        private CosmosClient cosmosClient;
        private Database database;
        private Container container;

        private readonly string CosmosDbUrl;
        private readonly string CosmosDbKey;
        private readonly string CosmosDbName;
        private readonly string CosmosDbContainerName;

        public Repository(IConfigurationSection configurationSection)
        {
            CosmosDbUrl = configurationSection.GetSection("CosmosDbUrl").Value;
            CosmosDbKey = configurationSection.GetSection("CosmosDbKey").Value;
            CosmosDbName = configurationSection.GetSection("CosmosDbName").Value;
            CosmosDbContainerName = typeof(T).Name + "s";

            ConfigureContainer();
        }

        public async Task<T> AddAsync(T entity)
        {
            try
            {
                ItemResponse<T> item = await container.ReadItemAsync<T>(entity.Id, new PartitionKey(entity.Id));
                return item;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                var newEntity = await container.CreateItemAsync(entity, new PartitionKey(entity.Id));
                return newEntity;
            }
        }

        public async Task BulkAddAsync(List<T> entities)
        {
            var concurrentTasks = new List<Task>();

            foreach (var entity in entities)
            {
                concurrentTasks.Add(container.UpsertItemAsync(entity));
            }

            await Task.WhenAll(concurrentTasks);
        }

        public async Task<List<T>> QueryAllItemsAsync()
        {
            var sqlQuery = "SELECT * FROM c";

            return await QueryAllItemsAsync(sqlQuery);
        }

        public async Task<T> QueryItemAsync(T entity)
        {
            return await QueryItemAsync(entity.Id);
        }

        public async Task<T> QueryItemAsync(string entityId)
        {
            try 
            {
                return await container.ReadItemAsync<T>(entityId, new PartitionKey(entityId));
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                var entityType = typeof(T);
                var newEntity = (T) Activator.CreateInstance(Type.GetType(entityType.ToString()));
                newEntity.Id = entityId;

                return await AddAsync(newEntity);
            }            
        }

        public async Task<List<T>> QueryByNameSearch(string search)
        {
            var sqlQuery = $"SELECT * FROM c WHERE c.Name LIKE '%{search}%'";

            return await QueryAllItemsAsync(sqlQuery);
        }

        private async Task<List<T>> QueryAllItemsAsync(string sqlQuery)
        {
            QueryDefinition queryDefinition = new QueryDefinition(sqlQuery);
            FeedIterator<T> queryResultSetIterator = container.GetItemQueryIterator<T>(queryDefinition);

            var entities = new List<T>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<T> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (T resultEntity in currentResultSet)
                {
                    entities.Add(resultEntity);
                }
            }

            return entities;
        }


        public async Task Update(T entity)
        {            
            await container.ReplaceItemAsync<T>(entity, entity.Id, new PartitionKey(entity.Id));
        }

        public async Task Delete(T entity)
        {
            await container.DeleteItemAsync<T>(entity.Id, new PartitionKey(entity.Id));
        }

        private void ConfigureContainer()
        {
            var options = new CosmosClientOptions() { AllowBulkExecution = true, MaxRetryAttemptsOnRateLimitedRequests = 100 };
            cosmosClient = new CosmosClient(CosmosDbUrl, CosmosDbKey, options);
            database = cosmosClient.GetDatabase(CosmosDbName);
            container = database.GetContainer(CosmosDbContainerName);
        }
    }    
}
