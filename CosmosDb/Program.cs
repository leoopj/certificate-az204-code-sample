using CosmosDb;
using Microsoft.Azure.Cosmos;

public static class Program
{
    private static readonly string endpoint = "https://<cosmosdb-name>.documents.azure.com:443/";
    private static readonly string key = "cosmosdb-key";
    private static readonly string databaseName = "database-name";
    private static readonly string collectionName = "collection-name";
    public static async void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        //Get container reference
        CosmosClient client = new CosmosClient(endpoint, key);
        Container container = client.GetContainer(databaseName, collectionName);

        //Create anonymous type
        Product productToCreate = new Product { Id = 1, Name = "Product 1" };

        //Upload item
        Product productCreated = await container.CreateItemAsync(productToCreate);
        Product productUpserted = await container.UpsertItemAsync(productCreated);


        //Get unique fields
        string id = Guid.NewGuid().ToString();
        PartitionKey partitionKey = new PartitionKey(id);

        //Read item using unique fields
        ItemResponse<Product> response = await container.ReadItemAsync<Product>(id, partitionKey);

        //Serialize response
        Product product = response.Resource;

        //Use SQL query language
        FeedIterator<Product> feedIterator = container.GetItemQueryIterator<Product>("SELECT * FROM products where diet = false");

        //Iterate over results
        while (feedIterator.HasMoreResults)
        {
            FeedResponse<Product> batch = await feedIterator.ReadNextAsync();
            foreach (Product item in batch)
            {
                Console.WriteLine(product.Name);
            }
        }
    }
}