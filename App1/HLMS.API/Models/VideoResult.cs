using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLMS.API.Models
{
    public class VideoResult
    {
        private string videoName;

        public string VideoName
        {
            get { return videoName; }
            set { videoName = value; }
        }

        private string videoDescription;

        public string VideoDescription
        {
            get { return videoDescription; }
            set { videoDescription = value; }
        }

        private Uri originalVideoUri;

        public Uri OriginalVideoUri
        {
            get { return originalVideoUri; }
            set { originalVideoUri = value; }
        }

        private Uri processedVideoUri;

        public Uri ProcessedVideoUri
        {
            get { return processedVideoUri; }
            set { processedVideoUri = value; }
        }
    }
}