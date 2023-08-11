using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OperacionesDiario.Modelo;

namespace GesfarmOnlineFunc.OperacionesDiario
{
    public static class GetDataOperacionesDiario
    {
        [FunctionName("GetDataOperacionesDiario")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            OperacionDiario operacionesDiario = new();

            return new OkObjectResult(operacionesDiario.Get());
        }
    }
}
