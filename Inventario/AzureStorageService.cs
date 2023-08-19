using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using StorageService.Models;


namespace StorageService
{
    public class AzureStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _container = "farmafd";


        private readonly string _connectionString = "DefaultEndpointsProtocol=https;AccountName=almcomp;AccountKey=nO4S0uqzMORwaa0a1hU1zDYMfKStUqAplXloykh7XIxjqOY+bYZpPJaYgXkGgyac7OwQjUcgFOky+AStcyDVHA==;EndpointSuffix=core.windows.net";




        public AzureStorageService()
        {
            //var credential = new StorageSharedKeyCredential(_storageAccount, _accessKey);
            _blobServiceClient = new BlobServiceClient(_connectionString);
        }


        public async Task ListBlobContainersAsync()
        {
            var containers = _blobServiceClient.GetBlobContainersAsync();
            await foreach (var container in containers)
            {
                Console.WriteLine(container.Name);
            }
        }

        public async Task<List<Uri>> UploadFileAsync(Stream file, string Carpeta, string Archivo)
        {
            var blobUris = new List<Uri>();
            var blobContainer = _blobServiceClient.GetBlobContainerClient(_container);

            var blob = blobContainer.GetBlobClient($"{Carpeta}/{Archivo}");



            await blob.UploadAsync(file, true);
            blob.SetAccessTier(AccessTier.Hot);
            blobUris.Add(blob.Uri);


            return blobUris;
        }

        public async Task DeleteFileAsync(Archivos Data)
        {

            var blobContainer = _blobServiceClient.GetBlobContainerClient(_container);

            if (Data.ListaArchivos == null)
            {

                foreach (string item in await ListFileNameAsync(Data))
                {
                    var blob = blobContainer.GetBlobClient(item);
                    await blob.DeleteIfExistsAsync();
                }
            }
            else
            {

                foreach (string item in Data.ListaArchivos)
                {
                    var blob = blobContainer.GetBlobClient($"{Data.Carpeta}/{item}");
                    await blob.DeleteIfExistsAsync();
                }
            }

        }

        public async Task<dynamic> DownloadFileAsync(string Carpeta, string Archivo)
        {

            var blobContainer = _blobServiceClient.GetBlobContainerClient(_container);

            var blob = blobContainer.GetBlobClient($"{Carpeta}/{Archivo}");

            if (await blob.ExistsAsync())
            {

                var data = await blob.OpenReadAsync();
                Stream blobContent = data;

                var content = await blob.DownloadContentAsync();

                string name = Archivo;
                string contentType = content.Value.Details.ContentType;

                return new
                {
                    content = blobContent,
                    contentType = contentType,
                    name = name
                };

            }

            return null;

        }

        public async Task<List<string>> ListFileAsync(Archivos Data)
        {
            Data.ListaArchivos = new List<string>();

            var blobContainer = _blobServiceClient.GetBlobContainerClient(_container);

            var blobs = blobContainer.GetBlobs();

            foreach (BlobItem _item in blobs)
            {

                if (Data.Carpeta == null)
                {
                    Data.ListaArchivos.Add(blobContainer.GetBlobClient(_item.Name).Uri.AbsoluteUri);
                }
                else
                {
                    if (_item.Name.Contains($"{Data.Carpeta}/"))
                    {
                        Data.ListaArchivos.Add(blobContainer.GetBlobClient(_item.Name).Uri.AbsoluteUri);
                    }
                }
            }

            return Data.ListaArchivos;

        }

        private async Task<List<string>> ListFileNameAsync(Models.Archivos Data)
        {
            Data.ListaArchivos = new List<string>();

            var blobContainer = _blobServiceClient.GetBlobContainerClient(_container);

            var blobs = blobContainer.GetBlobs();

            foreach (BlobItem _item in blobs)
            {

                if (Data.Carpeta == null)
                {
                    Data.ListaArchivos.Add(_item.Name);
                }
                else
                {
                    if (_item.Name.Contains($"{Data.Carpeta}/"))
                    {
                        Data.ListaArchivos.Add(_item.Name);
                    }
                }
            }

            return Data.ListaArchivos;

        }


    }
}