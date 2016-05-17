using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
        public void UploadFile(List<string> files, string uploaduri)
        {
            var httpClient = new HttpClient();
            // Read the files
            foreach (String file in files)
            {
                var fileStream = File.Open(file, FileMode.Open);
                var fileInfo = new FileInfo(file);
                FileUploadResult uploadResult = null;
                bool _fileUploaded = false;

                var content = new MultipartFormDataContent();
                using (var streamcontent = new StreamContent(fileStream))
                {
                    content.Add(streamcontent, "\"file\"", string.Format("\"{0}\"", fileInfo.Name));

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

                        fileStream.Dispose();
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