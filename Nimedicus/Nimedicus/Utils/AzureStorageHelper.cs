using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Win32;
using Prism.Services.Dialogs;
using System.Diagnostics;

namespace Nimedicus.Utils
{

    public class AzureBlobStorageHelper
    {
        private CloudBlobContainer _container;


        private const string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=nimedicus;AccountKey=kGEfsSkR4iL1r1QAds0pzd+4s1tnK//2jUVBIqd5wxH7QaayL//TmwmBnDOtrKEdBxfmJIxvxtG9+AStw7+yJg==;EndpointSuffix=core.windows.net";
        private const string containerName = "nimedicus-files";
        public AzureBlobStorageHelper(string connectionString = storageConnectionString, string containerName = containerName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            _container = blobClient.GetContainerReference(containerName);
        }

        public async Task DownloadFileFromAzure(string blobName, string localFilePath)
        {
            CloudBlockBlob blob = _container.GetBlockBlobReference(blobName);
            await blob.DownloadToFileAsync(localFilePath, FileMode.Create);
        }

        public async Task UploadFileToAzure(string blobName, string localFilePath)
        {
            string fileName = Path.GetFileName(localFilePath);
            CloudBlockBlob blob = _container.GetBlockBlobReference(blobName + Path.GetExtension(fileName));
            await blob.UploadFromFileAsync(localFilePath);
        }

        public async Task DownloadFileFromAzure(string blobName)
        {
            // Создание диалогового окна выбора пути для сохранения файла
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = blobName; // Имя файла по умолчанию
            saveFileDialog.Filter = "All Files (*.*)|*.*"; // Фильтр для выбора всех файлов
            saveFileDialog.Title = "Выберите путь для сохранения файла";

            // Отображение диалогового окна и ожидание выбора пути файла
            if (saveFileDialog.ShowDialog() == true)
            {
                string localFilePath = saveFileDialog.FileName;

                // Получение ссылки на блоб в Azure Storage
                CloudBlockBlob blob = _container.GetBlockBlobReference(blobName);

                // Скачивание файла на локальный путь
                await blob.DownloadToFileAsync(localFilePath, FileMode.Create);


                // Опционально: открытие файла после скачивания
                Process.Start(localFilePath);
            }
        }
    }
}
