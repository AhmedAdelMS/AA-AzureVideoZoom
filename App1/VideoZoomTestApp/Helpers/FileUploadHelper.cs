using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace VideoZoomTestApp.Helpers
{
    public class FileUploadResult
    {
        public string LocalFilePath { get; set; }
        public string FileName { get; set; }
        public long FileLength { get; set; }
    }

    public class FileUploadHelper
    {
        public static async Task UploadFile(List<StorageFile> files, string uploaduri)
        {
            var httpClient = new HttpClient();
            // Read the files
            foreach (StorageFile file in files)
            {
                var fileStream = await file.OpenStreamForReadAsync();

                FileUploadResult uploadResult = null;
                bool _fileUploaded = false;

                var content = new MultipartFormDataContent();
                using (var streamcontent = new StreamContent(fileStream))
                {
                    content.Add(streamcontent, "\"file\"", string.Format("\"{0}\"", file.Name));

                    var taskUpload = httpClient.PostAsync(uploaduri, content).ContinueWith(task =>
                    {
                        if (task.Status == TaskStatus.RanToCompletion)
                        {
                            var response = task.Result;

                            if (response.IsSuccessStatusCode)
                            {
                                //uploadResult = response.Content<FileUploadResult>().Result;
                                if (uploadResult != null)
                                    _fileUploaded = true;

                                // Read other header values if you want..
                                foreach (var header in response.Content.Headers)
                                {
                                }
                            }
                            else
                            {
                            }
                        }
                    });

                    taskUpload.Wait();
                    if (_fileUploaded)
                    {
                    }
                }
            }

            httpClient.Dispose();
        }
    }
}