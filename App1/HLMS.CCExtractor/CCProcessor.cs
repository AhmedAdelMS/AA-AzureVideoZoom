using Microsoft.WindowsAzure.MediaServices.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HLMS.CCExtractor
{
    public class CCProcessor
    {
        CloudMediaContext _context = null;
        const string _mediaProcessorName = "Azure Media Indexer";
        const string _configurationFile = "<<PLACEHOLDER>>";

        readonly string _accountName = "";
        readonly string _accountKey = "";

        public CCProcessor(string inputfile, string outputfile)
        {
            _context = new CloudMediaContext(_accountName, _accountKey);
            var inputFile = inputfile;
            var outputFolder = outputfile;
            RunIndexingJob(inputFile, outputFolder, _configurationFile);
        }

        private bool RunIndexingJob(string inputFilePath, string outputFolder, string configurationFile = "")
        {
            var asset = _context.Assets.Create("Indexer_Asset", AssetCreationOptions.None);
            var assetFile = asset.AssetFiles.Create(Path.GetFileName(inputFilePath));
            assetFile.Upload(inputFilePath);

            var indexer = GetLatestMediaProcessorByName(_mediaProcessorName);

            var job = _context.Jobs.Create("My Indexing Job");
            var configuration = "";
            if (!String.IsNullOrEmpty(configurationFile))
            {
                configuration = File.ReadAllText(configurationFile);
            }
            var task = job.Tasks.AddNew("Indexing task",
                              indexer,
                              configuration,
                              TaskOptions.None);

            // Specify the input asset to be indexed.
            task.InputAssets.Add(asset);

            // Add an output asset to contain the results of the job.
            task.OutputAssets.AddNew("Indexed video", AssetCreationOptions.None);

            job.StateChanged += Job_StateChanged;
            job.Submit();
            // Check job execution and wait for job to finish.
            var progressPrintTask = new Task(() =>
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
            DownloadAsset(job.OutputMediaAssets.First(), outputFolder);
            return true;
        }

        private void DownloadAsset(IAsset asset, string outputDirectory)
        {
            foreach (IAssetFile file in asset.AssetFiles)
            {
                file.Download(Path.Combine(outputDirectory, file.Name));
            }
        }

        private void Job_StateChanged(object sender, JobStateChangedEventArgs e)
        {
            switch (e.CurrentState)
            {
                case JobState.Finished:
                    // Process output and place into the DB for searching
                    // Inject Results into JSON
                    break;
            }
        }

        private IMediaProcessor GetLatestMediaProcessorByName(string mediaProcessorName)
        {
            var processor = _context.MediaProcessors
                        .Where(p => p.Name == mediaProcessorName)
                        .ToList()
                        .OrderBy(p => new Version(p.Version))
                        .LastOrDefault();

            if (processor == null)
                throw new ArgumentException(string.Format("Unknown media processor", mediaProcessorName));

            return processor;
        }
    }
}