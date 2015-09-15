/*using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lisa.Breakpoint.WebApi
{
    public class TableStorage
    {
        public readonly CloudStorageAccount storageAccount = CloudStorageAccount.DevelopmentStorageAccount;
        public void getData() {
            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the table if it doesn't exist.
            CloudTable table = tableClient.GetTableReference("people");

            TableQuery<CustomerEntity> query = new TableQuery<CustomerEntity>();
        }
    }

    public class CustomerEntity : TableEntity
    {
        public CustomerEntity(string lastName, string firstName)
        {
            this.PartitionKey = lastName;
            this.RowKey = firstName;
        }

        public CustomerEntity() { }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }

}*/
