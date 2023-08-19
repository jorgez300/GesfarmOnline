using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BASE;
using System.IO;
using Newtonsoft.Json;
using RestSharp;

namespace ServicioInventario
{
    public class InventarioRaw
    {
        public string Origen { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int Existen { get; set; } = 0;
        public decimal CostoBs { get; set; } = 0;
        public decimal CostoUsd { get; set; } = 0;
        public decimal PrecioBs { get; set; } = 0;
        public string IdPa { get; set; } = string.Empty;
        public string PrinAct { get; set; } = string.Empty;
    }

    public class InventarioClean
    {
        public string Codigo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int Existen { get; set; } = 0;
        public decimal CostoBs { get; set; } = 0;
        public decimal CostoUsd { get; set; } = 0;
        public decimal PrecioBs { get; set; } = 0;
        public List<PrinAct> ListPrinAct { get; set; } = new();
    }



    public class PrinAct
    {
        public string Id { get; set; } = string.Empty;
        public string Dsc { get; set; } = string.Empty;
    }

    public class InventarioService
    {
        private string Origen = string.Empty;
        private static string _Url = @"https://gesfarmonlinefunc.azurewebsites.net";
        private static string _EndPoint = @"/api/SetInventario?code=rN-t11VF3ns-eys-edQO31yj1IJLr1BXQaddK8aOpCp8AzFuSvWwqg==";
        private static string _Directorio = @"C:\InformesGesfarm\[Origen].json";


        private List<InventarioRaw> _inventarioRaw = new();
        private List<InventarioClean> _inventarioClean = new();

        public void GetInventario()
        {

            Data db = new Data();
            SqlParameter[] parameters = new SqlParameter[0];
            DataTable DT = db.CallDBList("GFO_INVENTARIO", parameters);

            if (DT.Rows.Count > 0)
            {
                foreach (DataRow item in DT.Rows)
                {
                    InventarioRaw Reg = new InventarioRaw();

                    Origen = item["Origen"].ToString()!;
                    Reg.Codigo = item["Codigo"].ToString()!;
                    Reg.Descripcion = item["Descripcion"].ToString()!;
                    Reg.Existen = int.Parse(item["Existen"].ToString()!);
                    Reg.CostoBs = decimal.Parse(item["CostoBs"].ToString()!);
                    Reg.CostoUsd = decimal.Parse(item["CostoUsd"].ToString()!);
                    Reg.PrecioBs = decimal.Parse(item["PrecioBs"].ToString()!);
                    Reg.IdPa = item["IdPa"].ToString()!;
                    Reg.PrinAct = item["PrinAct"].ToString()!;

                    _inventarioRaw.Add(Reg);
                }
            }
        }

        public void SetInventario()
        {

            foreach (InventarioRaw item in _inventarioRaw)
            {
                if (_inventarioClean.Exists(x => x.Codigo == item.Codigo))
                {
                    if (!_inventarioClean.Find(x => x.Codigo == item.Codigo)!.ListPrinAct.Exists(x => x.Id == item.IdPa))
                    {

                        _inventarioClean.Find(x => x.Codigo == item.Codigo)!.ListPrinAct.Add(new PrinAct
                        {
                            Id = item.IdPa,
                            Dsc = item.PrinAct
                        });

                    }

                }
                else
                {

                    InventarioClean x = new InventarioClean
                    {
                        Codigo = item.Codigo,
                        Descripcion = item.Descripcion,
                        Existen = item.Existen,
                        CostoBs = item.CostoBs,
                        CostoUsd = item.CostoUsd,
                        PrecioBs = item.PrecioBs
                    };

                    x.ListPrinAct.Add(new PrinAct
                    {
                        Id = item.IdPa,
                        Dsc = item.PrinAct
                    });


                    _inventarioClean.Add(x);
                }



            }


        }

        public void SaveJsonInventario()
        {

            File.WriteAllText(_Directorio.Replace("[Origen]", Origen), JsonConvert.SerializeObject(_inventarioClean));

        }

        public void SendJsonInventario()
        {
            var options = new RestClientOptions(_Url)
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest(_EndPoint, Method.Post);
            request.AlwaysMultipartFormData = true;
            request.AddFile("data", _Directorio.Replace("[Origen]", Origen));
            request.AddParameter("Carpeta", "Inventario");
            RestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }


    }


}
