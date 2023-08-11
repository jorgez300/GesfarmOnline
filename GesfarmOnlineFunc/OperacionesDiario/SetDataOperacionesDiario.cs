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
using System.Collections.Generic;

namespace GesfarmOnlineFunc.OperacionesDiario
{
    public static class SetDataOperacionesDiario
    {
        [FunctionName("SetDataOperacionesDiario")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            List<OperacionDiario> data = JsonConvert.DeserializeObject<List<OperacionDiario>>(requestBody);

            OperacionDiario operacionesDiario = new();
            operacionesDiario.Set(data);

            return new OkObjectResult("OK");

        }
    }
}
