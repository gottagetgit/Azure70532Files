using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Queue;

//** Code provided as-is, without support, warranty or guarantee **//
//** Scott Duffy @ www.softwarearchitect.ca **//

namespace AzureTables
{
    class Program
    {
        static void Main(string[] args)
        {
            string storageconnection =
    System.Configuration.ConfigurationManager.AppSettings.Get("StorageConnectionString");
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageconnection);

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("FirstTestTable");
            table.CreateIfNotExists();




            //CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            //CloudQueue myqueue = queueClient.GetQueueReference("queue");
            //myqueue.CreateIfNotExists();

            //CloudQueueMessage newmessage = new CloudQueueMessage("This is the fifth message!");
            //myqueue.AddMessage(newmessage);

            //CloudQueueMessage oldmessage = myqueue.GetMessage();
            //Console.WriteLine(oldmessage.AsString);



            TableBatchOperation tbo = new TableBatchOperation();
            CarEntity newcar = new CarEntity(124, 2012, "BMW", "X1", "Black");
            tbo.Insert(newcar);
            newcar = new CarEntity(125, 2012, "Honda", "Civic", "Yellow");
            tbo.Insert(newcar);
            newcar = new CarEntity(126, 2013, "BMW", "X1", "White");
            tbo.Insert(newcar);
            newcar = new CarEntity(127, 2014, "BMW", "X1", "Silver");
            tbo.Insert(newcar);
            table.ExecuteBatch(tbo);





            //TableOperation retrieve = TableOperation.Retrieve<CarEntity>("car", "123");
            //TableResult result = table.Execute(retrieve);

            //if (result.Result == null)
            //{
            //    Console.WriteLine("not found");
            //}
            //else
            //{
            //    Console.WriteLine("found the car " + ((CarEntity)result.Result).Make + " " + ((CarEntity)result.Result).Model);
            //}

            TableQuery<CarEntity> carquery = new TableQuery<CarEntity>().Take(4);
            foreach (CarEntity thiscar in table.ExecuteQuery(carquery))
            {
                Console.WriteLine(thiscar.Year.ToString() + " " + thiscar.Make + " " + thiscar.Model + " " + thiscar.Color);
            }

            Console.ReadKey();

        }
    }

    public class CarEntity : TableEntity
    {
        public CarEntity(int ID, int year, string make, string model, string color)
        {
            this.UniqueID = ID;
            this.Year = year;
            this.Make = make;
            this.Model = model;
            this.Color = color;
            this.PartitionKey = "car";
            this.RowKey = ID.ToString();
        }

        public CarEntity() { }

        public int UniqueID { get; set; }

        public int Year { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Color { get; set; }

    }
}
