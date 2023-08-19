using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StorageService.Models;
using StorageService;

namespace GesfarmOnlineFunc.Inventario
{
    public static class GetInventario
    {
        [FunctionName("GetInventario")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
        {


            AzureStorageService azureStorageService = new AzureStorageService();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Archivos data = JsonConvert.DeserializeObject<Archivos>(requestBody);

            return new OkObjectResult(azureStorageService.ListFileAsync(data).GetAwaiter().GetResult());
        }
    }
}
