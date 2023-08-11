using BASE;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using RestSharp;

namespace ServicioOperacionesDiario
{
    public class OperacionDiario
    {
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




    }

    public class Comunicacion
    {

        public List<OperacionDiario> Registros { get; set; } = new();
        public Comunicacion()
        {
            GetRegistros();
            SendRegistros();
        }

        void GetRegistros()
        {

            Data db = new Data();
            SqlParameter[] parameters = new SqlParameter[0];
            DataTable DT = db.CallDBList("GFO_RESUMEN_DIARIO", parameters);

            if (DT.Rows.Count > 0)
            {
                foreach (DataRow item in DT.Rows)
                {
                    OperacionDiario Reg = new OperacionDiario();

                    Reg.RD_Origen = item["RD_Origen"].ToString();
                    Reg.RD_Fecha = FormatFecha(item["RD_Anio"].ToString().Trim(), item["RD_Mes"].ToString().Trim(), item["RD_Dia"].ToString().Trim());
                    Reg.RD_Factor = (string.IsNullOrEmpty(item["RD_Factor"].ToString().Trim())) ? 0 : float.Parse(item["RD_Factor"].ToString());
                    Reg.RD_Ventas = (string.IsNullOrEmpty(item["RD_Ventas"].ToString().Trim())) ? 0 : float.Parse(item["RD_Ventas"].ToString());
                    Reg.RD_Costos = (string.IsNullOrEmpty(item["RD_Costos"].ToString().Trim())) ? 0 : float.Parse(item["RD_Costos"].ToString());
                    Reg.RD_Utilidad = (string.IsNullOrEmpty(item["RD_Utilidad"].ToString().Trim())) ? 0 : float.Parse(item["RD_Utilidad"].ToString());
                    Reg.RD_Porc_Util = (string.IsNullOrEmpty(item["RD_Porc_Util"].ToString().Trim())) ? 0 : float.Parse(item["RD_Porc_Util"].ToString());
                    Reg.RD_Fact_Emit = (string.IsNullOrEmpty(item["RD_Fact_Emit"].ToString().Trim())) ? 0 : float.Parse(item["RD_Fact_Emit"].ToString());
                    Reg.RD_Art_Venta = (string.IsNullOrEmpty(item["RD_Art_Venta"].ToString().Trim())) ? 0 : float.Parse(item["RD_Art_Venta"].ToString());
                    Reg.RD_Monto_Venta = (string.IsNullOrEmpty(item["RD_Monto_Venta"].ToString().Trim())) ? 0 : float.Parse(item["RD_Monto_Venta"].ToString());
                    Reg.RD_Cant_Fallas = (string.IsNullOrEmpty(item["RD_Cant_Fallas"].ToString().Trim())) ? 0 : float.Parse(item["RD_Cant_Fallas"].ToString());
                    Reg.RD_Monto_Fallas = (string.IsNullOrEmpty(item["RD_Monto_Fallas"].ToString().Trim())) ? 0 : float.Parse(item["RD_Monto_Fallas"].ToString());

                    Registros.Add(Reg);
                }
            }

        }

        void SendRegistros()
        {

            var client = new RestClient("https://gesfarmonlinefunc.azurewebsites.net");

            var request = new RestRequest("/api/SetDataOperacionesDiario?code=ZmButh_gIWj7RhhvcaN9DoUvs4yCOX49kRuuLlgR0jX2AzFuBNC8VQ==", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            var body = JsonConvert.SerializeObject(Registros);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);


            //var client = new RestClient("http://localhost:7042");
            //var request = new RestRequest("/api/SetDataOperacionesDiario", Method.Post);
            //request.AddHeader("Content-Type", "application/json");
            //var body = JsonConvert.SerializeObject(Registros);
            //request.AddParameter("application/json", body, ParameterType.RequestBody);
            //RestResponse response = client.Execute(request);
            //Console.WriteLine(response.Content);


        }

        string FormatFecha(string anio, string mes, string dia)
        {
            if (mes.Trim().Length == 1)
            {
                mes = "0" + mes;
            }

            if (dia.Trim().Length == 1)
            {
                dia = "0" + dia;
            }


            return anio + "-" + mes + "-" + dia;
        }

    }

}
