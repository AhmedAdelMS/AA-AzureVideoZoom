using Microsoft.WindowsAzure.MediaServices.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAAZureZoomConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get all needed parms
            Console.WriteLine("Pls enter the URL of the video/audio file: ");
            string fileURL = Console.ReadLine();
            // Upload the video file to Azure media services
            CreateAssetAndUploadSingleFile(AssetCreationOptions.None, "c:\\temp\\abiola.wmv");
            Console.WriteLine("Done! ;-) ");
            Console.ReadLine();
            // Transcribe the video
            // Search the transcription
            // Hyperlapse the video around the kywords

        }

        static public IAsset CreateAssetAndUploadSingleFile(AssetCreationOptions assetCreationOptions, string singleFilePath)
        {
            /*
            if (!File.Exists(singleFilePath))
            {
                Console.WriteLine("File does not exist.");
                return null;
            }
            */
            // Test Comment
            var _context = new CloudMediaContext("aamediaservice1", "uIux27Hig96nRLkGZEZcareBz0x5t2gPPA533riDgTo=");
            var assetName = Path.GetFileNameWithoutExtension(singleFilePath);
            IAsset inputAsset = _context.Assets.Create(assetName, assetCreationOptions);

            var assetFile = inputAsset.AssetFiles.Create(Path.GetFileName(singleFilePath));

            Console.WriteLine("Created assetFile {0}", assetFile.Name);

            var policy = _context.AccessPolicies.Create(
                                    assetName,
                                    TimeSpan.FromDays(30),
                                    AccessPermissions.Write | AccessPermissions.List);

            var locator = _context.Locators.CreateLocator(LocatorType.Sas, inputAsset, policy);

            Console.WriteLine("Upload {0}", assetFile.Name);

            assetFile.Upload(singleFilePath);
            Console.WriteLine("Done uploading {0}", assetFile.Name);

            locator.Delete();
            policy.Delete();

            return inputAsset;
        }
    }

    
    // Check job execution and wait for job to finish. 
    Task progressPrintTask = new Task(() =>
    {
        IJob jobQuery = null;
        do
        {
            var progressContext = new CloudMediaContext(_accountName,
                            _accountKey);
            jobQuery = progressContext.Jobs.Where(j => j.Id == job.Id).First();
            Console.WriteLine(string.Format("{0}\t{1}\t{2}",
                  DateTime.Now,
                  jobQuery.State,
                  jobQuery.Tasks[0].Progress));
            Thread.Sleep(10000);
        }
        while (jobQuery.State != JobState.Finished &&
               jobQuery.State != JobState.Error &&
               jobQuery.State != JobState.Canceled);
    });
    progressPrintTask.Start();

    // Check job execution and wait for job to finish. 
    Task progressJobTask = job.GetExecutionProgressTask(CancellationToken.None);
    progressJobTask.Wait();

    // If job state is Error, the event handling 
    // method for job progress should log errors.  Here you check 
    // for error state and exit if needed.
    if (job.State == JobState.Error)
    {
        Console.WriteLine("Exiting method due to job error.");
        return false;
    }

    // Download the job outputs.
    DownloadAsset(task.OutputAssets.First(), outputFolder);

    return true;
}

// helper function: event handler for Job State
static void StateChanged(object sender, JobStateChangedEventArgs e)
{
    Console.WriteLine("Job state changed event:");
    Console.WriteLine("  Previous state: " + e.PreviousState);
    Console.WriteLine("  Current state: " + e.CurrentState);
    switch (e.CurrentState)
    {
        case JobState.Finished:
            Console.WriteLine();
            Console.WriteLine("Job finished. Please wait for local tasks/downloads");
            break;
        case JobState.Canceling:
        case JobState.Queued:
        case JobState.Scheduled:
        case JobState.Processing:
            Console.WriteLine("Please wait...\n");
            break;
        case JobState.Canceled:
            Console.WriteLine("Job is canceled.\n");
            break;
        case JobState.Error:
            Console.WriteLine("Job failed.\n");
            break;
        default:
            break;
    }
}
// helper method to download the output assets
static void DownloadAsset(IAsset asset, string outputDirectory)
{
    foreach (IAssetFile file in asset.AssetFiles)
    {
        file.Download(Path.Combine(outputDirectory, file.Name));
    }
}
}

