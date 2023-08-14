using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.Json;
using ZstdSharp.Unsafe;

namespace OperacionesDiarioDB
{
    public static class OperacionDiarioDB
    {
        private static string _cs = "";

        public static List<string> GetOperaciones()
        {



            MongoClient dbClient = new MongoClient(_cs);

            var database = dbClient.GetDatabase("farmadata");
            var collection = database.GetCollection<BsonDocument>("OperacionesDiario");


            DateTime dt = DateTime.ParseExact(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd") + " 00:00:00,000", "yyyy-MM-dd HH:mm:ss,fff",
                       System.Globalization.CultureInfo.InvariantCulture);

            int unix = (int)dt.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

            var findFilter = Builders<BsonDocument>.Filter.Eq("RD_Unix", new BsonDocument { { "$gt", unix - 1 } });

            var Documentos = collection.Find(findFilter).ToList();

            List<string> Resultados = new();

            foreach (BsonDocument doc in Documentos)
            {
                Resultados.Add(doc.ToString());
            }

            return Resultados;

        }

        public static void SetOperaciones(List<BsonDocument> data)
        {


            MongoClient dbClient = new MongoClient(_cs);

            var database = dbClient.GetDatabase("farmadata");
            var collection = database.GetCollection<BsonDocument>("OperacionesDiario");

            UpdateOptions updateOptions = new UpdateOptions();
            updateOptions.IsUpsert = true;

            collection.InsertMany(data);


        }

        public static void DeleteOperaciones(string fecha, string origen)
        {


            MongoClient dbClient = new MongoClient(_cs);

            var database = dbClient.GetDatabase("farmadata");
            var collection = database.GetCollection<BsonDocument>("OperacionesDiario");


            DateTime dt = DateTime.ParseExact(fecha + " 00:00:00,000", "yyyy-MM-dd HH:mm:ss,fff",
                       System.Globalization.CultureInfo.InvariantCulture);

            int unix = (int)dt.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

            var deleteFilter = Builders<BsonDocument>.Filter;//.Eq("RD_Unix", new BsonDocument { { "$gt", unix - 1 } });
            
            //Opcion de produccion
            var filter = deleteFilter.Gte("RD_Unix", unix -1) & deleteFilter.Eq("RD_Origen", origen);
            
            //Opcion de limpieza de datos
            //var filter = deleteFilter.Gte("RD_Unix", -1);

            collection.DeleteMany(filter);


        }


    }
}