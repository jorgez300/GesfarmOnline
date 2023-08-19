using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StorageService;

namespace GesfarmOnlineFunc.Inventario
{
    public static class SetInventario
    {
        [FunctionName("SetInventario")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
        {

            var formdata = await req.ReadFormAsync();
            var Carpeta = req.Form["Carpeta"].ToString();

            AzureStorageService azureStorageService = new AzureStorageService();

            foreach (IFormFile fileDetails in req.Form.Files)
            {

                azureStorageService.UploadFileAsync(fileDetails.OpenReadStream(), Carpeta, fileDetails.FileName).GetAwaiter().GetResult();

            }

            return new OkObjectResult("ok");

        }
    }
}
