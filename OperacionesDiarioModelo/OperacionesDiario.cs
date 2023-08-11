using OperacionesDiarioDB;
using Newtonsoft.Json;
using System.Collections.Generic;
using MongoDB.Bson;

namespace OperacionesDiario.Modelo
{
    public class OperacionDiario
    {
        public string _id { get; set; } = string.Empty;
        public float RD_Unix { get; set; } = 0;
        public string RD_Origen { get; set; } = string.Empty;
        public string RD_Fecha { get; set; } = string.Empty;
        public float RD_Factor { get; set; } = 0;
        public float RD_Ventas { get; set; } = 0;
        public float RD_Costos { get; set; } = 0;
        public float RD_Utilidad { get; set; } = 0;
        public float RD_Porc_Util { get; set; } = 0;
        public float RD_Fact_Emit { get; set; } = 0;
        public float RD_Art_Venta { get; set; } = 0;
        public float RD_Monto_Venta { get; set; } = 0;
        public float RD_Cant_Fallas { get; set; } = 0;
        public float RD_Monto_Fallas { get; set; } = 0;


        public List<OperacionDiario> Get()
        {

            List<string> x = OperacionDiarioDB.GetOperaciones();
            List<OperacionDiario> Data = new();
            foreach (string item in x)
            {
                Data.Add(JsonConvert.DeserializeObject<OperacionDiario>(item));
            }

            return Data;

        }

        public void Set(List<OperacionDiario> data)
        {
            OperacionDiarioDB.DeleteOperaciones(data[0].RD_Fecha, data[0].RD_Origen);

            List<BsonDocument> docs = new List<BsonDocument>();

            foreach (OperacionDiario reg in data)
            {

                DateTime dt = DateTime.ParseExact(reg.RD_Fecha + " 00:00:00,000", "yyyy-MM-dd HH:mm:ss,fff",
                                       System.Globalization.CultureInfo.InvariantCulture);

                reg.RD_Unix = (int)dt.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                reg._id = reg.RD_Origen + reg.RD_Fecha;

                docs.Add(reg.ToBsonDocument());
            }


            OperacionDiarioDB.SetOperaciones(docs);


        }




    }
}