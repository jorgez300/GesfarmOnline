namespace StorageService
{
    public interface IStorageService
    {
        Task DeleteFileAsync(string Carpeta, string Archivo);
        Task<dynamic> DownloadFileAsync(string Carpeta, string Archivo);
        Task ListBlobContainersAsync();
        Task<List<string>> ListFileAsync(string? Carpeta = null);
        Task<List<Uri>> UploadFileAsync();
        Task<List<Uri>> UploadFileAsync(Stream file, string Carpeta, string Archivo);
    }
}