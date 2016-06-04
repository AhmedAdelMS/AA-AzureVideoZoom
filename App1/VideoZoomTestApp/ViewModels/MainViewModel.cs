using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoZoomTestApp.Helpers;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace VideoZoomTestApp.ViewModels
{
    public class MainViewModel
    {
        private List<StorageFile> filesToUpload;

        public MainViewModel()
        {
            filesToUpload = new List<StorageFile>();

            SelectFilesCommand = new SimpleCommand(async () => await SelectFile());
            uploadFileButtonCommand = new SimpleCommand(async () => await UploadFile());
        }

        private async Task UploadFile()
        {
            await FileUploadHelper.UploadFile(filesToUpload, "http://localhost:7990/api/VideoAPI/hello");
        }

        private async Task SelectFile()
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".txt");
            var files = await picker.PickMultipleFilesAsync();
            filesToUpload.Clear();
            foreach (StorageFile sf in files)
            {
                filesToUpload.Add(sf);
            }
        }

        private SimpleCommand selectFilesCommand;

        public SimpleCommand SelectFilesCommand
        {
            get { return selectFilesCommand; }
            set { selectFilesCommand = value; }
        }

        private SimpleCommand uploadFileButtonCommand;

        public SimpleCommand UploadFileButtonCommand
        {
            get { return uploadFileButtonCommand; }
            set { uploadFileButtonCommand = value; }
        }
    }
}